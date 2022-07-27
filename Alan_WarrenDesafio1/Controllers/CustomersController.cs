using Application;
using Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Alan_WarrenDesafio1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerAppService _customersAppService;

        public CustomersController(ICustomerAppService customerAppService)
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

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return SafeAction(() =>
            {
                return _customersAppService.GetBy(c => c.Id == id) is null
                    ? NotFound()
                    : Ok(_customersAppService.GetBy(c => c.Id == id));
            });
        }

        [HttpGet("full-name")]
        public IActionResult GetByFullName(string fullName)
        {
            return SafeAction(() =>
            {
                return !_customersAppService.GetAll(c => c.FullName.Contains(fullName)).Any()
                    ? NotFound()
                    : Ok(_customersAppService.GetAll(c => c.FullName.Contains(fullName)));
            });
        }

        [HttpGet("email")]
        public IActionResult GetByEmail(string email)
        {
            return SafeAction(() =>
            {
                return _customersAppService.GetBy(c => c.Email == email) is null
                    ? NotFound()
                    : Ok(_customersAppService.GetBy(c => c.Email == email));
            });
        }

        [HttpGet("cpf")]
        public IActionResult GetByCpf(string cpf)
        {
            return SafeAction(() =>
            {
                return _customersAppService.GetBy(c => c.Cpf == cpf) is null
                    ? NotFound()
                    : Ok(_customersAppService.GetBy(c => c.Cpf == cpf));
            });
        }

        [HttpPost]
        public IActionResult Post(CreateCustomerRequest newCustomerDto)
        {
            return SafeAction(() =>
            {
                var (status, messageResult) = _customersAppService.Create(newCustomerDto);

                return !status
                    ? BadRequest(messageResult)
                    : Created("~api/customer", messageResult);
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
                return !_customersAppService.Delete(id)
                    ? NotFound()
                    : NoContent();
            });
        }

        private IActionResult SafeAction(Func<IActionResult> action)
        {
            try
            {
                return action?.Invoke();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}