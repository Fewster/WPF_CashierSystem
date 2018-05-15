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

namespace LOVEiT_BAKERY
{
    /// <summary>
    /// Interaction logic for EditCustomer.xaml
    /// </summary>
    public partial class EditCustomer : Window
    {
        private Customer customer;
        bool firstNameOK;
        bool surnameOK;
        bool addressOK;
        bool phoneOK;
        bool emailOK;

        public EditCustomer(Customer toEdit)
        {
            customer = toEdit;
            InitializeComponent();

            FirstNameTextBox.Text = customer.getFirstName();
            SurnameTextBox.Text = customer.getSurname();
            AddressTextBox.Text = customer.getAddress();
            PhoneNumberTextBox.Text = customer.getPhone();
            EmailTextBox.Text = customer.getEmail();

            firstNameOK = true;
            surnameOK = true;
            addressOK = true;
            phoneOK = true;
            emailOK = true;

            SaveButton.IsEnabled = false;
        }

        public void ValidateData()
        {
            if (firstNameOK && surnameOK && addressOK && phoneOK && emailOK) { WartingBlock.Text = ""; SaveButton.IsEnabled = true; }
            else { SaveButton.IsEnabled = false; }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            if (customer.getFirstName() != FirstNameTextBox.Text) { customer.setFirstName(FirstNameTextBox.Text); }
            if (customer.getSurname() != SurnameTextBox.Text) { customer.setSurname(SurnameTextBox.Text); }
            if (customer.getAddress() != AddressTextBox.Text) { customer.setAddress(AddressTextBox.Text); }
            if (customer.getPhone() != PhoneNumberTextBox.Text) { customer.setPhone(PhoneNumberTextBox.Text); }
            if (customer.getEmail() != EmailTextBox.Text) { customer.setEmail(EmailTextBox.Text); }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstNameTextBox.Text != "") { firstNameOK = true; }
            else { firstNameOK = false; WartingBlock.Text = "Input first name"; }
            ValidateData();
        }

        private void SurnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SurnameTextBox.Text != "") { surnameOK = true; }
            else { surnameOK = false; WartingBlock.Text = "Input surname"; }
            ValidateData();
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddressTextBox.Text != "") { addressOK = true; }
            else { addressOK = false; WartingBlock.Text = "Input address"; }
            ValidateData();
        }

        private void PhoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneNumberTextBox.Text == "" && EmailTextBox.Text == "")
            { phoneOK = false; emailOK = false; WartingBlock.Text = "Please input either phone number or email."; }

            if (PhoneNumberTextBox.Text != "")
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
                else { phoneOK = true; }
            }

            ValidateData();
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            emailOK = true;
            if (PhoneNumberTextBox.Text == "" && EmailTextBox.Text == "")
            { emailOK = false; WartingBlock.Text = "Please input either phone number or email."; }

            if (EmailTextBox.Text != "")
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
            else { emailOK = true; WartingBlock.Text = ""; }

            ValidateData();
        }
    }
}
