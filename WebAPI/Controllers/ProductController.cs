using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Class;
using WebAPI.Models;
using Microsoft.AspNetCore.Http;


namespace WebAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]

    public class ProductController : Controller
    {

        private readonly ShopContext _context;

        public ProductController(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        //  [Route("*/Products")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameter queryParameter)
        {
            IQueryable<Product> products = _context.Products;



            if (!string.IsNullOrEmpty(queryParameter.searachItem))
            {
                products = products.Where(p => p.Sku.ToLower().Contains(queryParameter.searachItem.ToLower()) ||
                p.Name.ToLower().Contains(queryParameter.searachItem.ToLower()));
            }



            if (!string.IsNullOrEmpty(queryParameter.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameter.Name.ToLower()));
            }


            //if (!string.IsNullOrEmpty(queryParameter.sortBy))
            //{
            //    if (typeof(Product).GetProperty(queryParameter.sortBy) != null)
            //    {
            //        products = products.OrderByDescending(queryParameter.sortBy);
            //    }
            //}


            if (queryParameter.maxPrice != null || queryParameter.minPrice != null)
            {
                products = products.Where(p => p.Price <= queryParameter.maxPrice.Value &&
                p.Price >= queryParameter.minPrice.Value);
            }

            products = products.Skip(queryParameter.size * (queryParameter.page - 1))
                .Take(queryParameter.size);

            return Ok(await products.ToArrayAsync());
        }


        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }




        [HttpPut("{id:int}")]

        public async Task<IActionResult> PutProduct([FromQuery] int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(id) == null)
                    return BadRequest();
                throw;
            }
            return NoContent();
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<Product>> DeleteProduct([FromQuery] int id)
        {

            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            await  _context.SaveChangesAsync();
            return product;
        }

    }
}