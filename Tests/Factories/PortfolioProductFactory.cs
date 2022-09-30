using Bogus;
using Domain.Models;
using System.Collections.Generic;

namespace Tests.Factories
{
    public static class PortfolioProductFactory
    {
        public static ICollection<PortfolioProduct> CreatePortfolioProduct()
        {
            var fakePortfolioProduct = new Faker<PortfolioProduct>()
                .RuleFor(x => x.PortfolioId, x => x.Random.Int(1, 10000))
                .RuleFor(x => x.ProductId, x => x.Random.Int(1, 10000));

            var portfolioProduct = fakePortfolioProduct.Generate(1);

            return portfolioProduct;
        }
    }
}
