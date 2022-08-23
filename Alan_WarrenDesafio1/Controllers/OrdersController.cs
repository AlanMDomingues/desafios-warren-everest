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
    public class OrdersController : ControllersBase<OrdersController>
    {
        private readonly IOrderAppService _orderAppService;

        public OrdersController(
            IOrderAppService orderAppService,
            ILogger<OrdersController> logger)
            : base(logger)
            => _orderAppService = orderAppService ?? throw new ArgumentNullException(nameof(orderAppService));

        [HttpGet("get-all/{id}")]
        public IActionResult GetAll(int id)
        {
            return SafeAction(() =>
            {
                var results = _orderAppService.GetAll(id);

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
                var result = _orderAppService.Get(id);

                return result is null
                    ? NotFound()
                    : Ok(result);
            });
        }

        [HttpPost]
        public IActionResult Post(int customerId, CreateOrderRequest orderRequest)
        {
            return SafeAction(() =>
            {
                var (status, message) = _orderAppService.Add(customerId, orderRequest);

                return !status
                    ? BadRequest(message)
                    : Created("~api/orders", default);
            });
        }
    }
}
