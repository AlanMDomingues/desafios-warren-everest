using Application.Interfaces;
using Application.Models.Requests;
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
            ILogger<PortfoliosController> logger)
            : base(logger)
            => _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));

        [HttpGet("get-all-portfolios-by-a-customer/{id}")]
        public IActionResult GetAll(int id)
        {
            return SafeAction(() =>
            {
                var results = _portfolioAppService.GetAll(id);

                return !results.Any()
                    ? NotFound()
                    : Ok(results);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return SafeAction(() =>
            {
                var result = _portfolioAppService.Get(id);

                return result is null
                    ? BadRequest()
                    : Ok(result);
            });
        }

        [HttpPost]
        public IActionResult Post(CreatePortfolioRequest portfolio)
        {
            return SafeAction(() =>
            {
                _portfolioAppService.Add(portfolio);

                return Created("~api/portfolio", default);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdatePortfolioRequest portfolio)
        {
            return SafeAction(() =>
            {
                _portfolioAppService.Update(id, portfolio);

                return Ok();
            });
        }

        [HttpPut("transfer-money-to-portfolio/{portfolioId}")]
        public IActionResult TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal cash)
        {
            return SafeAction(() =>
            {
                var (status, message) = _portfolioAppService.TransferMoneyToPortfolio(customerBankInfoId, portfolioId, cash);

                return !status
                    ? BadRequest(message)
                    : Ok();
            });
        }

        [HttpPut("transfer-money-to-account-balance/{portfolioId}")]
        public IActionResult TransferMoneyToAccountBalance(int customerBankInfoId, int portfolioId, decimal cash)
        {
            return SafeAction(() =>
            {
                var (status, message) = _portfolioAppService.TransferMoneyToAccountBalance(customerBankInfoId, portfolioId, cash);

                return !status
                    ? BadRequest(message)
                    : Ok();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return SafeAction(() =>
            {
                var (status, message) = _portfolioAppService.Delete(id);

                return !status
                    ? BadRequest(message)
                    : NoContent();
            });
        }
    }
}
