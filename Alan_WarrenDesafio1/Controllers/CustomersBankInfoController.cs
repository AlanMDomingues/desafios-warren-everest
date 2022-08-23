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
                    ? BadRequest()
                    : Ok(result);
            });
        }

        [HttpPut("money-deposit/{id}")]
        public IActionResult MoneyDeposit(int id, decimal cash)
        {
            return SafeAction(() =>
            {
                var (status, message) = _customerBankInfoAppService.MoneyDeposit(id, cash);

                return !status
                    ? NotFound(message)
                    : Ok();
            });
        }

        [HttpPut("withdraw-money/{id}")]
        public IActionResult WithdrawMoney(int id, decimal cash)
        {
            return SafeAction(() =>
            {
                var (status, message) = _customerBankInfoAppService.WithdrawMoney(id, cash);

                return !status
                    ? NotFound(message)
                    : Ok();
            });
        }
    }
}
