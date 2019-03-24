using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Models.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int PlayersCount { get; set; }
        public int CurrentPlayersCount { get; set; }
        public SceneViewModel StartScene { get; set; }
    }
}
