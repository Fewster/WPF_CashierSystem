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
    /// Interaction logic for CreateNewPromo.xaml
    /// </summary>
    public partial class CreateNewPromo : UserControl
    {
        private Database DB;
        public CreateNewPromo(Database data)
        {
            DB = data;
            InitializeComponent();
        }

        private void Percentile_Click(object sender, RoutedEventArgs e)
        {
            Content = new Promo_Percentile(DB);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Menu(DB);
        }

        private void BuyGetFree_Click(object sender, RoutedEventArgs e)
        {
            Content = new Promo_BuyGetFree(DB);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Content = new Promo_Save(DB);
        }

    }
}
