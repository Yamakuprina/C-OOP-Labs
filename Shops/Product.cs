namespace Shops
{
    public class Product
    {
        public Product(string name, int quantity, float price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }

        public string Name { get; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int ID { get; set; }
    }
}