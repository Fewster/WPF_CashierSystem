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
    /// Interaction logic for Bread.xaml
    /// </summary>
    public partial class Checkout_Categorised : UserControl
    {
        
        Database OldDB = new Database();
        Database DB;
        Checkout CurrentCheckout;

        public Checkout_Categorised(Database Database, string Category)
        {
            OldDB = Database;
            DB = Database;
            CurrentCheckout = DB.GetCurrentCheckout();
            InitializeComponent();
            Label.Content = Category;
            
            if (Category == "Custom Orders")
            {
                List<CustomOrder> CustomOrders = new List<CustomOrder>();
                foreach (CustomOrder custOrder in DB.GetListOfCustomOrders())
                {
                    if (!custOrder.getIsPaid() && custOrder.getActiveOrder())
                    {
                        CustomOrders.Add(custOrder);
                    }
                }
                listBoxItems.ItemsSource = CustomOrders;

            }
            else
            {
                List<Product> RawProducts = DB.GetListOfProducts();
                List<Product> Products = new List<Product>();
                foreach (Product Product in RawProducts)
                {
                    if (Product.GetItemType() == Category)
                    {
                        Products.Add(Product);
                    }
                }
                listBoxItems.ItemsSource = Products;
            }

            listBoxCheckout.ItemsSource = CurrentCheckout.GetCollection();
            UpdateTotal();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Menu(OldDB);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DB.AddItemToCheckout(listBoxItems.SelectedItem);      
            listBoxCheckout.ItemsSource = CurrentCheckout.GetCollection();
            UpdateTotal();
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Menu(DB);
        }
        public void UpdateTotal()
        {
            TotalTextBlock.Text = CurrentCheckout.CalculateTotalWithoutCustOrders(DB.GetListOfPromotions()).ToString();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            DB.RemoveItemFromCheckout(listBoxCheckout.SelectedItem);
            listBoxCheckout.ItemsSource = CurrentCheckout.GetCollection();
            UpdateTotal();
        }
    }
}
