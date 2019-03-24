using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Entities
{
    public class MatrixVariant
    {
        public Guid Id { get; set; }

        public Guid ChooserId { get; set; }

        public string MatrixPosition { get; set; }

        public int Value { get; set; }

        public Guid NextSceneId { get; set; }
    }
}
