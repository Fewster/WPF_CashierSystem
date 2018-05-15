using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace LOVEiT_BAKERY
{
    public class Item : IXmlSerializable
    {
        protected int mItemID;
        protected int mSupplierID;
        protected string mName;
        protected string mUnits;
        protected double mQuantity;
        protected decimal mCost;
        protected int mMin;

        public Item(int pItemID, int pSupplierID, string pName, string pUnits, double pQuantity, decimal pCost, int pMin)
        {
            mItemID = pItemID;
            mSupplierID = pSupplierID;
            mName = pName;
            mUnits = pUnits;
            mQuantity = pQuantity;
            mCost = pCost;
            mMin = pMin;
        }


        public int Min { get { return GetMin(); }set { mMin = value; } }
        public string Name { get { return GiveName(); } set { mName = value; } }
        public string ID { get { return GetID().ToString(); } set { mItemID = int.Parse(value);  } }
        public double Quantity { get { return GetQuantity(); } set { double.TryParse(value.ToString(), out mQuantity); } }
        public string Units { get { return GetUnits(); } set { mUnits = value; } }
        public decimal Cost { get { return GetCost(); } set { decimal.TryParse(value.ToString(), out mCost); } }
      
        public Item()
        {

        }

   
      

        public bool ChangeQuantity(string input)
        {
            double update;
            if(
            double.TryParse(input, out update))
                {
                mQuantity = update;
                return true;
            }else
            {
                return false;  
            }
        }
        public string GiveName()
        {
            return this.mName;
        }

        public int GetMin()
        {
            return mMin;
        }
        public int GetID()
        {
            return this.mItemID;
        }
        public string GetSupplierEmail(Database Database)
        {
            foreach (ExternalBusiness Supplier in Database.GetListofSuppliers())
            {
                if (mSupplierID == Supplier.getPersonID())
                {
                    return Supplier.getEmail();
                }
            }
            return "No Supplier Found in Database";
        }

        public string GetSupplierPhone(Database Database)
        {
            foreach (ExternalBusiness Supplier in Database.GetListofSuppliers())
            {
                if (mSupplierID == Supplier.getPersonID())
                {
                    return Supplier.getPhone();
                }
            }
            return "No Supplier Found in Database";
        }

        public string GetSupplierName(Database Database)
        {
            foreach (ExternalBusiness Supplier in Database.GetSuppliers())
            {
                if (mSupplierID == Supplier.getPersonID())
                {
                    return Supplier.GetBusinessName();
                }
            }
            return "No Supplier Found in Database";
        }

        public int GetSupplierID(Database Database)
        {
            foreach (ExternalBusiness Supplier in Database.GetSuppliers())
            {
                if (mSupplierID == Supplier.getPersonID())
                {
                    return Supplier.getPersonID();
                }
            }
            return 0;
        }
        public Double GetQuantity()
        {
            return mQuantity;
        }

        public void SubtractQuantity(int sutract)
        {
            mQuantity = mQuantity - sutract;
        }
        public decimal GetCost()
        {
            return mCost;
        }
        public string GetUnits()
        {
            return mUnits;
        }

        public override string ToString()
        {
            return mName;
          
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            mItemID = int.Parse(reader.GetAttribute("ID"));
            bool isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if(!isEmptyElement)
            {
                mName = reader.ReadElementContentAsString("Name", "");
                mSupplierID = reader.ReadElementContentAsInt("SupplierID", "");
                mQuantity = reader.ReadElementContentAsDouble("Quantity", "");
                mUnits = reader.ReadElementContentAsString("Units", "");
                mCost = reader.ReadElementContentAsDecimal("Cost", "");
                reader.ReadEndElement();
            }
        }
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ID", mItemID.ToString());
            writer.WriteElementString("Name", mName); ;
            writer.WriteElementString("SupplierID", mSupplierID.ToString());
            writer.WriteElementString("Quantity", mQuantity.ToString());
            writer.WriteElementString("Units", mUnits);
            writer.WriteElementString("Cost", mCost.ToString());
        }
    }
}