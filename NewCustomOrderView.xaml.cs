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
    /// Interaction logic for NewCustomOrderView.xaml
    /// </summary>
    public partial class NewCustomOrderView : UserControl
    {
        private Database DB;
        private List<Customer> ToSearch;
        private List<Customer> Searched;
        private bool customerOK;
        private bool cakesizeOK;
        private bool descriptionOK;
        private bool priceOK;

        public NewCustomOrderView(Database data)
        {
            DB = data;

            ToSearch = DB.GetListOfCustomers();
            try
            {
                Searched = new List<Customer>();

                RegisteredCustomerCmb.ItemsSource = ToSearch;
            }
            catch { }

            InitializeComponent();

            CakeSizeTextBox.Items.Add("Small");
            CakeSizeTextBox.Items.Add("Medium");
            CakeSizeTextBox.Items.Add("Large");

            RegisteredCustomerCmb.ItemsSource = DB.GetListOfCustomers();

            ConfirmButton.IsEnabled = false;

            customerOK = false;
            cakesizeOK = false;
            descriptionOK = false;
            priceOK = false;
        }

        public void ValidateData()
        {
            if (customerOK && cakesizeOK && descriptionOK && priceOK)
            {
                WarningBlock.Text = "";
                ConfirmButton.IsEnabled = true;
            }
            else { ConfirmButton.IsEnabled = false; }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new GeneralCustomOrdersView(DB);
        }

        private void MMButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Searched = new List<Customer>();

            if (IDTextBox.Text != "")
            {
                try
                {
                    for (int i = 0; i < ToSearch.Count; i++)
                    {
                        if (ToSearch[i].getPersonID() == Convert.ToInt32(IDTextBox.Text)) { Searched.Add(ToSearch[i]); }
                    }

                    RegisteredCustomerCmb.ItemsSource = Searched;
                }
                catch (FormatException exception) { MessageBox.Show("Customer ID needs to be a number."); }
            }
            else if (FirstNameTextBox.Text != "" || LastNameTextBox.Text != "")
            {
                if (FirstNameTextBox.Text != "")
                {
                    for (int i = 0; i < ToSearch.Count; i++)
                    {
                        if (ToSearch[i].getFirstName().ToLower() == FirstNameTextBox.Text.ToLower()) { Searched.Add(ToSearch[i]); }
                    }

                    ToSearch = Searched;
                }

                if (LastNameTextBox.Text != "")
                {
                    for (int i = 0; i < ToSearch.Count; i++)
                    {
                        if (ToSearch[i].getSurname().ToLower() == LastNameTextBox.Text.ToLower()) { Searched.Add(ToSearch[i]); }
                    }
                }

                RegisteredCustomerCmb.ItemsSource = Searched;
            }
            else { RegisteredCustomerCmb.ItemsSource = DB.GetListOfCustomers(); }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int newID = 0;
            if (DB.GetListOfCustomOrders().Count > 0)
            {
                newID = DB.GetListOfCustomOrders()[DB.GetListOfCustomOrders().Count - 1].getOrderID() + 1;
            }
            decimal price = Decimal.Parse(PriceTextBox.Text);

            bool orderOutsourced = (bool)Outsourced.IsChecked;
            Customer SelectedCustomer = (Customer)RegisteredCustomerCmb.SelectedItem;

            CustomOrder newCustOrder = new CustomOrder(newID, SelectedCustomer.getPersonID(), CakeSizeTextBox.SelectedItem.ToString(), orderOutsourced, DescriptionTextBox.Text, price);

            if (!newCustOrder.AssignBaker(DB))
            {
                newCustOrder.setBakerID(-1);
                MessageBox.Show("Cannot assign a baker: the number of orders each baker has equals to the maximum of orders per baker. Please assign the baker manually in the orders management tab.");
            }

            DB.AddCustomOrder(newCustOrder);

            MessageBox.Show("Order placed.");
        }

        private void RegisteredCustomerCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RegisteredCustomerCmb.SelectedIndex == -1) { WarningBlock.Text = "Please select customer."; customerOK = false; }
            else { customerOK = true; WarningBlock.Text = ""; }
            ValidateData();

        }

        private void CakeSizeTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CakeSizeTextBox.SelectedIndex == -1) { WarningBlock.Text = "Please select cake size."; cakesizeOK = false; }
            else { cakesizeOK = true; WarningBlock.Text = ""; }
            ValidateData();
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DescriptionTextBox.Text == "") { WarningBlock.Text = "Please enter description."; descriptionOK = false; }
            else { descriptionOK = true; WarningBlock.Text = ""; }
            ValidateData();
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal price = 0;
            if (PriceTextBox.Text == "") { WarningBlock.Text = "Please enter price."; priceOK = false; }
            else if (!decimal.TryParse(PriceTextBox.Text, out price)) { WarningBlock.Text = "Please only use numbers for price!"; priceOK = false; }
            else { priceOK = true; WarningBlock.Text = ""; }
            ValidateData();
        }
    }
}
