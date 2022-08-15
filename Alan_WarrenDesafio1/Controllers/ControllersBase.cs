using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Alan_WarrenDesafio1.Controllers;

public abstract class ControllersBase<T> : Controller
    where T : class
{
    protected readonly ILogger _logger;

    public ControllersBase(ILogger<T> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected IActionResult SafeAction(Func<IActionResult> action)
    {
        try
        {
            return action?.Invoke();
        }
        catch (Exception ex)
        {
            var routeName = $"{Request.Method} - {Request.Path.Value}";
            if (Request.QueryString.HasValue)
            {
                routeName += Request.QueryString.Value.ToString();
            }

            if (ex.InnerException != null)
            {
                _logger.LogError(ex.InnerException, "An error occurred while calling {route}", routeName);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException);
            }

            _logger.LogError(ex, "An error occurred while calling {route}", routeName);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
