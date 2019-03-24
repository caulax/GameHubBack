using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Entities
{
    public class Game
    {
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Количество игроков")]
        public int PlayersCount { get; set; }

        public Guid StartSceneId { get; set; }

        public virtual ICollection<ImageResource> ImageResources { get; set; }

        public virtual ICollection<Scene> Scenes { get; set; }
    }
}
