using FileImageService.BusinessLayer.Abstract;
using FileImageService.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileImageService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
       
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            var createdProduct = await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            var updatedProduct = await _productService.UpdateProductAsync(product);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 