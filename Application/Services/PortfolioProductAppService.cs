using Application.Interfaces;
using Domain.Services.Interfaces;
using System;

namespace Application.Services
{
    public class PortfolioProductAppService : IPortfolioProductAppService
    {
        private readonly IPortfolioProductService _portfolioProductService;

        public PortfolioProductAppService(IPortfolioProductService portfolioProductService)
            => _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));

        public void Add(int portfolioId, int productId) => _portfolioProductService.Add(portfolioId, productId);
    }
}
