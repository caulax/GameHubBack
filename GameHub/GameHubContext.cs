using GameHub.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub
{
    public class GameHubContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<ImageResource> ImageResources { get; set; }

        public DbSet<Scene> Scenes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=gamehub", builder =>
            //{
            //    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            //});
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=gamehub");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
