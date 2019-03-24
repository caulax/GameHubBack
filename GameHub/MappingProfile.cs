using AutoMapper;
using GameHub.Entities;
using GameHub.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub
{
    public class MappingProfile : Profile
    {
        const string IMAGES_URL = "/image/";

        public MappingProfile()
        {
            CreateMap<Choice, ChoiceViewModel>();

            CreateMap<Sprite, SpriteViewModel>()
                .ForMember(vm => vm.ImageUrl, o => o.MapFrom(s => IMAGES_URL + s.ResourceId));

            CreateMap<Scene, SceneViewModel>()
                .ForMember(vm => vm.BackgroundUrl, o => o.MapFrom(s => IMAGES_URL + s.BackgroundId))
                .ForMember(vm => vm.Phrases, o => o.MapFrom(s => ParseSceneText(s.Text)))
                .ForMember(vm => vm.Choices, o => o.MapFrom(s => s.Chooser.Choices))
                .ForMember(vm => vm.Type, o => o.MapFrom(s => s.Type.ToString()));

            CreateMap<Game, GameViewModel>()
                .ForMember(vm => vm.StartScene, o => o.MapFrom(g => g.Scenes.First(x => x.Id == g.StartSceneId)))
                .ForMember(vm => vm.CurrentPlayersCount, o => o.Ignore());

            CreateMap<Game, ScenarioViewModel>();
        }

        private static IEnumerable<PhraseViewModel> ParseSceneText(string text)
        {
            var parts = (IEnumerable<string>)text.Split('|');

            while (parts.Any())
            {
                var pair = parts.Take(2).ToArray();
                parts = parts.Skip(2);
                yield return new PhraseViewModel { ActorName = pair[0], Text = pair[1] };
            }
        }
    }
}
