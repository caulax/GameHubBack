using System.Collections.Generic;

namespace GameHub.Models.ParseModels
{
    public class GameModel
    {
        public string Name { get; set; }

        public int PlayersCount { get; set; }

        public int StartSceneId { get; set; }

        public IEnumerable<ImageResourceModel> ImageResources { get; set; }

        public IEnumerable<SceneModel> Scenes { get; set; }
    }
}
