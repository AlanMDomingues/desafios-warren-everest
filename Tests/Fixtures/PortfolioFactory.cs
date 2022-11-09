using Application.Models.Requests;
using Bogus;
using Domain.Models;
using System.Collections.Generic;

namespace API.Tests.Fixtures
{
    public static class PortfolioFactory
    {
        private static readonly string[] FakePortfolioNames = new[] { "Minha Casa", "Meu Carro", "Minha filha", "Minha Viagem", "Reserva de emergência", "Aposentadoria" };

        public static List<Portfolio> FakePortfolios()
        {
            var fakePortfolioProducts = PortfolioProductFactory.FakePortfolioProduct();

            var fakePortfolios = new Faker<Portfolio>()
                .CustomInstantiator(x => new Portfolio(x.PickRandom(FakePortfolioNames), x.Random.Words(4)))
                .RuleFor(x => x.Id, x => ++x.IndexVariable)
                .RuleFor(x => x.TotalBalance, x => x.Finance.Amount(0, 10000000))
                .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 100))
                .RuleFor(x => x.PortfoliosProducts, fakePortfolioProducts);

            var portfolios = fakePortfolios.Generate(5);
            return portfolios;
        }

        public static Portfolio FakePortfolio()
        {
            var fakePortfolioProducts = PortfolioProductFactory.FakePortfolioProduct();

            var fakePortfolios = new Faker<Portfolio>()
                .CustomInstantiator(x => new Portfolio(x.PickRandom(FakePortfolioNames), x.Random.Words(4)))
                .RuleFor(x => x.Id, x => ++x.IndexVariable)
                .RuleFor(x => x.TotalBalance, x => x.Finance.Amount(0, 10000000))
                .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 100))
                .RuleFor(x => x.PortfoliosProducts, x => fakePortfolioProducts);

            var portfolio = fakePortfolios.Generate();
            return portfolio;
        }

        public static CreatePortfolioRequest FakeCreatePortfolioRequest()
        {
            var fakePortfolio = new Faker<CreatePortfolioRequest>()
                .RuleFor(x => x.Name, x => x.PickRandom(FakePortfolioNames))
                .RuleFor(x => x.Description, x => x.Random.Words(4))
                .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 100));

            var portfolio = fakePortfolio.Generate();
            return portfolio;
        }

        public static UpdatePortfolioRequest FakeUpdatePortfolioRequest()
        {
            var fakePortfolio = new Faker<UpdatePortfolioRequest>()
                .RuleFor(x => x.Name, x => x.PickRandom(FakePortfolioNames))
                .RuleFor(x => x.Description, x => x.Random.Words(4))
                .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 100));

            var portfolio = fakePortfolio.Generate();
            return portfolio;
        }
    }
}
