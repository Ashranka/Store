using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.DTOs;
using Store.Entities;
using Store.Extensions;
using Store.RequestHelpers;
using System.Text.Json;

namespace Store.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public ProductsController(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]

        public async Task<ActionResult<PageList<Product>>> GetProductsAsync([FromQuery] ProductParams productParams)
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


        [HttpGet("{id:int}", Name = "GetProduct")] // api/products/3 ej
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
                brands,
                types
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto productDto)
        {

            var product = _mapper.Map<Product>(productDto);

            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = "Problema creando un producto" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] UpdateproductDto UpdateproductDto)
        {
            //Primero debemos ver si existe el producto
            var product = await _context.Products.FindAsync(UpdateproductDto.Id);

            if (product == null) return NotFound();

            _mapper.Map(UpdateproductDto, product);

            var result = await _context.SaveChangesAsync() > 0;
            if (result) return NoContent();

            return BadRequest(new ProblemDetails { Title = "Problema Actualizando el producto" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "No se Pudo Actualizar el producto" });

        }





    }
}
