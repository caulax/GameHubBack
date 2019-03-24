using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameHub.Models;
using GameHub.Services;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GameHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService gameService;
        private readonly IImageResourceService imageResourceService;

        public HomeController(IGameService gameService, IImageResourceService imageResourceService)
        {
            this.gameService = gameService;
            this.imageResourceService = imageResourceService;
        }

        public IActionResult Index()
        {
            var games = gameService.GetAll();
            return View(games);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            if (Path.GetExtension(file.FileName) != ".zip")
            {
                return BadRequest("Only zip files are supported.");
            }

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var importResult = gameService.Import(filePath);

            switch (importResult)
            {
                case GameImportResult.ZipFileNotFound:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Zip file is lost.");
                case GameImportResult.GameJsonNotFound:
                    return BadRequest("Missing game json file.");
                case GameImportResult.ParseError:
                    return BadRequest("Invalid game json format.");
                case GameImportResult.InvalidResourceId:
                    return BadRequest("Invalid resource id.");
                case GameImportResult.ResourceFileNotFound:
                    return BadRequest("Missing resource file.");
                case GameImportResult.Ok:
                    return RedirectToAction("Index");
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public IActionResult Delete(Guid id)
        {
            var game = gameService.FindById(id);

            if (game == null)
            {
                return NotFound("Game not found.");
            }

            return View(game);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(Guid id)
        {
            gameService.Delete(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/image/{id}")]
        public IActionResult ImageResource(Guid id)
        {
            var resource = imageResourceService.FindById(id);
            string type;

            switch (Path.GetExtension(resource.Path).ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    type = "image/jpeg";
                    break;
                case ".png":
                    type = "image/png";
                    break;
                default:
                    throw new FormatException("This image format not supported");
            }

            return File(resource.Image, type);
        }
    }
}
