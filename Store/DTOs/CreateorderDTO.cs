using Store.Entities.OrderAggregate;

namespace Store.DTOs
{
    public class CreateorderDTO
    { 
        public bool SaveAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

    }
}
