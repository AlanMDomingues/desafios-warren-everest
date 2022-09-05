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
    public class ProductsController : ControllersBase<ProductsController>
    {
        private readonly IProductAppService _productAppService;

        public ProductsController(
            IProductAppService productAppService,
            ILogger<ProductsController> logger)
            : base(logger)
            => _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));

        [HttpGet]
        public IActionResult GetAll()
        {
            return SafeAction(() =>
            {
                var result = _productAppService.GetAll();

                return !result.Any()
                    ? NoContent()
                    : Ok(result);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return SafeAction(() =>
            {
                var result = _productAppService.Get(id);

                return result is null
                    ? NotFound($"Product not found for Id: {id}")
                    : Ok(result);
            });
        }

        [HttpPost]
        public IActionResult Post(CreateProductRequest product)
        {
            return SafeAction(() =>
            {
                _productAppService.Add(product);

                return Created("~api/products", default);
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProductRequest product)
        {
            return SafeAction(() =>
            {
                _productAppService.Update(id, product);

                return Ok();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return SafeAction(() =>
            {
                _productAppService.Delete(id);

                return NoContent();
            });
        }
    }
}
