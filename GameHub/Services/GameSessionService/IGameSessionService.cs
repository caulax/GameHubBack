using GameHub.Entities;
using System;
using System.Collections.Generic;

namespace GameHub.Services.GameSessionService
{
    public interface IGameSessionService
    {
        GameSession Create(Game game, string userName, string name);
        void Remove(Guid sessionId);
        IGameController GetGameController(Guid sessionId);
        IEnumerable<GameSession> FetchGamesList();
    }
}
