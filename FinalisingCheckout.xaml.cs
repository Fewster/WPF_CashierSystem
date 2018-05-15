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
    /// Interaction logic for FinalisingCheckout.xaml
    /// </summary>
    public partial class FinalisingCheckout : Window
    {
        private Database DB;
        public FinalisingCheckout(Database data)
        {
            DB = data;
            InitializeComponent();
            ToPayTextBlock.Text = DB.GetCurrentCheckout().CalculateTotal(DB.GetListOfPromotions()).ToString();
            DoneButton.IsEnabled = false;
        }

        private void PaidTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal paid = 0;
            if (PaidTextBox.Text == "") { WartingBlock.Text = "Please enter price."; DoneButton.IsEnabled = false; }
            else if (!decimal.TryParse(PaidTextBox.Text, out paid)) { WartingBlock.Text = "Please only use numbers for price!"; DoneButton.IsEnabled = false; }
            else if (Decimal.Parse(PaidTextBox.Text) < Decimal.Parse(ToPayTextBlock.Text)) { WartingBlock.Text = "Amount paid has to be more than or equal to the amount to be paid"; DoneButton.IsEnabled = false; }
            else { DoneButton.IsEnabled = true; WartingBlock.Text = ""; ChangeTextBlock.Text = (Decimal.Parse(ToPayTextBlock.Text) - Decimal.Parse(PaidTextBox.Text)).ToString(); }
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            Transaction newTransaction = new Transaction(DB.GetCurrentSession().getUserID(), DB.GetCurrentCheckout().GetProducts(), DB.GetCurrentCheckout().GetCustomOrders(), Decimal.Parse(ToPayTextBlock.Text), Decimal.Parse(PaidTextBox.Text));
            DB.AddTransaction(newTransaction);
            DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
