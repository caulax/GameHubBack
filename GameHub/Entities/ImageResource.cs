using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Entities
{
    public class ImageResource
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public string Path { get; set; }

        public byte[] Image { get; set; }
    }
}
