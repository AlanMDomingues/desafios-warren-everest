using Application.Interfaces;
using Domain.Models;
using Domain.Services.Interfaces;
using System;

namespace Application.Services
{
    public class PortfolioProductAppService : IPortfolioProductAppService
    {
        private readonly IPortfolioProductService _portfolioProductService;

        public PortfolioProductAppService(IPortfolioProductService portfolioProductService)
            => _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));

        public void Add(int portfolioId, int productId)
        {
            var portfolioProduct = new PortfolioProduct(portfolioId, productId);
            _portfolioProductService.Add(portfolioProduct);
        }
    }
}
