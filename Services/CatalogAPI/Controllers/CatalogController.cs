using CatalogAPI.Models;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : Controller
    {
        private Serilog.ILogger _logger { get; set; }
        private readonly IProductRepository _productRepository;

        public CatalogController(Serilog.ILogger logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Problem("Could not retrieve products");
        }

        [HttpGet("{id:length(30)}",Name ="GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Problem("Could not retrieve product");
        }

        [Route("[action]/{category}", Name = "GetByCategory")]
        [HttpGet]
        public async Task<IActionResult> GetByCategory(string category)
        {
            try
            {
                var product = await _productRepository.GetProductByCategoryAsync(category);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Problem("Could not retrieve product");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                await _productRepository.CreateProductAsync(product);

                return Ok("Product successfully created");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Problem("Could not create product");
        }

        [HttpDelete("{id:length(30)}", Name = "Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var hasDeleted = await _productRepository.DeleteProductAsync(id);

                if (hasDeleted)
                    return Ok("Product successfully deleted");

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Problem("Could not delete product");
        }
    }
}
