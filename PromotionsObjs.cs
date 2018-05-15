using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace LOVEiT_BAKERY
{
    public class Promo
    {
        public string Name;
        protected Product product;
        protected int productID;

        public string name { get { return Name; } set { Name = value; } }
        public string Product { get { return product.GetProdName(); } set {  } }

        public Promo(string pName, Product pproduct)
        {
            Name = pName;
            product = pproduct;
        }

        public Promo()
        { }

        public int getProductID()
        {
            return productID;
        }

        public string getName()
        {
            return Name;
        }

        public void setName(string nameSet)
        {
            Name = nameSet;
        }

        public Product getProduct()
        {
            return product;
        }

        public void setProduct(Product productSet)
        {
            product = productSet;
        }
    }

    //--------------------PERCENTILE------------------------------------//

    public class PromotionPercentile : Promo, IXmlSerializable
    {
        public int Percentage;

        public string Type { get { return "Percentage OFF"; } set { } }

        public PromotionPercentile(string pName, int pPercentage, Product pproduct) : base(pName, pproduct)
        {
            Percentage = pPercentage;
        }

        private PromotionPercentile()
        {
            product = null;
        }

        public int getPercentage()
        {
            return Percentage;
        }

        public void setPercentage(int percentSet)
        {
            Percentage = percentSet;
        }

        public decimal PromoLogicPercent()
        {
            decimal price = product.GetProdPrice();
            decimal percentAmountOff = Percentage * price / 100m;
            price = Math.Round(price - percentAmountOff);

            /*decimal percentAmountOff = price - Percentage / 100 * price;
            percentAmountOff = Math.Round(percentAmountOff, 2);

            price = percentAmountOff;*/
            return price;
        }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.MoveToContent();
                bool isEmptyElement = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (!isEmptyElement)
                {
                    Name = reader.ReadElementContentAsString("PromoName", "");
                    Percentage = reader.ReadElementContentAsInt("PercentageOFF", "");
                    productID = reader.ReadElementContentAsInt("ProductID", "");
                    reader.ReadEndElement();
                }
            }
            catch { }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("PromoName", Name);
            writer.WriteElementString("PercentageOFF", Percentage.ToString());
            writer.WriteElementString("ProductID", product.GetID().ToString());
        }
    }

    //----------------------------------BUY#GET#FREE-----------------------------------//

    public class PromotionBGF : Promo, IXmlSerializable
    {
        public int Buy;
        public int Get_Free;

        public string Type { get { return "Buy # Get # Free"; } set { } }

        public PromotionBGF(string pName, int pBuy, int pGet_Free, Product pproduct) : base(pName, pproduct)
        {
            Buy = pBuy;
            Get_Free = pGet_Free;
        }

        private PromotionBGF()
        {
            product = null;
        }

        public int getBuy()
        {
            return Buy;
        }

        public void setBuy(int BuySet)
        {
            Buy = BuySet;
        }

        public int getGet_Free()
        {
            return Get_Free;
        }

        public void setGet_Free(int Get_FreeSet)
        {
            Get_Free = Get_FreeSet;
        }

        public void PromoLogicBGF(int buy, int get)
        {

        }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.MoveToContent();
                bool isEmptyElement = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (!isEmptyElement)
                {
                    Name = reader.ReadElementContentAsString("PromoName", "");
                    Buy = reader.ReadElementContentAsInt("Buy", "");
                    Get_Free = reader.ReadElementContentAsInt("GetFree", "");
                    productID = reader.ReadElementContentAsInt("ProductID", "");
                    reader.ReadEndElement();
                }
            }
            catch { }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("PromoName", Name);
            writer.WriteElementString("Buy", Buy.ToString());
            writer.WriteElementString("GetFree", Get_Free.ToString());
            writer.WriteElementString("ProductID", product.GetID().ToString());
        }
    }

    //-------------------------------SAVE_£££------------------------------------//

    public class PromotionSave : Promo, IXmlSerializable
    {
        public decimal Save;

        public string Type { get { return "Save £#"; } set { } }

        public PromotionSave(string pName, decimal pSave, Product pproduct) : base(pName, pproduct)
        {
            Save = pSave;
        }

        private PromotionSave()
        {
            product = null;
        }

        public decimal getSave()
        {
            return Save;
        }

        public void setSave(decimal SaveSet)
        {
            Save = SaveSet;
        }

        public decimal PromoLogicSave()
        {
            decimal price = product.GetProdPrice();
            decimal newPrice = price - Save;

            return newPrice;
        }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.MoveToContent();
                bool isEmptyElement = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (!isEmptyElement)
                {
                    Name = reader.ReadElementContentAsString("PromoName", "");
                    Save = reader.ReadElementContentAsDecimal("Save", "");
                    productID = reader.ReadElementContentAsInt("ProductID", "");
                    reader.ReadEndElement();
                }
            }
            catch { }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("PromoName", Name);
            writer.WriteElementString("Save", Save.ToString());
            writer.WriteElementString("ProductID", product.GetID().ToString());
        }
    }

    //-------------------------------------------------------------------//
}
