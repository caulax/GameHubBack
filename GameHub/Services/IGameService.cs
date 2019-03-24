using GameHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Services
{
    public enum GameImportResult
    {
        Ok,
        ZipFileNotFound,
        GameJsonNotFound,
        ParseError,
        InvalidResourceId,
        ResourceFileNotFound
    }

    public interface IGameService
    {
        GameImportResult Import(string zipFileName);
        IEnumerable<Game> GetAll();
        Game FindById(Guid id);
        void Delete(Guid id);
    }
}
