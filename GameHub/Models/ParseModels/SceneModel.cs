using System.Collections.Generic;

namespace GameHub.Models.ParseModels
{
    public enum SceneTypeModel
    {
        Normal,
        Result
    }

    public class SceneModel
    {
        public int Id { get; set; }

        public IEnumerable<SpriteModel> Sprites { get; set; }

        public ChooserModel Chooser { get; set; }

        public int BackgroundId { get; set; }

        public string Text { get; set; }

        public SceneTypeModel Type { get; set; }

        public bool WaitForOthers { get; set; }
    }
}
