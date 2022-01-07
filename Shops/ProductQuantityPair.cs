namespace Shops
{
    public class ProductQuantityPair
    {
        public ProductQuantityPair(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; }
        public int Quantity { get; }
    }
}