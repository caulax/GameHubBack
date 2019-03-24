using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameHub.Entities;

namespace GameHub.Services.GameSessionService
{
    public class GameSessionService : IGameSessionService
    {
        private readonly ConcurrentDictionary<Guid, IGameController> sessionControllers;
        private readonly ConcurrentDictionary<string, Guid> usersToSessions;

        public GameSessionService(IGameService gameService)
        {
            sessionControllers = new ConcurrentDictionary<Guid, IGameController>();
            usersToSessions = new ConcurrentDictionary<string, Guid>();
        }

        public GameSession Create(Game game, string userName, string name)
        {
            var sessionController = new GameController(game, userName, name);
            sessionControllers[sessionController.SessionId] = sessionController;
            return sessionController.GetSessionInfo();
        }

        public void Remove(Guid sessionId)
        {
            sessionControllers.Remove(sessionId, out _);
        }

        public IEnumerable<GameSession> FetchGamesList()
        {
            return sessionControllers.Values.Select(x => x.GetSessionInfo());
        }

        public IGameController GetGameController(Guid sessionId)
        {
            if (sessionControllers.TryGetValue(sessionId, out var gameController))
            {
                return gameController;
            }
            else
            {
                return null;
            }
        }
    }
}
