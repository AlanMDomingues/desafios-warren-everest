using Application.Models.Requests;
using Bogus;
using Domain.Models;
using System.Collections.Generic;

namespace Tests.Factories
{
    public static class ProductFactory
    {
        public static readonly string[] FakeSymbols = new[] { "VIIA3", "CEDO4F", "NFLX34F", "NIKE34F", "MCDC34F", "AMZO34F", "RDNI3F", "SLED4F", "LREN3F", "MGLU3F" };

        public static List<Product> FakeProducts()
        {
            var fakeProduct = new Faker<Product>()
                .CustomInstantiator(x => new Product(x.PickRandom(FakeSymbols), x.Finance.Amount()))
                .RuleFor(x => x.Id, x => ++x.IndexVariable);

            var product = fakeProduct.Generate(5);

            return product;
        }

        public static Product FakeProduct()
        {
            var fakeProduct = new Faker<Product>()
                .CustomInstantiator(x => new Product(x.PickRandom(FakeSymbols), x.Finance.Amount()))
                .RuleFor(x => x.Id, x => ++x.IndexVariable);

            var product = fakeProduct.Generate();

            return product;
        }

        public static CreateProductRequest FakeCreateProductRequest()
        {
            var fakeProduct = new Faker<CreateProductRequest>()
                .RuleFor(x => x.Symbol, x => x.PickRandom(FakeSymbols))
                .RuleFor(x => x.UnitPrice, x => x.Finance.Amount());

            var product = fakeProduct.Generate();

            return product;
        }

        public static UpdateProductRequest FakeUpdateProductRequest()
        {
            var fakeProduct = new Faker<UpdateProductRequest>()
                .RuleFor(x => x.Symbol, x => x.PickRandom(FakeSymbols))
                .RuleFor(x => x.UnitPrice, x => x.Finance.Amount());

            var product = fakeProduct.Generate();

            return product;
        }
    }
}
