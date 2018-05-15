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
    /// Interaction logic for EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        private Staff staff;
        public EditEmployee(Staff toEdit)
        {
            staff = toEdit;
            InitializeComponent();

            FirstNameTextBox.Text = staff.getFirstName();
            SurnameTextBox.Text = staff.getSurname();
            AddressTextBox.Text = staff.getAddress();
            PhoneNumberTextBox.Text = staff.getPhone();
            EmailTextBox.Text = staff.getEmail();
            if (staff.getIsBaker()) { BakerBox.IsChecked = true; }
            if (staff.getIsBaker())
            {
                string ids = "";
                for (int i = 0; i < staff.getOrderIDsCount(); i++)
                {
                    if (i == 0) { ids = staff.getOrderIDs()[i].ToString(); } else { ids = ids + ", " + staff.getOrderIDs()[i].ToString(); }
                }
                OrderIDsBlock.Text = ids;
            }
            else { OrderIDsBlock.Text = "Inaplicable"; }

            SaveButton.IsEnabled = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            if (staff.getFirstName() != FirstNameTextBox.Text) { staff.setFirstName(FirstNameTextBox.Text); }
            if (staff.getSurname() != SurnameTextBox.Text) { staff.setSurname(SurnameTextBox.Text); }
            if (staff.getAddress() != AddressTextBox.Text) { staff.setAddress(AddressTextBox.Text); }
            if (staff.getPhone() != PhoneNumberTextBox.Text) { staff.setPhone(PhoneNumberTextBox.Text); }
            if (staff.getEmail() != EmailTextBox.Text) { staff.setEmail(EmailTextBox.Text); }
            if (staff.getIsBaker() != (bool)BakerBox.IsChecked) { staff.setIsBaker((bool)BakerBox.IsChecked); }

            MessageBox.Show("Staff details have been changed.");
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
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

        private void BakerBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (staff.getIsBaker() == false) { SaveButton.IsEnabled = true; }
            else { if (staff.getOrderIDsCount() != 0) { SaveButton.IsEnabled = false; WartingBlock.Text = "Please reassign their orders to other bakers!"; }
                else { SaveButton.IsEnabled = true; } }
        }

        private void BakerBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!staff.getIsBaker()) { SaveButton.IsEnabled = true; }
        }
    }
}
