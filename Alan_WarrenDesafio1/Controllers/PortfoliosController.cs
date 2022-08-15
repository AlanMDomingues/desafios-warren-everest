using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Alan_WarrenDesafio1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllersBase<PortfoliosController>
    {
        private readonly IPortfolioAppService _portfolioAppService;

        public PortfoliosController(
            IPortfolioAppService portfolioAppService,
            ILogger<PortfoliosController> logger
        ) : base(logger)
            => _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));

        [HttpGet("get-all/{id}")]
        public IActionResult GetAll(int id)
        {
            return SafeAction(() =>
            {
                return !_portfolioAppService.GetAll(id).Any()
                ? NotFound()
                : Ok(_portfolioAppService.GetAll(id));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return SafeAction(() =>
            {
                return _portfolioAppService.Get(id) is null
                ? BadRequest()
                : Ok(_portfolioAppService.Get(id));
            });
        }

        [HttpPost]
        public IActionResult Post(CreatePortfolioRequest portfolio)
        {
            return SafeAction(() =>
            {
                var result = _portfolioAppService.Add(portfolio);
                return !result
                    ? NotFound()
                    : Ok(result);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdatePortfolioRequest portfolio)
        {
            return SafeAction(() =>
            {
                return !_portfolioAppService.Update(id, portfolio)
                    ? NotFound()
                    : Ok(_portfolioAppService.Update(id, portfolio));
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return SafeAction(() =>
            {
                return !_portfolioAppService.Delete(id)
                    ? NotFound()
                    : Ok(_portfolioAppService.Delete(id));
            });
        }
    }
}
