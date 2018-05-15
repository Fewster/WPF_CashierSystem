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
    /// Interaction logic for Promotions.xaml
    /// </summary>
    public partial class Promotions : UserControl
    {
        private Database DB;
        public Promotions(Database data)
        {
            DB = data;
            InitializeComponent();
        }

        private void CreateNewPromo_Click(object sender, RoutedEventArgs e)
        {
            Content = new CreateNewPromo(DB);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new Checkout_Menu(DB);
        }

        private void DeletePromo_Click(object sender, RoutedEventArgs e)
        {
            Content = new DeletePromotionView(DB);
        }
    }
}
