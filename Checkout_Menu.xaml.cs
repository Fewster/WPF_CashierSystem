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
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LOVEiT_BAKERY
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout_Menu : UserControl
    {

        private Database DB;
        Checkout Current = new Checkout();
        public Checkout_Menu(Database data)
        {
            InitializeComponent();
            DB = data;
            Current = DB.GetCurrentCheckout();
            listBox.ItemsSource = Current.GetCollection();
        }

        private void MAINMENU_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
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

        private void CUSTOMORDER_Click(object sender, RoutedEventArgs e)
        {
            Content = new GeneralCustomOrdersView(DB);
        }

        private void STOCKS_Click(object sender, RoutedEventArgs e)
        {
            Content = new StockManagement(DB);
        }

        private void PROMOS_Click(object sender, RoutedEventArgs e)
        {
            Content = new Promotions(DB);
        }

        private void PASTRIES_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Categorised(DB,"Pastries");
        }

        private void BUNS_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Categorised(DB,"Buns");
        }

        private void CAKES_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Categorised(DB, "Cake");
        }

        private void BREAD_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Categorised(DB,"Bread");
        }

        private void DRINKS_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Categorised(DB,"Drink");
        }

        private void MISC_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Categorised(DB,"Misc.");
        }

        private void ORDERLIST_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Categorised(DB, "Custom Orders");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBox.SelectedItem.GetType() != typeof(CustomOrder))
                {
                    KeyValuePair<Product, int> productToAdd = (KeyValuePair<Product, int>)listBox.SelectedItem;
                    DB.AddItemToCheckout(productToAdd.Key);
                    listBox.ItemsSource = Current.GetCollection();
                }
            } catch { }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.RemoveItemFromCheckout(listBox.SelectedItem);
                listBox.ItemsSource = Current.GetCollection();
            } catch { }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBox.SelectedItem.GetType() == typeof(CustomOrder))
                {
                    DB.RemoveItemFromCheckout(listBox.SelectedItem);
                }
                else
                {
                    DB.RemoveItemFromCheckoutCompletely(listBox.SelectedItem);
                }

                listBox.ItemsSource = Current.GetCollection();
            } catch { }
        }

        private void CONFIRM_Click(object sender, RoutedEventArgs e)
        {
            FinalisingCheckout window = new FinalisingCheckout(DB);
            window.ShowDialog();

            if (window.DialogResult != null)
            {
                if ((bool)window.DialogResult)
                {
                    if (DB.GetCurrentCheckout().GetCustomOrders().Count > 0)
                    {
                        foreach (CustomOrder order in DB.GetCurrentCheckout().GetCustomOrders())
                        {
                            order.setIsPaid(true);
                        }
                    }

                    if (DB.GetCurrentCheckout().GetProducts().Count > 0)
                    {
                        foreach (KeyValuePair<Product, int> KVP in DB.GetCurrentCheckout().GetProducts())
                        {
                            KVP.Key.SubtractQuantity(DB.GetCurrentCheckout().GetProducts()[KVP.Key]);
                        }
                    }
                    DB.ClearCheckout();
                    Current = DB.GetCurrentCheckout();
                    listBox.ItemsSource = Current.GetCollection();
                }
            }
        }
    }
}
