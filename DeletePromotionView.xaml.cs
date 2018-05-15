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
    /// Interaction logic for DeletePromotionView.xaml
    /// </summary>
    public partial class DeletePromotionView : UserControl
    {
        private Database DB;
        public DeletePromotionView(Database data)
        {
            InitializeComponent();
            DB = data;
            PromotionsView.ItemsSource = DB.GetListOfPromotions();
        }

        private void RemovePromoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PromotionsView.SelectedItem != null)
                {
                    DB.RemovePromotion((Promo)PromotionsView.SelectedItem);
                }
            }
            catch { }

            PromotionsView.ClearValue(ListView.ItemsSourceProperty);
            PromotionsView.ItemsSource = DB.GetListOfPromotions();
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Menu(DB);
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
