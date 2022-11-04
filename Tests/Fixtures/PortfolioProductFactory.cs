using Bogus;
using Domain.Models;
using System.Collections.Generic;

namespace API.Tests.Fixtures
{
    public static class PortfolioProductFactory
    {
        public static ICollection<PortfolioProduct> FakePortfolioProduct()
        {
            var fakePortfolioProduct = new Faker<PortfolioProduct>()
                .CustomInstantiator(x => new PortfolioProduct(x.Random.Int(1, 10000), x.Random.Int(1, 10000)))
                .RuleFor(x => x.Id, x => ++x.IndexVariable);

            var portfolioProduct = fakePortfolioProduct.Generate(1);

            return portfolioProduct;
        }
    }
}
