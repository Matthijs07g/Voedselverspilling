using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.Services;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        //GET all
        [HttpGet]
        public async Task<IActionResult> GetAllProducten()
        {
            IEnumerable<Product> Producten = await _productService.GetAllProductsAsync();

            if (Producten == null)
            {
                return BadRequest("No Products found");
            }
            else
            {
                return Ok(Producten);
            }
        }

        //GET one
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            Product Product = await _productService.GetProductByIdAsync(id);
            if (Product == null)
            {
                return BadRequest("Not Product found");
            }
            else
            {
                return Ok(Product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product Product)
        {
            if (Product == null)
            {
                return BadRequest("Product object is null.");
            }

            await _productService.AddProductAsync(Product);
            return CreatedAtAction(nameof(AddProduct), new { id = Product.Id }, Product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product object is null or ID mismatch.");
            }

            product.Id = id;

            await _productService.UpdateProductAsync(product);
            return Ok(product);
        }



        //DELETE
        [HttpDelete("{id}")]
        public async Task DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
        }
    }
}
