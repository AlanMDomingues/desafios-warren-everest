using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Alan_WarrenDesafio1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersBankInfoController : ControllersBase<CustomersBankInfoController>
    {
        private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

        public CustomersBankInfoController(
            ICustomerBankInfoAppService customerBankInfoAppService,
            ILogger<CustomersBankInfoController> logger)
            : base(logger)
            => _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return SafeAction(() =>
            {
                var result = _customerBankInfoAppService.Get(id);

                return result is null
                    ? NotFound($"CustomerBankInfo not found for Id: {id}")
                    : Ok(result);
            });
        }

        [HttpPatch("deposit/{id}")]
        public IActionResult Deposit(int id, decimal amount)
        {
            return SafeAction(() =>
            {
                _customerBankInfoAppService.Deposit(id, amount);

                return Ok();
            });
        }

        [HttpPatch("withdraw/{id}")]
        public IActionResult Withdraw(int id, decimal amount)
        {
            return SafeAction(() =>
            {
                _customerBankInfoAppService.Withdraw(id, amount);

                return Ok();
            });
        }

        [HttpPatch("transfer/{portfolioId}")]
        public IActionResult TransferMoneyToPortfolio(int customerBankInfoId, int portfolioId, decimal amount)
        {
            return SafeAction(() =>
            {
                _customerBankInfoAppService.TransferMoneyToPortfolio(customerBankInfoId, portfolioId, amount);

                return Ok();
            });
        }
    }
}
