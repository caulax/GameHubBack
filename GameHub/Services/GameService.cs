using GameHub.Entities;
using GameHub.Models.ParseModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Services
{
    public class GameService : IGameService
    {
        public const string GAME_JSON_FILE_NAME = "game.json";
        //public const string RESOURCES_DIR_NAME = "resources";

        private readonly GameHubContext context;

        public GameService(GameHubContext context)
        {
            this.context = context;
        }

        public void Delete(Guid id)
        {
            var game = context.Games.Find(id);

            if (game == null)
            {
                return;
            }

            context.Games.Remove(game);
            context.SaveChanges();
        }

        public IEnumerable<Game> GetAll()
        {
            return context.Games.ToList();
        }

        public Game FindById(Guid id)
        {
            return context.Games.Find(id);
        }

        private Game MapGameModel(GameModel gameModel)
        {
            var resourceIdsMapper = gameModel.ImageResources.ToDictionary(x => x.Id, x => Guid.NewGuid());
            var sceneIdsMapper = gameModel.Scenes.ToDictionary(x => x.Id, x => Guid.NewGuid());

            var game = new Game
            {
                Name = gameModel.Name,
                PlayersCount = gameModel.PlayersCount,
                StartSceneId = sceneIdsMapper[gameModel.StartSceneId],
                ImageResources = gameModel.ImageResources.Select(x => new ImageResource
                {
                    Id = resourceIdsMapper[x.Id],
                    Path = x.Path
                }).ToArray(),
                Scenes = gameModel.Scenes.Select(x => new Scene
                {
                    Id = sceneIdsMapper[x.Id],
                    BackgroundId = resourceIdsMapper[x.BackgroundId],
                    Type = x.Type == SceneTypeModel.Normal ? SceneType.Normal : SceneType.Result,
                    Text = x.Text,
                    WaitForOthers = x.WaitForOthers,
                    Sprites = x.Sprites?.Select(z => new Sprite
                    {
                        ResourceId = resourceIdsMapper[z.ResourceId],
                        PositionX = z.PositionX,
                        PositionY = z.PositionY
                    })?.ToArray() ?? new Sprite[0],
                    Chooser = new Chooser
                    {
                        Choices = x.Chooser?.Choices?.Select(z => new Choice
                        {
                            Id = z.Id,
                            Caption = z.Caption
                        })?.ToArray() ?? new Choice[0],
                        ChoiceMatrix = x.Chooser?.ChoiceMatrix?.Select(z => new MatrixVariant
                        {
                            MatrixPosition = z.MatrixPosition,
                            NextSceneId = sceneIdsMapper[z.NextSceneId],
                            Value = z.Value
                        })?.ToArray() ?? new MatrixVariant[0]
                    }
                }).ToArray()
            };

            return game;
        }

        public GameImportResult Import(string zipFileName)
        {
            // TODO: format text: split to phrases, character names

            if (!File.Exists(zipFileName))
            {
                return GameImportResult.ZipFileNotFound;
            }

            var unpackedPath = Path.GetTempPath() + Path.GetRandomFileName();
            ZipFile.ExtractToDirectory(zipFileName, unpackedPath);
            var gameJsonFileName = unpackedPath + "\\" + GAME_JSON_FILE_NAME;

            if (!File.Exists(gameJsonFileName))
            {
                return GameImportResult.GameJsonNotFound;
            }

            var gameJson = File.ReadAllText(gameJsonFileName);

            GameModel gameModel;

            try
            {
                gameModel = JsonConvert.DeserializeObject<GameModel>(gameJson);
            }
            catch
            {
                return GameImportResult.ParseError;
            }

            var game = MapGameModel(gameModel);

            //using (var writer = new StreamWriter(@"C:\Users\User\Desktop\testgame\game.txt"))
            //{
            //    JsonSerializer.Create().Serialize(writer, game);
            //}

            foreach (var resource in game.ImageResources)
            {
                var resourceFilePath = unpackedPath + resource.Path;

                if (!File.Exists(resourceFilePath))
                {
                    return GameImportResult.ResourceFileNotFound;
                }

                resource.Image = File.ReadAllBytes(resourceFilePath);
            }

            context.Games.Add(game);
            context.SaveChanges();

            return GameImportResult.Ok;
        }
    }
}
