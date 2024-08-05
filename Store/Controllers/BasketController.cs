using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.DTOs;
using Store.Entities;

namespace Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;

        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            //La consulta la haremos a traves de la cookie que se almacenan en el ordenador
            //del usuario
            var basket = await RetrieveBasket();

            if (basket == null)
            {

                return NotFound();
            }

            return MapBasketToDto(basket);

        }

        

        [HttpPost]
        public async Task<ActionResult> AddItemToBasket(int productId, int quantity)
        {

            //Get Basket
            var basket = await RetrieveBasket();
            //create basket
            if (basket == null)
            {
                basket = CreateBasket();
            }

            //Get product
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            //add item 
            basket.AddItem(product, quantity);

            //save changes
            var result = await _context.SaveChangesAsync() > 0;
             if (result)
            {
                return CreatedAtRoute("GetBasket", MapBasketToDto(basket));
            }

             return BadRequest(new ProblemDetails { Title = "Problema agregando el producto al carrito"});
            
        }


        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem (int productId, int quantity)
        {
            //Get Baket
            var basket = await RetrieveBasket();
            if (basket == null) return NotFound();

            basket.RemoveItem(productId, quantity);
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Algo Salio Mal Eliminado el ELmento del Carrito"}); 

        }
           

        private async Task<Basket> RetrieveBasket()
        {
            return await _context.Baskets
                         .Include(i => i.Items)
                         .ThenInclude(p => p.Product)
                         .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
        }

        private Basket CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30)};
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            var basket = new Basket
            {
                BuyerId = buyerId
            };

            _context.Baskets.Add(basket);
            return basket;
        }


        private BasketDto MapBasketToDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity

                }).ToList()
            };
        }

    }
}
