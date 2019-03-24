using System;

namespace GameHub.Services.GameSessionService
{
    public class GameSession
    {
        public Guid Id { get; set; }
        public Guid ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public string Name { get; set; }
        public string CreatorUserName { get; set; }
        public int MaxPlayersCount { get; set; }
        public int CurrentPlayersCount { get; set; }
    }
}
