using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Alan_WarrenDesafio1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductAppService _productAppService;

        public ProductsController(IProductAppService productAppService)
            => _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));

        [HttpGet]
        public IActionResult GetAll()
        {
            return SafeAction(() =>
            {
                return !_productAppService.GetAll().Any()
                    ? NotFound()
                    : Ok();
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return SafeAction(() =>
            {
                return _productAppService.Get(id) is null
                    ? NotFound()
                    : Ok(_productAppService.Get(id));
            });
        }

        [HttpPost]
        public IActionResult Post(CreateProductRequest product)
        {
            return SafeAction(() =>
            {
                var (status, message) = _productAppService.Add(product);
                return !status
                    ? BadRequest(message)
                    : Ok();
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProductRequest product)
        {
            return SafeAction(() =>
            {
                var (status, message) = _productAppService.Update(id, product);
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
                _productAppService.Delete(id);
                return NoContent();
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
