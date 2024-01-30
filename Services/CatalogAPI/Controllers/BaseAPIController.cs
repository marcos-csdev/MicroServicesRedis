using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    public abstract class BaseAPIController:Controller
    {
        protected Serilog.ILogger Logger { get; set; }

        protected BaseAPIController(Serilog.ILogger logger)
        {
            Logger = logger;
        }

    }
}
