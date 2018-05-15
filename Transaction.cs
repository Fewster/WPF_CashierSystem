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
    public class Transaction : IXmlSerializable
    {
        private int StaffID;
        private DateTime dateTime;
        private Dictionary<Product, int> Products;
        private List<CustomOrder> Orders;
        private decimal total;
        private decimal amountPaid;
        private decimal change;

        public Transaction(int pStaffID, Dictionary<Product, int> pProducts, List<CustomOrder> pOrders, decimal pTotal, decimal pAmountPaid)
        {
            StaffID = pStaffID;
            dateTime = DateTime.Now;
            Products = pProducts;
            Orders = pOrders;
            total = pTotal;
            amountPaid = pAmountPaid;
            change = amountPaid - total;
        }

        private Transaction()
        { }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            /*
            try
            {
                reader.MoveToContent();
                bool isEmptyElement = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (!isEmptyElement)
                {
                    StaffID = reader.ReadElementContentAsInt("CustomerID", "");
                    dateTime = reader.ReadElementContentAsInt("BakerID", "");
                    OrderOutsourced = reader.ReadElementContentAsBoolean("OrderOutsourced", "");
                    OrderSize = reader.ReadElementContentAsString("OrderSize", "");
                    OrderDescription = reader.ReadElementContentAsString("OrderDescription", "");
                    DateOrdered = DateTime.Parse(reader.ReadElementContentAsString("DateOrdered", ""));
                    OrderPrice = reader.ReadElementContentAsDecimal("OrderPrice", "");
                    IsPaid = reader.ReadElementContentAsBoolean("IsPaid", "");
                    ReadyToCollect = reader.ReadElementContentAsBoolean("ReadyToCollect", "");
                    Collected = reader.ReadElementContentAsBoolean("Collected", "");
                    reader.ReadEndElement();
                }
            }
            catch { } */
        }

        public void WriteXml(XmlWriter writer)
        {
            
        }
    }
}
