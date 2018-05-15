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
    public class LoginDatabase : IXmlSerializable
    {
        private List<Session> ListOfUsers;

        public LoginDatabase()
        {
            ListOfUsers = new List<Session>();
        }

        public void createAdminAccount()
        {
            bool adminExists = false;
            foreach (Session session in ListOfUsers)
            {
                if (session.getUsername() == "admin") { adminExists = true; }
            }

            if (adminExists == false)
            {
                Session admin = new Session(true);
                ListOfUsers.Add(admin);
            }
        }

        public List<Session> getExistingUsers()
        {
            return ListOfUsers;
        }

        public void AddUser(Session user)
        {
            ListOfUsers.Add(user);
        }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            bool isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                var accountSerializer = new XmlSerializer(typeof(Session));
                reader.ReadToFollowing("Session");
                while (!reader.EOF && reader.Name == "Session")
                {
                    Session session = (Session)accountSerializer.Deserialize(reader);
                    ListOfUsers.Add(session);
                }
            }
            reader.Close();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Users");
            foreach (Session session in ListOfUsers)
            {
                var accountSerializer = new XmlSerializer(typeof(Session));
                accountSerializer.Serialize(writer, session);
            }
            writer.WriteEndElement();
        }
    }
}
