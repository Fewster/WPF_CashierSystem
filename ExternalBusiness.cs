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
    public class ExternalBusiness : Person, IXmlSerializable
    {
        string mBusinessName;
        private bool isBaker;
        private List<int> OrderIDs;
        public ExternalBusiness(int pID, string pFirstName, string pSurname, string pAddress, string pPNumber, string pEmail, string pBusinessName, bool pIsBaker) : base(pID, pFirstName, pSurname, pAddress, pPNumber, pEmail)
        {
            mBusinessName = pBusinessName;
            isBaker = pIsBaker;
            if (isBaker)
            {
                OrderIDs = new List<int>();
            }
        }

        public ExternalBusiness()
        { }
        public int ID { get { return getPersonID(); } set {}}
        public string Name { get { return mBusinessName; } set { mBusinessName = value; } }
        public string extFirstName { get { return FirstName; } set { FirstName = value; } }
        public string extLastName { get { return Surname; } set { Surname = value; } }
        public string extEmail { get { return Email; } set { Email = value; } }

        //ignore this, need it for a combo box
        public int staffID { get { return getPersonID(); } set { } }
        public string firstName { get { return mBusinessName; } set { } }
        public string surname { get { return FirstName + " " + Surname; } set { } }

        public string GetBusinessName()
        {
            return mBusinessName;
        }

        public void SetBusinessName(string pBusinessName)
        {
            mBusinessName = pBusinessName;
        }

        public override string ToString()
        {
            return mBusinessName;
        }

        public int getOrderIDsCount()
        {
            if (OrderIDs == null)
            {
                return 0;
            }
            else { return OrderIDs.Count; }
        }

        public List<int> getOrderIDs()
        {
            return OrderIDs;
        }

        public void addOrderID(int orderID)
        {
            OrderIDs.Add(orderID);
        }

        public void removeOrderID(int orderID)
        {
            OrderIDs.Remove(orderID);
        }

        public XmlSchema GetSchema() { return null; }

        public override void ReadXml(XmlReader reader)
        {
            try
            {
                reader.MoveToContent();
                PersonID = int.Parse(reader.GetAttribute("ID"));
                bool isEmptyElement = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (!isEmptyElement)
                {
                    firstName = reader.ReadElementContentAsString("FirstName", "");
                    surname = reader.ReadElementContentAsString("SurName", "");
                    Address = reader.ReadElementContentAsString("Address", "");
                    Phone = reader.ReadElementContentAsString("Phone", "");
                    Email = reader.ReadElementContentAsString("Email", "");
                    mBusinessName = reader.ReadElementContentAsString("BusinessName", "");
                    isBaker = reader.ReadElementContentAsBoolean("IsBaker", "");
                    if (isBaker == true)
                    {
                        reader.ReadStartElement();
                        if (reader.Name == "OrderID")
                        {
                            while (reader.Name == "OrderID")
                            {
                                OrderIDs.Add(int.Parse(reader.GetAttribute("ID")));
                                Console.WriteLine(int.Parse(reader.GetAttribute("ID")));
                                reader.Read();
                            }
                            if (OrderIDs.Count > 0)
                            {
                                reader.ReadEndElement();
                            }
                        }
                    }
                    reader.ReadEndElement();
                }
            }
            catch { }
        }
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ID", PersonID.ToString());
            writer.WriteElementString("FirstName", FirstName);
            writer.WriteElementString("SurName", Surname);
            writer.WriteElementString("Phone", Phone);
            writer.WriteElementString("Email", Email);
            writer.WriteElementString("BusinessName", mBusinessName);
            writer.WriteElementString("IsBaker", isBaker.ToString().ToLower());
            if (isBaker == true)
            {
                writer.WriteStartElement("OrderIDs");
                foreach (int id in OrderIDs)
                {
                    writer.WriteStartElement("OrderID");
                    writer.WriteAttributeString("ID", id.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
    }
}
