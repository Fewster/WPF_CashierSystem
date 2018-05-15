using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LOVEiT_BAKERY
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private LoginDatabase loginDB;
        private Database DB;
        private Session currentSession;

        public Login(Database data)
        {
            DB = data;
            loginDB = new LoginDatabase();

            if (File.Exists("loginDetails.xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoginDatabase));
                using (StreamReader reader = new StreamReader("loginDetails.xml"))
                {
                    loginDB = (LoginDatabase)xmlSerializer.Deserialize(reader);
                }
                loginDB.createAdminAccount();
            }
            else { loginDB.createAdminAccount(); }

            InitializeComponent();
        }

        public void LoginSystem()
        {
            string inputUsername = textBoxUsername.Text;
            string inputPassword = passwordBoxPassword.Password;
            Session userToCheck = null;

            foreach (Session session in loginDB.getExistingUsers())
            {
                if (session.getUsername() == inputUsername)
                {
                    userToCheck = session;
                }
            }

            if (userToCheck == null) { MessageBox.Show("User does not exist"); }
            else if (userToCheck.getPassword() != inputPassword) { MessageBox.Show("Wrong password"); }
            else { currentSession = userToCheck; DB.SetCurrentSession(currentSession); Content = new MainMenu(DB); }
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginSystem();
        }

        private void buttonLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LoginSystem();
            }
        }
    }
}
