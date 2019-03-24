using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameHub.Entities;

namespace GameHub.Services.GameSessionService
{
    /// <summary>
    /// Контроллер игры. Содержит всю логику, необходимую для проведения одной игры
    /// </summary>
    public class GameController : IGameController
    {
        private readonly List<string> userIds;
        private readonly Dictionary<string, int> userAnswers;
        private readonly Dictionary<string, Guid> userSceneIds;

        /// <summary>
        /// Название игры
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Уникальный идентификатор игры
        /// </summary>
        public Guid SessionId { get; }

        /// <summary>
        /// Никнейм создателя игры
        /// </summary>
        public string CreatorUserName { get; }

        /// <summary>
        /// Сценарий игры
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// Количество игроков, присоединившихся к игре
        /// </summary>
        public int CurrentPlayersCount => userIds.Count;

        /// <summary>
        /// Количество игроков, сделавших выбор в текущем разветвлении сюжета
        /// </summary>
        public int AnsweredCount => userAnswers.Count;

        /// <summary>
        /// Показывает, все ли игроки сделали выбор в текущем разветвлении сюжета
        /// </summary>
        public bool AllPlayersAnswered => CurrentPlayersCount == AnsweredCount;
        
        /// <summary>
        /// Показывает, достаточно ли игроков присоединилось к игре для начала игры
        /// </summary>
        public bool HasEnoughPlayers => CurrentPlayersCount == Game.PlayersCount;

        /// <summary>
        /// Создать новую игру
        /// </summary>
        /// <param name="game">Сценарий игры</param>
        /// <param name="userName">Никнейм создателя игры</param>
        /// <param name="name">Название игры</param>
        public GameController(Game game, string userName, string name)
        {
            SessionId = Guid.NewGuid();
            Game = game;
            CreatorUserName = userName;
            Name = name;
            userIds = new List<string>(game.PlayersCount);
            userAnswers = new Dictionary<string, int>(game.PlayersCount);
            userSceneIds = new Dictionary<string, Guid>(game.PlayersCount);
        }

        /// <summary>
        /// Добавить игрока к игре
        /// </summary>
        /// <param name="playerId">Идентификатор игрока</param>
        /// <returns>True, если игрок был присоединен к игре; False, если свободных мест нет и игрок не был присоединен</returns>
        public bool AddPlayer(string playerId)
        {
            if (HasEnoughPlayers)
            {
                return false;
            }

            userIds.Add(playerId);
            userSceneIds[playerId] = Game.StartSceneId;
            return true;
        }

        /// <summary>
        /// Добавить выбор игрока
        /// </summary>
        /// <param name="playerId">Идентификатор игрока</param>
        /// <param name="answer">Выбор игрока (номер выбранного варианта)</param>
        public void AddPlayerAnswer(string playerId, int answer)
        {
            userAnswers[playerId] = answer;
        }

        /// <summary>
        /// Получить текущую сцену для игрока
        /// </summary>
        /// <param name="playerId">Идентификатор игрока</param>
        /// <returns>Идентификатор сцены</returns>
        public Guid GetPlayerSceneId(string playerId)
        {
            return userSceneIds[playerId];
        }

        /// <summary>
        /// Получить идентификаторы текущих сцен для каждого из игроков, участвующих в данной игре
        /// </summary>
        /// <returns>Идентификаторы текущих сцен для каждого из игроков</returns>
        public IDictionary<string, Guid> GetPlayersSceneIds()
        {
            return userSceneIds.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Получить информацию об игре, собранную в одну структуру данных
        /// </summary>
        /// <returns>Информация об игре</returns>
        public GameSession GetSessionInfo()
        {
            return new GameSession
            {
                Id = SessionId,
                CreatorUserName = CreatorUserName,
                CurrentPlayersCount = CurrentPlayersCount,
                MaxPlayersCount = Game.PlayersCount,
                ScenarioId = Game.Id,
                ScenarioName = Game.Name,
                Name = Name
            };
        }

        /// <summary>
        /// Переключить сцены для каждого из игроков на основании сделанных игроками выборов
        /// </summary>
        public void Next()
        {
            for (var i = 0; i < userIds.Count; i++)
            {
                var userId = userIds[i];
                var matrixPosition = string.Join('-', i == 0 ? userAnswers.Values : userAnswers.Values.Reverse());

                userSceneIds[userId] = Game.Scenes
                    .Single(x => x.Id == userSceneIds[userId])
                    .Chooser.ChoiceMatrix
                    .Single(x => x.MatrixPosition == matrixPosition)
                    .NextSceneId;
            }

            userAnswers.Clear();
        }
    }
}
