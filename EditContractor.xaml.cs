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
    /// Interaction logic for EditContractor.xaml
    /// </summary>
    public partial class EditContractor : Window
    {
        private ExternalBusiness contractor;
        public EditContractor(ExternalBusiness contractorToEdit)
        {
            InitializeComponent();
            contractor = contractorToEdit;

            BusinessNameTextBox.Text = contractorToEdit.GetBusinessName();
            FirstNameTextBox.Text = contractorToEdit.getFirstName();
            SurnameTextBox.Text = contractorToEdit.getSurname();
            AddressTextBox.Text = contractorToEdit.getAddress();
            PhoneNumberTextBox.Text = contractorToEdit.getPhone();
            EmailTextBox.Text = contractorToEdit.getEmail();
            SaveButton.IsEnabled = false;
        }

        private void BusinessNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BusinessNameTextBox.Text != "") { SaveButton.IsEnabled = true; WartingBlock.Text = ""; }
            else { SaveButton.IsEnabled = false; WartingBlock.Text = "Input business name"; }
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstNameTextBox.Text != "") { SaveButton.IsEnabled = true; WartingBlock.Text = ""; }
            else { SaveButton.IsEnabled = false; WartingBlock.Text = "Input first name"; }
        }

        private void SurnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SurnameTextBox.Text != "") { SaveButton.IsEnabled = true; WartingBlock.Text = ""; }
            else { SaveButton.IsEnabled = false; WartingBlock.Text = "Input surname"; }
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddressTextBox.Text != "") { SaveButton.IsEnabled = true; WartingBlock.Text = ""; }
            else { SaveButton.IsEnabled = false; WartingBlock.Text = "Input address"; }
        }

        private void PhoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneNumberTextBox.Text == "" && EmailTextBox.Text == "")
            { SaveButton.IsEnabled = false; WartingBlock.Text = "Please input either phone number or email."; }
            else if (PhoneNumberTextBox.Text != "")
            {
                bool isNumeric = true;
                int lengthCount = 0;
                foreach (char c in PhoneNumberTextBox.Text)
                {
                    int isInt;
                    if (int.TryParse(c.ToString(), out isInt) == false)
                    { isNumeric = false; SaveButton.IsEnabled = false; }
                    lengthCount++;
                }
                if (isNumeric == false)
                { WartingBlock.Text = "One or more characters used in the phone number are not a number. Please use only numbers."; }
                else if (lengthCount < 8)
                { SaveButton.IsEnabled = false; WartingBlock.Text = "The phone number is too short."; }
                else if (lengthCount > 11)
                { SaveButton.IsEnabled = false; WartingBlock.Text = "The phone number is too long."; }
                else { SaveButton.IsEnabled = true; WartingBlock.Text = ""; }
            }
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool emailWorks = true;
            if (PhoneNumberTextBox.Text == "" && EmailTextBox.Text == "")
            { SaveButton.IsEnabled = false; emailWorks = false; WartingBlock.Text = "Please input either phone number or email."; }
            else if (EmailTextBox.Text != "")
            {
                int atcount = 0;
                int charcount = 0;
                int totalcharcount = 0;
                foreach (char c in EmailTextBox.Text)
                {
                    if (c.ToString().Equals("@"))
                    {
                        if (totalcharcount == 0) { SaveButton.IsEnabled = false; emailWorks = false; WartingBlock.Text = "Invalid email address. An email cannot start with an '@'."; }
                        else if (totalcharcount == EmailTextBox.Text.Length - 1) { SaveButton.IsEnabled = false; emailWorks = false; WartingBlock.Text = "Invalid email address. An email cannot end on a '@'."; }
                        atcount++;
                    }

                    if (atcount == 1)
                    {
                        if (c.ToString().Equals("."))
                        {
                            if (charcount == 1) { SaveButton.IsEnabled = false; emailWorks = false; WartingBlock.Text = "Invalid email address. A '.' cannot be directly after '@'."; }
                            else if (totalcharcount == EmailTextBox.Text.Length - 1) { SaveButton.IsEnabled = false; emailWorks = false; WartingBlock.Text = "Invalid email address. An email cannot end on a '.'."; }
                        }
                        charcount++;
                    }

                    totalcharcount++;
                }

                if (atcount == 0 || atcount > 1)
                {
                    SaveButton.IsEnabled = false;
                    emailWorks = false;
                    WartingBlock.Text = "Invalid email address. An email has to have one and only one '@' symbol.";
                }
            }

            if (emailWorks == true)
            {
                SaveButton.IsEnabled = true;
                WartingBlock.Text = "";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            if (contractor.GetBusinessName() != BusinessNameTextBox.Text) { contractor.SetBusinessName(BusinessNameTextBox.Text); }
            if (contractor.getFirstName() != FirstNameTextBox.Text) { contractor.setFirstName(FirstNameTextBox.Text); }
            if (contractor.getSurname() != SurnameTextBox.Text) { contractor.setSurname(SurnameTextBox.Text); }
            if (contractor.getAddress() != AddressTextBox.Text) { contractor.setAddress(AddressTextBox.Text); }
            if (contractor.getPhone() != PhoneNumberTextBox.Text) { contractor.setPhone(PhoneNumberTextBox.Text); }
            if (contractor.getEmail() != EmailTextBox.Text) { contractor.setEmail(EmailTextBox.Text); }

            MessageBox.Show("Contractor details have been changed.");
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
