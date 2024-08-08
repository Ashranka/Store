namespace Store.Entities
{
    public class Basket
    {
        // Propiedad que actúa como clave primaria para la entidad Basket.
        public int Id { get; set; }

        // Propiedad que representa el identificador del comprador.
        public string BuyerId { get; set; }

        // Lista que contiene los elementos de la cesta.
        // Inicializada como una lista vacía.
        public List<BasketItem> Items { get; set; } = new();

        // Método para agregar un producto a la cesta.
        public void AddItem(Product product, int quantity)
        {
            // Verifica si el producto ya está en la cesta.
            if (Items.All(item => item.ProductId != product.Id))
            {
                // Si el producto no está en la cesta, se añade un nuevo BasketItem.
                Items.Add(new BasketItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }

            // Busca el producto en la lista de items.
            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);

            // Si el producto ya está en la lista, se incrementa la cantidad.
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
        }

        // Método para remover una cantidad específica de un producto de la cesta.
        public void RemoveItem(int productId, int quantity)
        {
            // Busca el producto en la lista de items.
            var item = Items.FirstOrDefault(item => item.ProductId == productId);

            // Si el producto no está en la lista, el método termina sin hacer nada.
            if (item == null)
            {
                return;
            }

            // Reduce la cantidad del producto en la cesta.
            item.Quantity -= quantity;

            // Si la cantidad llega a cero, se remueve el item de la lista.
            if (item.Quantity == 0)
            {
                Items.Remove(item);
            }
        }
    }
}
