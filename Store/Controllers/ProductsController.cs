using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Extensions;
using Store.RequestHelpers;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace Store.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }


        [HttpGet]

        public async Task<ActionResult<PageList<Product>>> GetProductsAsync([FromQuery]ProductParams productParams)
        {
            // Obtiene una consulta IQueryable de la tabla Products
            var query = _context.Products
                .Sort(productParams.OrderBy) //LLammaos a nuestro metodo que extendimos
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();



            // Ejecuta la consulta de forma asíncrona y devuelve la lista de productos
            var products = await PageList<Product>.TopageList(query, productParams.PageNumber, productParams.PageSize);

            Response.Headers.Add("Pagination", JsonSerializer.Serialize(products.Metadata));

            return products;
        }


        [HttpGet("{id:int}")] // api/products/3 ej
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }


        [HttpGet("filters")]

        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(x => x.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(x => x.Type).Distinct().ToListAsync();

            return Ok(new
            {
                brands, types
            });
        }

    }


}
