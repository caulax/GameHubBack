using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Entities
{
    public class Sprite
    {
        public Guid Id { get; set; }

        public Guid SceneId { get; set; }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public Guid ResourceId { get; set; }
    }
}
