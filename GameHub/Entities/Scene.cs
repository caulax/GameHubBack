using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Entities
{
    public enum SceneType
    {
        Normal,
        Result
    }

    public class Scene
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public virtual IEnumerable<Sprite> Sprites { get; set; }

        public Guid ChooserId { get; set; }

        public virtual Chooser Chooser { get; set; }

        public Guid BackgroundId { get; set; }

        public string Text { get; set; }

        public SceneType Type { get; set; }

        public bool WaitForOthers { get; set; }
    }
}
