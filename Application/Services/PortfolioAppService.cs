using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Response;
using AutoMapper;
using Domain.Models;
using Domain.Services.Interfaces;
using System.Collections.Generic;

namespace Application.Services
{
    public class PortfolioAppService : IPortfolioAppService
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IMapper _mapper;

        public PortfolioAppService(IPortfolioService portfolioService, IMapper mapper)
        {
            _portfolioService = portfolioService;
            _mapper = mapper;
        }

        public IEnumerable<PortfolioResult> GetAll(int id)
        {
            var portfolio = _portfolioService.GetAll(id);
            var result = _mapper.Map<IEnumerable<PortfolioResult>>(portfolio);
            return result;
        }

        public PortfolioResult Get(int id)
        {
            var portfolio = _portfolioService.Get(id);
            var result = _mapper.Map<PortfolioResult>(portfolio);
            return result;
        }

        public bool Add(CreatePortfolioRequest portfolio)
        {
            var portfolioToCreate = _mapper.Map<Portfolio>(portfolio);
            return _portfolioService.Add(portfolioToCreate);
        }

        public bool Update(int id, UpdatePortfolioRequest portfolio)
        {
            var portfolioToUpdate = _mapper.Map<Portfolio>(portfolio);
            portfolioToUpdate.Id = id;
            return _portfolioService.Update(portfolioToUpdate);
        }

        public bool Delete(int id) => _portfolioService.Delete(id);
    }
}
