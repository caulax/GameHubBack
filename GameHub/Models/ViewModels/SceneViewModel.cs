using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Models.ViewModels
{
    public class SceneViewModel
    {
        public Guid Id { get; set; }
        public string BackgroundUrl { get; set; }
        public IEnumerable<PhraseViewModel> Phrases { get; set; }
        public IEnumerable<SpriteViewModel> Sprites { get; set; }
        public IEnumerable<ChoiceViewModel> Choices { get; set; }
        public string Type { get; set; }
    }
}
