using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Alan_WarrenDesafio1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderAppService _orderAppService;

        public OrdersController(IOrderAppService orderAppService)
            => _orderAppService = orderAppService ?? throw new ArgumentNullException(nameof(orderAppService));

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return SafeAction(() =>
            {
                return _orderAppService.Get(id) is null
                    ? NotFound()
                    : Ok(_orderAppService.Get(id));
            });
        }

        [HttpPost]
        public IActionResult Post(int portfolioId, int productId, int quotes)
        {
            return SafeAction(() =>
            {
                return !_orderAppService.Add(portfolioId, productId, quotes)
                    ? NotFound()
                    : Ok(_orderAppService.Add(portfolioId, productId, quotes));
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
