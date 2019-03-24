using GameHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Services.GameSessionService
{
    public interface IGameController
    {
        int CurrentPlayersCount { get; }
        bool AllPlayersAnswered { get; }
        int AnsweredCount { get; }
        bool HasEnoughPlayers { get; }
        Game Game { get; }

        GameSession GetSessionInfo();
        bool AddPlayer(string playerId);
        void AddPlayerAnswer(string playerId, int answer);
        void Next();
        Guid GetPlayerSceneId(string playerId);
        IDictionary<string, Guid> GetPlayersSceneIds();
    }
}
