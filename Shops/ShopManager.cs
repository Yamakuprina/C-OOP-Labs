using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shops
{
    public class ShopManager
    {
        private List<Product> registeredProducts = new List<Product>();

        public Shop FindShopWithLowestPriceAndNeededQuantity(List<Shop> shops, Product product, int quantityneeded)
        {
            float minPrice = -1;
            Shop minShop = null;
            foreach (Shop shop in shops)
            {
                if (minPrice.Equals(-1) && shop.FindProduct(product.Name) != null &&
                    shop.FindProduct(product.Name).Quantity >= quantityneeded)
                {
                    minPrice = shop.FindProduct(product.Name).Price;
                    minShop = shop;
                }

                if (shop.FindProduct(product.Name) != null && shop.FindProduct(product.Name).Price < minPrice &&
                    shop.FindProduct(product.Name).Quantity >= quantityneeded)
                {
                    minPrice = shop.FindProduct(product.Name).Price;
                    minShop = shop;
                }
            }

            return minShop;
        }

        public Product RegisterProduct(string name, int quantity, float recommededprice)
        {
            if (FindRegisteredProduct(name) != null)
            {
                Product product = FindRegisteredProduct(name);
                product.Quantity += quantity;
                product.Price = recommededprice;
                product.ID = ID.ProductIdGenerate();
                registeredProducts.Add(product);
                return product;
            }

            var newproduct = new Product(name, quantity, recommededprice);
            newproduct.ID = ID.ProductIdGenerate();
            registeredProducts.Add(newproduct);
            return newproduct;
        }

        public Shop CreateShop(float balance, string address, string name)
        {
            var shop = new Shop(balance, address, name);
            return shop;
        }

        private Product FindRegisteredProduct(string name)
        {
            Product product = registeredProducts.FirstOrDefault();
            return product;
        }
    }
}