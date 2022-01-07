namespace Shops
{
    public class ID
    {
        private static int _shopstart = 0;
        private static int _productstart = 0;

        public static int ShopIdGenerate()
        {
            return _shopstart++;
        }

        public static int ProductIdGenerate()
        {
            return _productstart++;
        }
    }
}