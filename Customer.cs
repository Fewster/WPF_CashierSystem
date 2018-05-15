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
    public class Customer : Person, IXmlSerializable
    {
        public Customer (int pID, string pFirstName, string pSurname, string pAddress, string pPNumber, string pEmail) : base(pID, pFirstName, pSurname, pAddress,pPNumber, pEmail)
        {

        }

        public int ID { get { return PersonID; } set { PersonID = value; } }
        public string firstName { get { return FirstName; } set { FirstName = value; } }
        public string surname { get { return Surname; } set { Surname = value; } }
        public string address { get { return Address; } set { Address = value; } }
        public string email { get { return Email; } set { Email = value; } }

        private Customer() { }
        public override void ReadXml(XmlReader reader)
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
                reader.ReadEndElement();
            }

        }
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ID", PersonID.ToString());
            writer.WriteElementString("FirstName", FirstName);
            writer.WriteElementString("Surname", Surname);
            writer.WriteElementString("Address", Address);
            writer.WriteElementString("PhoneNumber", Phone);
            writer.WriteElementString("Email", Email);
        }
    }
}
