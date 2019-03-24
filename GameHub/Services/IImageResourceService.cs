using GameHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Services
{
    public interface IImageResourceService
    {
        ImageResource FindById(Guid id);
    }
}
