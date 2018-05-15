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
    public class CustomOrder : IXmlSerializable
    {
        int OrderID;
        int CustomerID;
        int BakerID;
        string OrderSize;
        bool OrderOutsourced;
        string OrderDescription;
        DateTime DateOrdered;
        decimal OrderPrice;
        bool IsPaid;
        bool ReadyToCollect;
        bool Collected;
        bool ActiveOrder;

        public CustomOrder(int pOrderID, int pCustomerID, string pOrderSize, bool pOrderOutsourced, string pOrderDescription, decimal pOrderPrice)
        {
            OrderID = pOrderID;
            CustomerID = pCustomerID;
            BakerID = -1;
            OrderSize = pOrderSize;
            OrderOutsourced = pOrderOutsourced;
            OrderDescription = pOrderDescription;
            OrderPrice = pOrderPrice;
            DateOrdered = DateTime.Now;
            IsPaid = false;
            ReadyToCollect = false;
            Collected = false;
            ActiveOrder = true;
        }
        public CustomOrder(bool Blank)
        {

        }
        public bool AssignBaker(Database DB)
        {
            bool bakerAssigned = false;

            if (OrderOutsourced)
            {
                for (int i = 0; i < DB.GetOrdersPerContractor(); i++)
                {
                    foreach (ExternalBusiness contractor in DB.GetListOfContractors())
                    {
                        if (i == contractor.getOrderIDsCount())
                        {
                            BakerID = contractor.getPersonID();
                            bakerAssigned = true;
                        }

                        if (bakerAssigned) { break; }
                    }

                    if (bakerAssigned) { break; }
                }
            }
            else
            {
                for (int i = 0; i < DB.GetOrdersPerBaker(); i++)
                {
                    foreach (Staff staff in DB.GetListOfStaffMembers())
                    {
                        if (staff.getIsBaker())
                        {
                            if (i == staff.getOrderIDsCount())
                            {
                                BakerID = staff.getPersonID();
                                bakerAssigned = true;
                            }
                        }

                        if (bakerAssigned) { break; }
                    }

                    if (bakerAssigned) { break; }
                }
            }

            return bakerAssigned;
        }

        public int ID { get { return OrderID; } set { OrderID = value; } }
        public string size { get { return OrderSize; } set { OrderSize = value; } }
        public string description { get { return OrderDescription; } set { OrderDescription = value; } }

        private CustomOrder()
        {
            if (!IsPaid)
            {
                if (DateOrdered.AddDays(3) > DateTime.Now)
                {
                    ActiveOrder = true;
                }
                else { ActiveOrder = false; }
            }
        }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.MoveToContent();
                OrderID = int.Parse(reader.GetAttribute("ID"));
                bool isEmptyElement = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (!isEmptyElement)
                {
                    CustomerID = reader.ReadElementContentAsInt("CustomerID", "");
                    BakerID = reader.ReadElementContentAsInt("BakerID", "");
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
            catch { }

        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ID", OrderID.ToString());
            writer.WriteElementString("CustomerID", CustomerID.ToString());
            writer.WriteElementString("BakerID", BakerID.ToString());
            writer.WriteElementString("OrderOutsourced", OrderOutsourced.ToString().ToLower());
            writer.WriteElementString("OrderSize", OrderSize);
            writer.WriteElementString("OrderDescription", OrderDescription);
            writer.WriteElementString("DateOrdered", DateOrdered.ToString());
            writer.WriteElementString("OrderPrice", OrderPrice.ToString());
            writer.WriteElementString("IsPaid", IsPaid.ToString().ToLower());
            writer.WriteElementString("ReadyToCollect", ReadyToCollect.ToString().ToLower());
            writer.WriteElementString("Collected", Collected.ToString().ToLower());
        }

        public int getOrderID()
        {
            return OrderID;
        }

        public void setOrderID(int pOrderID)
        {
            OrderID = pOrderID;
        }
        public int getCustomerID()
        {
            return CustomerID;
        }

        public void setCustomerID(int pCustomerID)
        {
            CustomerID = pCustomerID;
        }
        public int getBakerID()
        {
            return BakerID;
        }

        public void setBakerID(int pBakerID)
        {
            BakerID = pBakerID;
        }

        public string getOrderSize()
        {
            return OrderSize;
        }

        public void setOrderSize(string pOrderSize)
        {
            OrderSize = pOrderSize;
        }

        public string getDescription()
        {
            return OrderDescription;
        }

        public void setDescription(string pDescription)
        {
            OrderDescription = pDescription;
        }

        public bool getOrderOutsourced()
        {
            return OrderOutsourced;
        }

        public void setOrderOutsourced(bool pOrderOutsourced)
        {
            OrderOutsourced = pOrderOutsourced;
        }
        public decimal getOrderPrice()
        {
            return OrderPrice;
        }

        public void setOrderPrice(decimal pOrderPrice)
        {
            OrderPrice = pOrderPrice;
        }
        public bool getIsPaid()
        {
            return IsPaid;
        }

        public void setIsPaid(bool pIsPaid)
        {
            IsPaid = pIsPaid;
        }
        public string getOrderDescription()
        {
            return OrderDescription;
        }

        public void getOrderDescription(string pOrderDescription)
        {
            OrderDescription = pOrderDescription;
        }

        public bool getReadyToCollect()
        {
            return ReadyToCollect;
        }

        public bool getCollected()
        {
            return Collected;
        }

        public bool getActiveOrder()
        {
            return ActiveOrder;
        }

        public override string ToString()
        { 
            return "Order ID: " + OrderID + ", Price: £" + OrderPrice + ", Description: " + OrderDescription;
        }
    }
}
