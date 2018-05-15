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
    public class Staff : Person
    {
        private bool isBaker;
        private List<int> OrderIDs;
        public Staff(int pID, string pFirstName, string pSurname, string pAddress, string pPNumber, string pEmail, bool pIsBaker) : base(pID, pFirstName, pSurname, pAddress, pPNumber, pEmail)
        {
            isBaker = pIsBaker;
            if (isBaker == true)
            {
                OrderIDs = new List<int>();
            }
        }

        private Staff()
        {
            OrderIDs = new List<int>();
        }

        public int staffID { get { return PersonID; } set { PersonID = value; } }
        public string firstName { get { return FirstName; } set { FirstName = value; } }
        public string surname { get { return Surname; } set { Surname = value; } }
        public string email { get { return Email; } set { Email = value; } }

        public bool getIsBaker()
        {
            return isBaker;
        }

        public void setIsBaker(bool pIsBaker)
        {
            isBaker = pIsBaker;
            if (isBaker) { OrderIDs = new List<int>(); }
            else { OrderIDs = null; }
        }

        public List<int> getOrderIDs()
        {
            return OrderIDs;
        }

        public int getOrderIDsCount()
        {
            if (OrderIDs == null)
            {
                return 0;
            }
            else { return OrderIDs.Count; }
        }

        public void removeOrderID(int orderID)
        {
            OrderIDs.Remove(orderID);
        }

        public void addOrderID(int orderID)
        {
            OrderIDs.Add(orderID);
        }


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
                    FirstName = reader.ReadElementContentAsString("FirstName", "");
                    Surname = reader.ReadElementContentAsString("Surname", "");
                    Address = reader.ReadElementContentAsString("Address", "");
                    Phone = reader.ReadElementContentAsString("PhoneNumber", "");
                    Email = reader.ReadElementContentAsString("Email", "");
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
            writer.WriteElementString("Surname", Surname);
            writer.WriteElementString("Address", Address);
            writer.WriteElementString("PhoneNumber", Phone);
            writer.WriteElementString("Email", Email);
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
