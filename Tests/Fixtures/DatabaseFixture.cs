using EntityFrameworkCore.UnitOfWork;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Z.EntityFramework.Extensions;

namespace API.Tests.Fixtures
{
    [CollectionDefinition(nameof(DatabaseFixtureCollection))]
    public sealed class DatabaseFixtureCollection : ICollectionFixture<DatabaseFixture>
    { }

    public class DatabaseFixture
    {
        public UnitOfWork<DataContext> CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;
            var dbContext = new DataContext(dbContextOptions);

            EntityFrameworkManager.ContextFactory = _ => dbContext;

            dbContext.Database.EnsureCreated();

            return new UnitOfWork<DataContext>(dbContext);
        }
    }
}
