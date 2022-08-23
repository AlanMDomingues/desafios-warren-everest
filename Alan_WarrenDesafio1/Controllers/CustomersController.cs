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
    public class CustomersController : ControllersBase<CustomersController>
    {
        private readonly ICustomerAppService _customersAppService;

        public CustomersController(
            ICustomerAppService customerAppService,
            ILogger<CustomersController> logger)
            : base(logger)
            => _customersAppService = customerAppService ?? throw new ArgumentNullException(nameof(customerAppService));

        [HttpGet]
        public IActionResult GetAll()
        {
            return SafeAction(() =>
            {
                var customers = _customersAppService.GetAll();

                return !customers.Any()
                    ? NotFound()
                    : Ok(customers);
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return SafeAction(() =>
            {
                var result = _customersAppService.Get(c => c.Id.Equals(id));

                return result is null
                    ? NotFound()
                    : Ok(result);
            });
        }

        [HttpGet("full-name")]
        public IActionResult GetByFullName(string fullName)
        {
            return SafeAction(() =>
            {
                var results = _customersAppService.GetAll(c => c.FullName.Contains(fullName));

                return !results.Any()
                    ? NotFound()
                    : Ok(results);
            });
        }

        [HttpGet("email")]
        public IActionResult GetByEmail(string email)
        {
            return SafeAction(() =>
            {
                var result = _customersAppService.Get(c => c.Email.Equals(email));

                return result is null
                    ? NotFound()
                    : Ok(result);
            });
        }

        [HttpGet("cpf")]
        public IActionResult GetByCpf(string cpf)
        {
            return SafeAction(() =>
            {
                var result = _customersAppService.Get(c => c.Cpf.Equals(cpf));

                return result is null
                    ? NotFound()
                    : Ok(result);
            });
        }

        [HttpPost]
        public IActionResult Post(CreateCustomerRequest newCustomerDto)
        {
            return SafeAction(() =>
            {
                var (status, messageResult) = _customersAppService.Add(newCustomerDto);

                return !status
                    ? BadRequest(messageResult)
                    : Created("~api/customers", messageResult);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateCustomerRequest customerToUpdateDto)
        {
            return SafeAction(() =>
            {
                var (status, messageResult) = _customersAppService.Update(id, customerToUpdateDto);

                return !status
                    ? BadRequest(messageResult)
                    : Ok(messageResult);
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return SafeAction(() =>
            {
                var (status, message) = _customersAppService.Delete(id);

                return !status
                    ? BadRequest(message)
                    : NoContent();
            });
        }
    }
}