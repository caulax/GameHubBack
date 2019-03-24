using AutoMapper;
using GameHub.Entities;
using GameHub.Models.ViewModels;
using GameHub.Services;
using GameHub.Services.GameSessionService;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Hubs
{
    public class GameSessionsHub : Hub
    {
        private const string CLIENT_SESSIONID_FIELD = "SessionId";
        private const string CLIENT_USERNAME_FIELD = "UserName";

        class RequestResult
        {
            public object Data { get; set; }
            public object Error { get; set; }

            public static RequestResult Ok(object data)
            {
                return new RequestResult { Data = data };
            }

            public static RequestResult Ok()
            {
                return Ok("");
            }

            public static RequestResult Err(object error)
            {
                return new RequestResult { Error = error };
            }

            public static RequestResult Err()
            {
                return Err("");
            }
        }

        private readonly IGameService gameService;
        private readonly IGameSessionService gameSessionService;
        private readonly IMapper mapper;
        
        public GameSessionsHub(IGameService gameService, IGameSessionService gameSessionService, IMapper mapper)
        {
            this.gameService = gameService;
            this.gameSessionService = gameSessionService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить идентификатор игры, к которой присоединен текущий игрок
        /// </summary>
        /// <returns>Индентификатор игры</returns>
        private Guid GetCurrentSessionId()
        {
            if (Context.Items.TryGetValue(CLIENT_SESSIONID_FIELD, out var val) && val is Guid sessionId)
            {
                return sessionId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Получить контроллер игры, к которой присоединен текущий игрок
        /// </summary>
        /// <returns>Контроллер игры</returns>
        private IGameController GetCurrentController()
        {
            var sessionId = GetCurrentSessionId();
            return sessionId == Guid.Empty ? null : gameSessionService.GetGameController(sessionId);
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Авторизоваться на игровом сервере
        /// </summary>
        /// <param name="userName">Никнейм игрока</param>
        public async Task Authorization(string userName)
        {
            Context.Items[CLIENT_USERNAME_FIELD] = userName;
            await Clients.Caller.SendAsync(nameof(Authorization), RequestResult.Ok(userName));
        }

        /// <summary>
        /// Получить список доступных сценариев
        /// </summary>
        public async Task FetchScenarioList()
        {
            var scenarioes = gameService.GetAll();
            var scenarioModels = mapper.Map<IEnumerable<ScenarioViewModel>>(scenarioes);

            await Clients.Caller.SendAsync(nameof(FetchScenarioList), RequestResult.Ok(scenarioModels));
        }

        /// <summary>
        /// Получить список активных игр
        /// </summary>
        public async Task FetchGamesList()
        {
            var games = gameSessionService.FetchGamesList();
            await Clients.Caller.SendAsync(nameof(FetchGamesList), RequestResult.Ok(games));
        }

        /// <summary>
        /// Создать игру
        /// </summary>
        /// <param name="createGameRequest">Описание создаваемой игры</param>
        public async Task CreateGame(CreateGameRequestViewModel createGameRequest)
        {
            var game = gameService.FindById(createGameRequest.ScenarioId);

            if (game == null)
            {
                await Clients.Caller.SendAsync(nameof(CreateGame), RequestResult.Err("Invalid scenario id"));
                return;
            }

            var userName = (string)Context.Items[CLIENT_USERNAME_FIELD];
            var gameSession = gameSessionService.Create(game, userName, createGameRequest.Name);

            await Clients.Caller.SendAsync(nameof(CreateGame), RequestResult.Ok(gameSession.Id));
            await Clients.All.SendAsync("GameCreated", RequestResult.Ok(gameSession));
        }

        /// <summary>
        /// Подключиться к игре
        /// </summary>
        /// <param name="sessionId">Идентификатор игры</param>
        public async Task ConnectToGame(Guid sessionId)
        {
            var group = sessionId.ToString();
            var gameController = gameSessionService.GetGameController(sessionId);

            if (gameController == null)
            {
                await Clients.Caller.SendAsync(nameof(ConnectToGame), RequestResult.Err("Game does not exist"));
                return;
            }

            gameController.AddPlayer(Context.ConnectionId);
            Context.Items[CLIENT_SESSIONID_FIELD] = sessionId;

            var gameModel = mapper.Map<GameViewModel>(gameController.Game);
            gameModel.CurrentPlayersCount = gameController.CurrentPlayersCount;

            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Caller.SendAsync(nameof(ConnectToGame), RequestResult.Ok(gameModel));
            await Clients.All.SendAsync(nameof(FetchGamesList), RequestResult.Ok(gameSessionService.FetchGamesList()));
            await Clients.Group(group).SendAsync("PlayerConnected", RequestResult.Ok(gameController.CurrentPlayersCount));
        }

        /// <summary>
        /// Отправить выбор игрока
        /// </summary>
        /// <param name="answer">Выбор игрока (номер выбранного варианта)</param>
        public async Task SendAnswer(int answer)
        {
            var gameController = GetCurrentController();

            if (gameController == null)
            {
                await Clients.Caller.SendAsync(nameof(SendAnswer), "Player is not connected to a game");
                return;
            }

            gameController.AddPlayerAnswer(Context.ConnectionId, answer);

            var group = GetCurrentSessionId().ToString();

            await Clients.Caller.SendAsync(nameof(SendAnswer), RequestResult.Ok());
            await Clients.Group(group).SendAsync("AnsweredCount", RequestResult.Ok(gameController.AnsweredCount));

            if (gameController.AllPlayersAnswered)
            {
                gameController.Next();

                foreach (var playerData in gameController.GetPlayersSceneIds())
                {
                    var scene = gameController.Game.Scenes.First(x => x.Id == playerData.Value);
                    var sceneModel = mapper.Map<SceneViewModel>(scene);

                    await Clients.Client(playerData.Key).SendAsync(nameof(SendAnswer), RequestResult.Ok(sceneModel));
                }
            }
        }
    }
}
