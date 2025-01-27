using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController(IUnitOfWork uow) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
            return await CreatePagedResult(uow.Repository<Product>(), spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await uow.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            uow.Repository<Product>().Add(product);
            if (await uow.Complete())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Error creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !uow.Repository<Product>().Exists(id))
                return BadRequest("Cannot update this product");

            uow.Repository<Product>().Update(product);
            if (await uow.Complete())
            {
                return NoContent();
            }
            return BadRequest("Problem updating product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await uow.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound();
            uow.Repository<Product>().Remove(product);
            if (await uow.Complete())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting product");
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await uow.Repository<Product>().ListAsyncWithSpec(spec));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await uow.Repository<Product>().ListAsyncWithSpec(spec));
        }
    }
}