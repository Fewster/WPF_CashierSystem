using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LOVEiT_BAKERY
{
    public class Session : IXmlSerializable
    {
        private string userName;
        private string password;
        private int userID;
        private bool isAdmin;

        public Session(Staff staff)
        {
            userName = staff.getFirstName() + "." + staff.getSurname() + staff.getPersonID().ToString();
            password = "password";
            userID = staff.getPersonID();
            isAdmin = false;
        }

        public Session(bool admin)
        {
            userName = "admin";
            password = "admin";
            userID = -1;
            isAdmin = true;
        }

        private Session() { }

        public int getUserID()
        {
            return userID;
        }

        public string getUsername()
        {
            return userName;
        }

        public void setUsername(string newUsername)
        {
            userName = newUsername;
        }

        public string getPassword()
        {
            return password;
        }

        public void changePassword(string pPassword)
        {
            password = pPassword;
        }

        public void makeAdmin()
        {
            isAdmin = true;
        }

        public bool getIsAdmin()
        {
            return isAdmin;
        }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            userID = int.Parse(reader.GetAttribute("ID"));
            bool isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                userName = reader.ReadElementContentAsString("Username", "");
                password = reader.ReadElementContentAsString("Password", "");
                isAdmin = reader.ReadElementContentAsBoolean("IsAdmin", "");

                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ID", userID.ToString());
            writer.WriteElementString("Username", userName);
            writer.WriteElementString("Password", password);
            writer.WriteElementString("IsAdmin", isAdmin.ToString().ToLower());
        }
    }
}
