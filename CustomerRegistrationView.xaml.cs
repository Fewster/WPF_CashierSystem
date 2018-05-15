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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LOVEiT_BAKERY
{
    /// <summary>
    /// Interaction logic for CustomerRegistrationView.xaml
    /// </summary>
    public partial class CustomerRegistrationView : UserControl
    {
        private Database DB;
        private bool firstNameOK;
        private bool surnameOK;
        private bool addressOK;
        private bool phoneOK;
        private bool emailOK;

        public CustomerRegistrationView(Database data)
        {
            DB = data;
            InitializeComponent();

            RegistrationButton.IsEnabled = false;

            firstNameOK = false;
            surnameOK = false;
            addressOK = false;
            phoneOK = false;
            emailOK = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new GeneralCustomOrdersView(DB);
        }

        public void ValidateData()
        {
            if (firstNameOK && surnameOK && addressOK && (phoneOK || emailOK))
            {
                WarningBlock.Text = "";
                RegistrationButton.IsEnabled = true;
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            int newID = 0;
            if (DB.GetListOfCustomers().Count > 0)
            {
                newID = DB.GetListOfCustomers()[DB.GetListOfCustomers().Count - 1].getPersonID() + 1;
            }
            Customer NewCustomer = new Customer(newID, FirstNameTextBox.Text, LastNameTextBox.Text, AddressTextBox.Text, PhoneTextBox.Text, EmailTextBox.Text);
            DB.AddCustomer(NewCustomer);
            MessageBox.Show("New customer has been added successfully with ID of " + newID.ToString());
            Content = new NewCustomOrderView(DB);
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstNameTextBox.Text != "") { firstNameOK = true; WarningBlock.Text = ""; }
            else { firstNameOK = false; WarningBlock.Text = "Input first name"; }
            ValidateData();
        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LastNameTextBox.Text != "") { surnameOK = true; WarningBlock.Text = ""; }
            else { surnameOK = false; WarningBlock.Text = "Input surname"; }
            ValidateData();
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddressTextBox.Text != "") { addressOK = true; WarningBlock.Text = ""; }
            else { addressOK = false; WarningBlock.Text = "Input address"; }
            ValidateData();
        }

        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneTextBox.Text == "" && EmailTextBox.Text == "")
            { phoneOK = false; emailOK = false; WarningBlock.Text = "Please input either phone number or email."; }

            if (PhoneTextBox.Text != "")
            {
                bool isNumeric = true;
                int lengthCount = 0;
                foreach (char c in PhoneTextBox.Text)
                {
                    int isInt;
                    if (int.TryParse(c.ToString(), out isInt) == false)
                    { isNumeric = false; phoneOK = false; }
                    lengthCount++;
                }
                if (isNumeric == false)
                { WarningBlock.Text = "One or more characters used in the phone number are not a number. Please use only numbers."; }
                else if (lengthCount < 8)
                { phoneOK = false; WarningBlock.Text = "The phone number is too short."; }
                else if (lengthCount > 11)
                { phoneOK = false; WarningBlock.Text = "The phone number is too long."; }
                else { phoneOK = true; WarningBlock.Text = ""; }
            }

            ValidateData();
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            emailOK = true;
            if (PhoneTextBox.Text == "" && EmailTextBox.Text == "")
            { emailOK = false; phoneOK = false; WarningBlock.Text = "Please input either phone number or email."; }

            if (EmailTextBox.Text != "")
            {
                RegistrationButton.IsEnabled = false;
                int atcount = 0;
                int charcount = 0;
                int totalcharcount = 0;
                foreach (char c in EmailTextBox.Text)
                {
                    if (c.ToString().Equals("@"))
                    {
                        if (totalcharcount == 0) { emailOK = false; WarningBlock.Text = "Invalid email address. An email cannot start with an '@'."; }
                        else if (totalcharcount == EmailTextBox.Text.Length - 1) { emailOK = false; WarningBlock.Text = "Invalid email address. An email cannot end on a '@'."; }
                        atcount++;
                    }

                    if (atcount == 1)
                    {
                        if (c.ToString().Equals("."))
                        {
                            if (charcount == 1) { emailOK = false; WarningBlock.Text = "Invalid email address. A '.' cannot be directly after '@'."; }
                            else if (totalcharcount == EmailTextBox.Text.Length - 1) { emailOK = false; WarningBlock.Text = "Invalid email address. An email cannot end on a '.'."; }
                        }
                        charcount++;
                    }

                    totalcharcount++;
                }

                if (atcount == 0 || atcount > 1)
                {
                    emailOK = false;
                    WarningBlock.Text = "Invalid email address. An email has to have one and only one '@' symbol.";
                }
            }

            if (emailOK) { ValidateData(); }
        }
    }
}
