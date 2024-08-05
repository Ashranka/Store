using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.DTOs;
using Store.Entities;
using Store.Entities.OrderAggregate;
using Store.Extensions;

namespace Store.Controllers
{

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
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            return await _storeContext.Orders.
                Include(o => o.OrderItems).
                Where(x => x.BuyerId == User.Identity.Name)
                .ToListAsync();
                
        }

        [HttpGet("{id}", Name ="GetOrder")]

        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            return await _storeContext.Orders.
                Include(x => x.OrderItems)
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id ).
                FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateorderDTO createorderDTO)
        {
            // Obtener el carrito de compras del usuario autenticado
            var basket = await _storeContext.Baskets
                .RetrieveBasketWithItems(User.Identity.Name).FirstOrDefaultAsync();

            // Verificar si el carrito de compras existe
            if (basket == null) return BadRequest(new ProblemDetails
            {
                Title = "No se Puede Traer el Carrito"
            });

            // Crear una lista para almacenar los ítems de la orden
            var items = new List<OrderItem>();

            // Iterar sobre cada ítem en el carrito de compras
            foreach (var item in basket.Items)
            {
                // Buscar el producto correspondiente en la base de datos
                var productItem = await _storeContext.Products.FindAsync(item.ProductId);

                // Crear una representación del ítem del producto ordenado
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl,
                };

                // Crear un ítem de orden con los detalles del producto y la cantidad
                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                // Agregar el ítem de orden a la lista de ítems de la orden
                items.Add(orderItem);

                // Disminuir la cantidad en stock del producto
                productItem.QuantityInStock -= item.Quantity;
            }

            // Calcular el subtotal de la orden
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            // Definir la tarifa de envío basada en el subtotal
            var deliveryFree = subtotal > 10000 ? 0 : 500;

            // Crear la orden con los ítems y detalles correspondientes
            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = createorderDTO.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFree,
            };

            // Agregar la orden a la base de datos
            _storeContext.Orders.Add(order);
            // Eliminar el carrito de compras del usuario
            _storeContext.Baskets.Remove(basket);

            // Guardar la dirección de envío en el perfil del usuario si se solicita
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

            // Guardar todos los cambios en la base de datos
            var result = await _storeContext.SaveChangesAsync() > 0;

            // Retornar el resultado de la creación de la orden
            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
            return BadRequest("Problema Creando la orden");
        }


    }
}
