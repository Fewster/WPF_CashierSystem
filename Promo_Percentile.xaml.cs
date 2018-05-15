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
    /// Interaction logic for Promo_Percentile.xaml
    /// </summary>
    public partial class Promo_Percentile : UserControl
    {
        private Database DB;
        public Promo_Percentile(Database data)
        {
            DB = data;
            InitializeComponent();
            Type.Items.Add("All");
            Type.Items.Add("Pastries");
            Type.Items.Add("Buns");
            Type.Items.Add("Cakes");
            Type.Items.Add("Bread");
            Type.Items.Add("Drinks");
            Type.Items.Add("Misc");

            Type.SelectedValue = "All";

            //Product testprod = new Product("test", "Buns",9,"g" 0.51m, " ");
            //Product testprod2 = new Product("test2", "Drinks", 0.02m, " ");

            //DB.AddItem(0,"TestBuns","Buns",9,0,0.51m, "Bread",true);
            //DB.AddItem(0, "TestDrinks", "Cans", 9, 0.02m, 0.9m, "Drink", true);

            UpdateCombo();
                 
        }

        public void UpdateCombo()
        {
            Product.Items.Clear();

            foreach (Product product in DB.GetListOfProducts())
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = product;

                try
                {
                    if (Type.SelectedValue.ToString() == "All")
                    {
                        Product.Items.Add(newItem);
                    }

                    else if (product.GetProdType() == Type.SelectedValue.ToString())
                    {
                        Product.Items.Add(newItem);
                    }
                }
                catch
                {

                }
                                  
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Product.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select product.");
                }

                else if (PromoName.Text == "")
                {
                    MessageBox.Show("Please enter promotion name.");
                }

                else if (Percent.Text == "")
                {
                    MessageBox.Show("Please enter a percentage.");
                }

                else
                {
                    ComboBoxItem comboBoxItem = Product.SelectedItem as ComboBoxItem;
                    Product item = comboBoxItem.Content as Product;
                    PromotionPercentile promo = new PromotionPercentile(PromoName.Text, Convert.ToInt32(Percent.Text), item);             
                    DB.AddPromotion(promo);
                    Content = new Checkout_Menu(DB);
                }
            }

            catch(FormatException exception)
            {
                MessageBox.Show("Please only use intergers for percentage");
            }



        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new CreateNewPromo(DB);
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCombo();
        }

    }
}
