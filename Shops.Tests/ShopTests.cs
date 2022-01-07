using System;
using System.Collections.Generic;
using Shops;
using NUnit.Framework;

namespace Shops.Tests
{
    public class ShopTests
    {
        private ShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        public void CreateShopMakeDeliveryAndBuy()
        {
            Shop shop = _shopManager.CreateShop(0, "Vyazemskiy 5/7", "Diksy");
            shop.AcceptDelivery(_shopManager.RegisterProduct("Doshirak", 10, 50));
            var customer = new Customer(550);
            shop.SellProduct(customer, "Doshirak", 5);
            shop.SellProduct(customer, "Doshirak", 5);
            Assert.Catch<ShopException>((() =>
            {
                shop.SellProduct(customer, "Doshirak", 1);
            }));
        }

        [Test]
        public void NotEnoughMoney()
        {
            Shop shop = _shopManager.CreateShop(0, "Vyazemskiy 5/7", "Diksy");
            shop.AcceptDelivery(_shopManager.RegisterProduct("Doshirak", 10, 600));
            var customer = new Customer(500);
            Assert.Catch<ShopException>((() =>
            {
                shop.SellProduct(customer, "Doshirak", 1);
            }));
        }

        [Test]
        public void NotProductFound()
        {
            Shop shop = _shopManager.CreateShop(0, "Vyazemskiy 5/7", "Diksy");
            shop.AcceptDelivery(_shopManager.RegisterProduct("Rolton", 10, 600));
            var customer = new Customer(500);
            Assert.IsNull(shop.FindProduct("Doshirak"));
            Assert.Catch<ShopException>((() =>
            {
                shop.SellProduct(customer, "Doshirak", 1);
            }));
        }

        [Test]
        public void BuyChangePriceAndBuy()
        {
            Shop shop = _shopManager.CreateShop(0, "Vyazemskiy 5/7", "Diksy");
            shop.AcceptDelivery(_shopManager.RegisterProduct("Doshirak", 10, 50));
            var customer = new Customer(550);
            shop.SellProduct(customer, "Doshirak", 1);
            shop.ChangePriceOfProduct("Doshirak",100);
            shop.SellProduct(customer, "Doshirak", 1);
            Assert.AreEqual(customer.Balance, 400);
        }

        [Test]
        public void FindShopLowPriceAndQuantity()
        {
            Shop shop1 = _shopManager.CreateShop(0, "Vyazemskiy 5/7", "Diksy");
            Product product = _shopManager.RegisterProduct("Doshirak", 1, 50);
            shop1.AcceptDelivery(product);
            Shop shop2 = _shopManager.CreateShop(0, "Kronverskiy 49", "Pyaterochka");
            shop2.AcceptDelivery(product);
            shop2.ChangePriceOfProduct(product.Name, 100);
            var shops = new List<Shop>() {shop1, shop2};
            Assert.AreEqual(shop1, _shopManager.FindShopWithLowestPriceAndNeededQuantity(shops, product, 1));
        }

        [Test]
        public void SellProductInPairsProductQuantity()
        {
            Shop shop = _shopManager.CreateShop(0, "Vyazemskiy 5/7", "Diksy");
            var customer = new Customer(1000);
            Product product1 = _shopManager.RegisterProduct("Doshirak", 10, 50);
            Product product2 = _shopManager.RegisterProduct("Mayonnaise", 10, 100);
            shop.AcceptDelivery(product1);
            shop.AcceptDelivery(product2);
            var pair1 = new ProductQuantityPair(product1, 5);
            var pair2 = new ProductQuantityPair(product2, 5);
            var productandquantity = new List<ProductQuantityPair>() {pair1,pair2};
            shop.SellProduct(customer, productandquantity);
            Assert.AreEqual(customer.Balance, 0);
        }
    }
}