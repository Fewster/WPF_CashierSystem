using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOVEiT_BAKERY
{
   public class Checkout
    {
        private Dictionary<Product, int> Products;
        private List<CustomOrder> currentOrders;

        public Checkout()
        {
            Products = new Dictionary<Product, int>();
            currentOrders = new List<CustomOrder>();
        }

        public List<object> GetCollection()
        {
            List<Object> Collection = new List<object>();
            foreach(KeyValuePair<Product,int> KVP in Products)
            {
                Collection.Add(KVP);
            }
            foreach(CustomOrder CO in currentOrders)
            {
                Collection.Add(CO);
            }
            return Collection;
        }

        public void AddProduct(Product Product)
        {
            bool found = false;
            foreach (KeyValuePair<Product, int> Dict in Products)
            {

                if (Dict.Key == Product)
                {
                    Products[Dict.Key]++;
                    found = true;
                    break;
                }
            }
             if (Products.Count == 0 || found == false)
                {
                    Products.Add(Product, 1);
                }
        }

        public void RemoveProduct(KeyValuePair<Product,int> ProductInfo)
        {
            foreach (KeyValuePair<Product, int> KVP in Products)
            {
                if (KVP.Key == ProductInfo.Key)
                {
                    Products[KVP.Key]--;
                    if (Products[KVP.Key] <= 0)
                    {
                        Products.Remove(KVP.Key);
                    }
                }
            }
        }

        public void RemoveProductCompletely(KeyValuePair<Product, int> ProductInto)
        {
            Products.Remove(ProductInto.Key);
        }

        public decimal CalculateTotal(List<Promo> Promotions)
        {
            decimal total = CalculateTotalWithoutCustOrders(Promotions);

            foreach (CustomOrder customOrder in currentOrders)
            {
                total = total + customOrder.getOrderPrice();
            }

            return total;
        }

        public decimal CalculateTotalWithoutCustOrders(List<Promo> Promotions)
        {
            decimal total = 0;

            foreach (KeyValuePair<Product, int> KVP in Products)
            {
                decimal productPrice = KVP.Key.GetProdPrice();
                int productMultiplier = Products[KVP.Key];
                decimal productTotal = 0;

                if (Promotions.Count > 0)
                {
                    foreach (Promo promotion in Promotions)
                    {
                        if (promotion.getProduct() == KVP.Key)
                        {
                            if (promotion.GetType() == typeof(PromotionPercentile))
                            {
                                productPrice = ((PromotionPercentile)promotion).PromoLogicPercent();
                            }

                            else if (promotion.GetType() == typeof(PromotionBGF))
                            {
                                productMultiplier = productMultiplier / (((PromotionBGF)promotion).getBuy() + ((PromotionBGF)promotion).getGet_Free()) * ((PromotionBGF)promotion).getBuy() + (productMultiplier % (((PromotionBGF)promotion).getBuy() + ((PromotionBGF)promotion).getGet_Free()));
                            }

                            else if (promotion.GetType() == typeof(PromotionSave))
                            {
                                productPrice = ((PromotionSave)promotion).PromoLogicSave();
                            }
                        }
                    }
                }
                productTotal = productPrice * productMultiplier;
                total = total + productTotal;
            }

            return total;
        }

        public void AddOrder(CustomOrder CustomOrder)
        {
            currentOrders.Add(CustomOrder);
        }

        public void RemoveOrder(CustomOrder CustomOrder)
        {
            currentOrders.Remove(CustomOrder);
        }

        public Dictionary<Product, int> GetProducts()
        {
            return Products;
        }
        public List<CustomOrder> GetCustomOrders()
        {
            return currentOrders;
        }
    }
}
