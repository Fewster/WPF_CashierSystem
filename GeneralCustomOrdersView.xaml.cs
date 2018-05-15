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
    /// Interaction logic for GeneralCustomOrdersView.xaml
    /// </summary>
    public partial class GeneralCustomOrdersView : UserControl
    {
        private Database DB; 
        public GeneralCustomOrdersView(Database Data)
        {
            DB = Data;
            InitializeComponent();
        }

        private void CustomerRegButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new CustomerRegistrationView(DB);
        }

        private void CreateCustomOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new NewCustomOrderView(DB);
        }

        private void MMButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
        }

        private void ManageOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new ManageCustomOrders(DB);
        }

        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new CustomersListSearchView(DB);
        }
    }
}
