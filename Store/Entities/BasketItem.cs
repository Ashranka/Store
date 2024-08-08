using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Entities
{
    [Table("BasketItemss")]
    public class BasketItem
    {

        public int Id { get; set; }
        public int Quantity { get; set; }   

        //Navegation Properties o propiedas de navegacion
        //Sirven para que EF sepa que tenemos otras tablas

        public int ProductId { get; set; }
        public Product Product { get; set; }


        public int BaskedId { get; set; }
        public Basket Basket { get; set; }  
    }
}