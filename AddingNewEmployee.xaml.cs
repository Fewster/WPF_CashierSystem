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
    /// Interaction logic for AddingNewEmployee.xaml
    /// </summary>
    public partial class AddingNewEmployee : Window
    {
        private Database DB;
        private bool firstNameOK;
        private bool surnameOK;
        private bool addressOK;
        private bool phoneOK;
        private bool emailOK;
        public AddingNewEmployee(Database data)
        {
            DB = data;
            InitializeComponent();
            SaveButton.IsEnabled = false;

            firstNameOK = false;
            surnameOK = false;
            addressOK = false;
            phoneOK = false;
            emailOK = false;
        }

        public void ValidateData()
        {
            if (firstNameOK && surnameOK && addressOK && (phoneOK || emailOK)) { SaveButton.IsEnabled = true; WartingBlock.Text = ""; }
            else { SaveButton.IsEnabled = false; }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            int newID = 0;
            if (DB.GetListOfStaffMembers().Count != 0)
            {
                newID = DB.GetListOfStaffMembers()[DB.GetListOfStaffMembers().Count - 1].getPersonID() + 1;
            }
            bool isBaker = (bool)BakerBox.IsChecked;
            Staff staffMember = new Staff(newID, FirstNameTextBox.Text, SurnameTextBox.Text, AddressTextBox.Text, PhoneNumberTextBox.Text, EmailTextBox.Text, isBaker);
            DB.AddStaffMember(staffMember);
            UpdateLoginDatabase(staffMember);
            this.Close();
        }

        public void UpdateLoginDatabase(Staff newStaff)
        {
            LoginDatabase loginDB = new LoginDatabase();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoginDatabase));
            using (StreamReader reader = new StreamReader("loginDetails.xml"))
            {
                loginDB = (LoginDatabase)xmlSerializer.Deserialize(reader);
            }

            LoginDatabase newLoginDB = new LoginDatabase();

            if (loginDB.getExistingUsers().Count != 0 && loginDB.getExistingUsers()[0].getUsername() == "admin")
            {
                newLoginDB.AddUser(loginDB.getExistingUsers()[0]);
            }
            else
            {
                newLoginDB.createAdminAccount();
            }

            foreach (Staff staff in DB.GetListOfStaffMembers())
            {
                bool staffAdded = false;
                foreach (Session user in loginDB.getExistingUsers())
                {
                    if (staff.getPersonID() == user.getUserID() && !staffAdded)
                    {
                        string username = staff.getFirstName() + "." + staff.getSurname() + staff.getPersonID().ToString();
                        if (username != user.getUsername())
                        {
                            user.setUsername(username);
                            user.changePassword("password");
                        }
                        newLoginDB.AddUser(user);
                        staffAdded = true;
                    }
                }
            }

            Session newUser = new Session(newStaff);
            newLoginDB.AddUser(newUser);

            using (TextWriter writer = new StreamWriter("loginDetails.xml"))
            {
                xmlSerializer.Serialize(writer, newLoginDB);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstNameTextBox.Text != "") { firstNameOK = true; WartingBlock.Text = ""; }
            else { firstNameOK = false; WartingBlock.Text = "Input first name"; }
            ValidateData();
        }

        private void SurnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SurnameTextBox.Text != "") { surnameOK = true; WartingBlock.Text = ""; }
            else { surnameOK = false; WartingBlock.Text = "Input surname"; }
            ValidateData();
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddressTextBox.Text != "") { addressOK = true; WartingBlock.Text = ""; }
            else { addressOK = false; WartingBlock.Text = "Input address"; }
            ValidateData();
        }

        private void PhoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneNumberTextBox.Text == "" && EmailTextBox.Text == "")
            { phoneOK = false; emailOK = false; WartingBlock.Text = "Please input either phone number or email."; }
            else if (PhoneNumberTextBox.Text != "")
            {
                bool isNumeric = true;
                int lengthCount = 0;
                foreach (char c in PhoneNumberTextBox.Text)
                {
                    int isInt;
                    if (int.TryParse(c.ToString(), out isInt) == false)
                    { isNumeric = false; phoneOK = false; }
                    lengthCount++;
                }
                if (isNumeric == false)
                { WartingBlock.Text = "One or more characters used in the phone number are not a number. Please use only numbers."; }
                else if (lengthCount < 8)
                { phoneOK = false; WartingBlock.Text = "The phone number is too short."; }
                else if (lengthCount > 11)
                { phoneOK = false; WartingBlock.Text = "The phone number is too long."; }
                else { phoneOK = true; WartingBlock.Text = ""; }
            }

            ValidateData();
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            emailOK = true;
            if (PhoneNumberTextBox.Text == "" && EmailTextBox.Text == "")
            { emailOK = false; WartingBlock.Text = "Please input either phone number or email."; }
            else if (EmailTextBox.Text != "")
            {
                int atcount = 0;
                int charcount = 0;
                int totalcharcount = 0;
                foreach (char c in EmailTextBox.Text)
                {
                    if (c.ToString().Equals("@"))
                    {
                        if (totalcharcount == 0) { emailOK = false; WartingBlock.Text = "Invalid email address. An email cannot start with an '@'."; }
                        else if (totalcharcount == EmailTextBox.Text.Length - 1) { emailOK = false; WartingBlock.Text = "Invalid email address. An email cannot end on a '@'."; }
                        atcount++;
                    }

                    if (atcount == 1)
                    {
                        if (c.ToString().Equals("."))
                        {
                            if (charcount == 1) { emailOK = false; WartingBlock.Text = "Invalid email address. A '.' cannot be directly after '@'."; }
                            else if (totalcharcount == EmailTextBox.Text.Length - 1) { emailOK = false; WartingBlock.Text = "Invalid email address. An email cannot end on a '.'."; }
                        }
                        charcount++;
                    }

                    totalcharcount++;
                }

                if (atcount == 0 || atcount > 1)
                {
                    emailOK = false;
                    WartingBlock.Text = "Invalid email address. An email has to have one and only one '@' symbol.";
                }
            }

            ValidateData();
        }
    }
}
