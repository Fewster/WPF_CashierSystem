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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private Database DB;
        private bool oldPassOK;
        private bool newPassOK;
        private bool newPassConfirmOK;

        public ChangePassword(Database data)
        {
            InitializeComponent();
            DB = data;
        }

        public void CheckData()
        {
            if (DB.GetCurrentSession().getPassword() == CurrentPassTextBox.Password)
            {
                oldPassOK = true; WartingBlock.Text = "";
            }
            else { oldPassOK = false; WartingBlock.Text = "Wrong password"; }

            if (NewPassTextBox.Password == "") { newPassOK = false; WartingBlock.Text = "Input new password"; }
            else if (NewPassTextBox.Password != RepeatPassTextBox.Password) { newPassConfirmOK = false; WartingBlock.Text = "New password and its confirmation do not match"; }
            else if (NewPassTextBox.Password != "" && NewPassTextBox.Password == RepeatPassTextBox.Password) { newPassOK = true; newPassConfirmOK = true; WartingBlock.Text = ""; }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CheckData();
            if (oldPassOK && newPassOK && newPassConfirmOK)
            {
                DB.GetCurrentSession().changePassword(NewPassTextBox.Password);
                LoginDatabase loginDB = new LoginDatabase();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoginDatabase));
                using (StreamReader reader = new StreamReader("loginDetails.xml"))
                {
                    loginDB = (LoginDatabase)xmlSerializer.Deserialize(reader);
                }

                foreach (Session user in loginDB.getExistingUsers())
                {
                    if (DB.GetCurrentSession().getUserID() == user.getUserID())
                    { user.changePassword(NewPassTextBox.Password); }
                }

                using (TextWriter writer = new StreamWriter("loginDetails.xml"))
                {
                    xmlSerializer.Serialize(writer, loginDB);
                }

                DialogResult = true;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
