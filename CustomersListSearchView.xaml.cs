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
    /// Interaction logic for CustomersListSearchView.xaml
    /// </summary>
    public partial class CustomersListSearchView : UserControl
    {
        private Database DB;
        private List<Customer> displayList;
        public CustomersListSearchView(Database data)
        {
            DB = data;
            displayList = DB.GetListOfCustomers();

            InitializeComponent();
            UpdateCustomersView();
        }

        public void UpdateCustomersView()
        {
            CustomersView.ClearValue(ListView.ItemsSourceProperty);
            CustomersView.ItemsSource = displayList;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustNameTextBox.Text == "" && CustSurnameTextBox.Text == "") { displayList = DB.GetListOfCustomers(); UpdateCustomersView(); }
            else
            {
                List<Customer> ToSearch = DB.GetListOfCustomers();
                List<Customer> Searched = new List<Customer>();

                if (CustNameTextBox.Text != "")
                {
                    foreach (Customer customer in ToSearch)
                    {
                        if (customer.getFirstName().ToLower() == CustNameTextBox.Text.ToLower())
                        { Searched.Add(customer); }
                    }

                    ToSearch = Searched;
                    Searched = new List<Customer>();
                }

                if (CustSurnameTextBox.Text != "")
                {
                    foreach (Customer customer in ToSearch)
                    {
                        if (customer.getSurname().ToLower() == CustSurnameTextBox.Text.ToLower())
                        { Searched.Add(customer); }
                    }
                }

                displayList = Searched;
                UpdateCustomersView();
            }
        }

        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = (Customer)CustomersView.SelectedItem;

            if (customer != null)
            {
                EditCustomer window = new EditCustomer(customer);
                window.ShowDialog();

                if (window.DialogResult != null)
                {
                    if ((bool)window.DialogResult)
                    {
                        UpdateCustomersView();
                    }
                }
            }
            else { MessageBox.Show("You have not selected a customer to edit!"); }
        }

        private void RemoveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = (Customer)CustomersView.SelectedItem;

            if (customer != null)
            {
                foreach (CustomOrder custOrder in DB.GetListOfCustomOrders())
                {
                    if (custOrder.getCustomerID() == customer.getPersonID() && custOrder.getActiveOrder())
                    {
                        MessageBox.Show("You cannot remove a customer who has pending orders!");
                    }
                    else if (custOrder.getCustomerID() == customer.getPersonID() && !custOrder.getActiveOrder())
                    {
                        custOrder.setCustomerID(-1);
                        DB.GetListOfCustomers().Remove(customer);
                        UpdateCustomersView();
                    }
                    else
                    {
                        DB.GetListOfCustomers().Remove(customer);
                        UpdateCustomersView();
                    }
                }
            }
            else { MessageBox.Show("You have not selected a customer to remove!"); }
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
        }

        private void CustomersView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MainMenuButtor_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
        }
    }
}
