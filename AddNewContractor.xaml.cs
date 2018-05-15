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
    /// Interaction logic for AddNewContractor.xaml
    /// </summary>
    public partial class AddNewContractor : Window
    {
        private Database DB;
        private bool businessNameOK;
        private bool firstNameOK;
        private bool surnameOK;
        private bool addressOK;
        private bool phoneOK;
        private bool emailOK;
        private bool IsSupplier;

        public AddNewContractor(Database data, bool Supplier)
        {
            InitializeComponent();

            DB = data;
            SaveButton.IsEnabled = false;
            businessNameOK = false;
            firstNameOK = false;
            surnameOK = false;
            addressOK = false;
            phoneOK = false;
            emailOK = false;
            IsSupplier = Supplier;
        }

        public void ValidateData()
        {
            if (((firstNameOK && surnameOK) || businessNameOK) && addressOK && (phoneOK || emailOK)) { SaveButton.IsEnabled = true; WartingBlock.Text = ""; }
            else { SaveButton.IsEnabled = false; }
        }

        private void BusinessNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BusinessNameTextBox.Text != "" || (firstNameOK && surnameOK)) { businessNameOK = true; WartingBlock.Text = ""; }
            else if (!(firstNameOK || surnameOK)) { businessNameOK = false; WartingBlock.Text = "Input business name or contractor's full name"; }
            ValidateData();
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstNameTextBox.Text != "" || businessNameOK) { firstNameOK = true; WartingBlock.Text = ""; }
            else if (!businessNameOK) { firstNameOK = false; WartingBlock.Text = "Input business name or contractor's full name"; }
            ValidateData();
        }

        private void SurnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SurnameTextBox.Text != "" || businessNameOK) { surnameOK = true; WartingBlock.Text = ""; }
            else if (!businessNameOK) { businessNameOK = false; WartingBlock.Text = "Input business name or contractor's full name"; }
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (IsSupplier == true)
            {
                
                    DB.AddExternalBusiness(FirstNameTextBox.Text, SurnameTextBox.Text, AddressTextBox.Text, PhoneNumberTextBox.Text, EmailTextBox.Text, BusinessNameTextBox.Text, false);
                }
            else { DB.AddExternalBusiness(FirstNameTextBox.Text, SurnameTextBox.Text, AddressTextBox.Text, PhoneNumberTextBox.Text, EmailTextBox.Text, BusinessNameTextBox.Text, true); }
            
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
