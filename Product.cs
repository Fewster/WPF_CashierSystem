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
    public class Product : Item
    {
        protected decimal mPrice;
        string mType;

        public Product(int pItemID, int pSupplierID, string pName, string pUnits, double pQuantity, decimal pCost, decimal pPrice, string pType, int pMin) : base(pItemID, pSupplierID, pName, pUnits, pQuantity, pCost, pMin)
        {
            mPrice = pPrice;
            mType = pType;
        }

        public string ProdAndPrice { get { return GetProdAndPrice(); } set { } }
        public string Type { get { return GetItemType(); } set { mType = value; } }
        public decimal Price { get { return GetProdPrice(); } set { decimal.TryParse(value.ToString(), out mPrice); } }
        public Product()
        {

        }

        public string GetProdAndPrice()
        {
            return mName + "-(£" + mPrice + ")";
        }
        public override string ToString()
        {

            // I need this to not include the price for my code to work, sorry, i hope we can find a solution that helps us both in the future though.

            // return mName + " " + mPrice.ToString();
            return mName;
        }

        public string GetProdType()
        {
            return mType;
        }

        public string GetProdName()
        {
            return mName;
        }

        public decimal GetProdPrice()
        {
            return mPrice;
        }

        public string GetItemType()
        {
            return mType;
        }
        public void SetProdPrice(decimal setPrice)
        {
            mPrice = setPrice;
        }
        public Item ToItem(Database DB)
        {
            Item Item = new Item(mItemID, mSupplierID, mName, mUnits, mQuantity, mCost, mMin);
            return Item;
        }
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            mItemID = int.Parse(reader.GetAttribute("ID"));
            bool isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                mName = reader.ReadElementContentAsString("Name", "");
                mSupplierID = reader.ReadElementContentAsInt("SupplierID", "");
                mQuantity = reader.ReadElementContentAsDouble("Quantity", "");
                mUnits = reader.ReadElementContentAsString("Units", "");
                mCost = reader.ReadElementContentAsDecimal("Cost", "");
                mType = reader.ReadElementContentAsString("Type", "");
                mPrice = reader.ReadElementContentAsDecimal("Price", "");
                reader.ReadEndElement();
            }
        }
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ID", mItemID.ToString());
            writer.WriteElementString("Name", mName); 
            writer.WriteElementString("SupplierID", mSupplierID.ToString());
            writer.WriteElementString("Quantity", mQuantity.ToString());
            writer.WriteElementString("Units", mUnits);
            writer.WriteElementString("Cost", mCost.ToString());
            writer.WriteElementString("Type", mType);
            writer.WriteElementString("Price", mPrice.ToString());
        }


    }
}
