using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Controllers;
using Store.Data;
using Store.DTOs;
using Store.Entities.OrderAggregate;
using Store.Entities;
using Microsoft.EntityFrameworkCore;
using Store.Extensions;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderController : BaseApiController
{
    private readonly StoreContext _storeContext;

    public OrderController(StoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        var buyerId = GetBuyerId();
        var orders = await _storeContext.Orders.ProjectOrderToOrderDto()
            .Where(x => x.BuyerId == buyerId)
            .ToListAsync();

        if (orders == null)
        {
            return NotFound("No se encontraron órdenes para este usuario.");
        }

        return orders;
    }

    [HttpGet("{id}", Name = "GetOrder")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var buyerId = GetBuyerId();
        var order = await _storeContext.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.BuyerId == buyerId && x.Id == id)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound("No se encontró la orden.");
        }

        return order;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateorderDTO createorderDTO)
    {
        var buyerId = GetBuyerId();
        var basket = await _storeContext.Baskets
            .RetrieveBasketWithItems(buyerId)
            .FirstOrDefaultAsync();

        if (basket == null)
        {
            return BadRequest(new ProblemDetails { Title = "No se puede traer el carrito" });
        }

        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _storeContext.Products.FindAsync(item.ProductId);

            var itemOrdered = new ProductItemOrdered
            {
                ProductId = productItem.Id,
                Name = productItem.Name,
                PictureUrl = productItem.PictureUrl,
            };

            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };

            items.Add(orderItem);
            productItem.QuantityInStock -= item.Quantity;
        }

        var subtotal = items.Sum(item => item.Price * item.Quantity);
        var deliveryFee = subtotal > 10000 ? 0 : 500;

        var order = new Order
        {
            OrderItems = items,
            BuyerId = buyerId,
            ShippingAddress = createorderDTO.ShippingAddress,
            Subtotal = subtotal,
            DeliveryFee = deliveryFee,
        };

        _storeContext.Orders.Add(order);
        _storeContext.Baskets.Remove(basket);

        if (createorderDTO.SaveAddress)
        {
            var user = await _storeContext.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            user.Address = new UserAddress
            {
                FullName = createorderDTO.ShippingAddress.FullName,
                Address1 = createorderDTO.ShippingAddress.Address1,
                Address2 = createorderDTO.ShippingAddress.Address2,
                City = createorderDTO.ShippingAddress.City,
                Region = createorderDTO.ShippingAddress.Region,
                PostalCode = createorderDTO.ShippingAddress.PostalCode,
                Country = createorderDTO.ShippingAddress.Country,
            };
            _storeContext.Update(user);
        }

        var result = await _storeContext.SaveChangesAsync() > 0;

        if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
        return BadRequest("Problema creando la orden");
    }

    //private string GetBuyerId()
    //{
    //    var buyerIdClaim = User.FindFirst("buyerId")?.Value;
    //    if (buyerIdClaim == null || !Guid.TryParse(buyerIdClaim, out Guid buyerId))
    //    {
    //        throw new Exception("Invalid BuyerId format");
    //    }
    //    return buyerId.ToString();
    //}

    private string GetBuyerId()
    {
        return Request.Cookies["buyerId"];
    }
}

