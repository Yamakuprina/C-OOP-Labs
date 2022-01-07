using System;
using System.Collections.Generic;

namespace Shops
{
    public class Shop
    {
        private List<Product> shopProducts = new List<Product>();

        public Shop(float balance, string address, string name)
        {
            ID = Shops.ID.ShopIdGenerate();
            Balance = balance;
            Address = address;
            Name = name;
        }

        public float Balance { get; set; }
        public string Address { get; }
        public int ID { get; }
        public string Name { get; }

        public Product FindProduct(string name)
        {
            foreach (Product product in shopProducts)
            {
                if (product.Name.Equals(name))
                {
                    return product;
                }
            }

            return null;
        }

        public void AcceptDelivery(Product product)
        {
            if (FindProduct(product.Name) == null)
            {
                shopProducts.Add(product);
            }
            else
            {
                FindProduct(product.Name).Quantity += product.Quantity;
            }
        }

        public void ChangePriceOfProduct(string name, float newprice)
        {
            if (FindProduct(name) != null)
            {
                FindProduct(name).Price = newprice;
            }
            else
            {
                throw new ShopException("No product found");
            }
        }

        public void SellProduct(Customer customer, string name, int quantity)
        {
            Product product = FindProduct(name);
            if (product != null && product.Quantity >= quantity &&
                customer.Balance >= (((float)quantity) * product.Price))
            {
                customer.Balance -= ((float)quantity) * product.Price;
                Balance += ((float)quantity) * product.Price;
                if (product.Quantity.Equals(quantity))
                {
                    product.Quantity = 0;
                }
                else
                {
                    product.Quantity -= quantity;
                }
            }
            else
            {
                throw new ShopException("Sell cant be done");
            }

            if (FindProduct(name).Quantity == 0)
            {
                shopProducts.Remove(FindProduct(name));
            }
        }

        public void SellProduct(Customer customer, List<ProductQuantityPair> productandquantity)
        {
            float productSum = 0;
            foreach (ProductQuantityPair pair in productandquantity)
            {
                productSum += pair.Product.Price * ((float)pair.Quantity);
            }

            if (customer.Balance < productSum)
            {
                throw new ShopException("Not enough money");
            }

            foreach (ProductQuantityPair pair in productandquantity)
            {
                SellProduct(customer, pair.Product.Name, pair.Quantity);
            }
        }
    }
}