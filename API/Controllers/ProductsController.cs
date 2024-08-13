using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")] // The route will be api/products because Controller is removed
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext context; // private field should start with a "_"

        public ProductsController(StoreContext context)
        { // dependency injection of our DB context
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product==null) {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id)) 
                return BadRequest("Cannot update this product");

            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }        

        private bool ProductExists(int id)
        {
            return context.Products.Any(x => x.Id == id);
        }
    }

}