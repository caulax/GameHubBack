using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameHub.Entities;

namespace GameHub.Services
{
    public class ImageResourceService : IImageResourceService
    {
        private readonly GameHubContext context;

        public ImageResourceService(GameHubContext context)
        {
            this.context = context;
        }

        public ImageResource FindById(Guid id)
        {
            return context.ImageResources.Find(id);
        }
    }
}
