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
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LOVEiT_BAKERY
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private Database DB;

        public MainMenu(Database data)
        {
            DB = data;
            InitializeComponent();

            if (DB.GetCurrentSession().getIsAdmin() == false) { Admin.IsEnabled = false; }
        }

        private void GeneralCustomOrders_Click(object sender, RoutedEventArgs e)
        {
            Content = new GeneralCustomOrdersView(DB);          
        }

        private void CheckoutScreen_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Menu(DB);           
        }

        private void StocksScreen_Click(object sender, RoutedEventArgs e)
        {
            Content = new StockManagement(DB);
        }

        private void LOGOUT_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Database));
            using (TextWriter writer = new StreamWriter("outputTest.xml"))
            {
                xmlSerializer.Serialize(writer, DB);
            }

            Content = new Login(DB);
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            Content = new AdministrativeView(DB);
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword window = new ChangePassword(DB);
            window.ShowDialog();
        }
    }
}
