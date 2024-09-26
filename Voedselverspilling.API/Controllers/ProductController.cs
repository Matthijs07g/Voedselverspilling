using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        //GET all
        [HttpGet]
        public async Task<IActionResult> GetAllProducten()
        {
            IEnumerable<Product> Producten = await _productRepository.GetAllAsync();

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
            Product Product = await _productRepository.GetByIdAsync(id);
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

            await _productRepository.AddAsync(Product);
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

            await _productRepository.UpdateAsync(product);
            return Ok(product);
        }



        //DELETE
        [HttpDelete("{id}")]
        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
