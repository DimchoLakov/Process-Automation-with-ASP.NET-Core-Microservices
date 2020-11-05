using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyOnlineShop.Catalog.Profiles;
using MyOnlineShop.Ordering.Data;
using System;

namespace MyOnlineShop.Catalog.Tests
{
    public class BaseTesting
    {
        public BaseTesting()
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(config => config.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            this.CatalogDbContext = new CatalogDbContext(options);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CatalogProfile());
            });
            Mapper = mapperConfiguration.CreateMapper();
        }

        protected CatalogDbContext CatalogDbContext { get; }
        
        protected IMapper Mapper { get; }
    }
}
