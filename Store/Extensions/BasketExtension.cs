using Microsoft.EntityFrameworkCore;
using Store.DTOs;
using Store.Entities;

namespace Store.Extensions
{
    public static class BasketExtension
    {
        public static BasketDto MapBasketToDto(this Basket basket)
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
                }).ToList(),
            };
        }
        public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query, string buyerId)
        {
            // Agregar log para verificar el buyerId
            Console.WriteLine($"Retrieving basket for BuyerId: {buyerId}");

            var result = query
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .Where(b => b.BuyerId == buyerId);

            // Agregar log para verificar el número de resultados
            Console.WriteLine($"Number of baskets found: {result.Count()}");

            return result;
        }


    }

}
