using Bogus;
using Domain.Models;
using System.Collections.Generic;

namespace Tests.Factories
{
    public static class ProductFactory
    {
        public static List<Product> CreateProduct()
        {
            var fakeSymbols = new List<string>() { "VIIA3", "CEDO4F", "NFLX34F", "NIKE34F", "MCDC34F", "AMZO34F", "RDNI3F", "SLED4F", "LREN3F", "MGLU3F" };
            var fakeProduct = new Faker<Product>()
                .RuleFor(x => x.Symbol, x => x.PickRandom(fakeSymbols))
                .RuleFor(x => x.UnitPrice, x => x.Finance.Amount());

            var product = fakeProduct.Generate(5);

            return product;
        }
    }
}
