using Application.Models.Requests;
using Bogus;
using Domain.Models;
using System.Collections.Generic;

namespace Tests.Factories
{
    public static class PortfolioFactory
    {
        public static List<Portfolio> CreatePortfolios()
        {
            var fakePortfolioNames = new[] { "Minha Casa", "Meu Carro", "Minha filha", "Minha Viagem", "Reserva de emergência", "Aposentadoria" };
            var fakePortfolioProducts = PortfolioProductFactory.CreatePortfolioProduct();

            var fakePortfolios = new Faker<Portfolio>()
                .RuleFor(x => x.Name, x => x.PickRandom(fakePortfolioNames))
                .RuleFor(x => x.Description, x => x.Lorem.Text())
                .RuleFor(x => x.TotalBalance, x => x.Finance.Amount(0, 10000000))
                .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 100))
                .RuleFor(x => x.PortfoliosProducts, fakePortfolioProducts);

            var portfolios = fakePortfolios.Generate(5);
            return portfolios;
        }

        public static Portfolio CreatePortfolio()
        {
            var fakePortfolioNames = new[] { "Minha Casa", "Meu Carro", "Minha filha", "Minha Viagem", "Reserva de emergência", "Aposentadoria" };
            var fakePortfolioProducts = PortfolioProductFactory.CreatePortfolioProduct();

            var fakePortfolios = new Faker<Portfolio>()
                .RuleFor(x => x.Name, x => x.PickRandom(fakePortfolioNames))
                .RuleFor(x => x.Description, x => x.Lorem.Text())
                .RuleFor(x => x.TotalBalance, x => x.Finance.Amount(0, 10000000))
                .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 100))
                .RuleFor(x => x.PortfoliosProducts, x => fakePortfolioProducts);

            var portfolio = fakePortfolios.Generate();
            return portfolio;
        }

        public static CreatePortfolioRequest CreatePortfolioRequest()
        {
            var fakePortfolioNames = new[] { "Minha Casa", "Meu Carro", "Minha filha", "Minha Viagem", "Reserva de emergência", "Aposentadoria" };

            var fakePortfolio = new Faker<CreatePortfolioRequest>()
                .RuleFor(x => x.Name, x => x.PickRandom(fakePortfolioNames))
                .RuleFor(x => x.Description, x => x.Lorem.Text())
                .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 100));

            var portfolio = fakePortfolio.Generate();
            return portfolio;
        }

        public static UpdatePortfolioRequest UpdatePortfolioRequest()
        {
            var fakePortfolioNames = new[] { "Minha Casa", "Meu Carro", "Minha filha", "Minha Viagem", "Reserva de emergência", "Aposentadoria" };

            var fakePortfolio = new Faker<UpdatePortfolioRequest>()
                .RuleFor(x => x.Name, x => x.PickRandom(fakePortfolioNames))
                .RuleFor(x => x.Description, x => x.Lorem.Text());

            var portfolio = fakePortfolio.Generate();
            return portfolio;
        }
    }
}
