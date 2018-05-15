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
    public class Person : IXmlSerializable
    {
        protected int PersonID;
        protected string FirstName;
        protected string Surname;
        protected string Address;
        protected string Phone;
        protected string Email;

        public Person(int pID, string pFirstName, string pSurname, string pAddress, string pPNumber, string pEmail)
        {
            PersonID = pID;
            FirstName = pFirstName;
            Surname = pSurname;
            Address = pAddress;
            Phone = pPNumber;
            Email = pEmail;

        }

        public Person() { }

        public int getPersonID()
        {
            return PersonID;
        }

        public void setPersonID(int pID)
        {
            PersonID = pID;
        }

        public string getFirstName()
        {
            return FirstName;
        }

        public void setFirstName(string pFirstName)
        {
            FirstName = pFirstName;
        }

        public string getSurname()
        {
            return Surname;
        }

        public void setSurname(string pSurname)
        {
            Surname = pSurname;
        }

        public string getAddress()
        {
            return Address;
        }

        public void setAddress(string pAddress)
        {
            Address = pAddress;
        }
       
        public string getPhone()
        {
            return Phone;
        }

        public void setPhone(string pPhone)
        {
            Phone = pPhone;
        }

        public string getEmail()
        {
            return Email;
        }

        public void setEmail(string pEmail)
        {
            Email = pEmail;
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public virtual void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
