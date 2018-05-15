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
using System.Text.RegularExpressions;

namespace LOVEiT_BAKERY
{
    /// <summary>
    /// Interaction logic for AddNewItem.xaml
    /// </summary>
    public partial class AddNewItem : UserControl
    {
        bool NameValid;
        bool CostValid;
        bool SupplierValid;
        bool UnitsValid;
        bool IsProduct;
        bool TypeValid;
        bool QuantityValid;
        bool Minvalid;
        bool PriceValid;
        Database DB;
        
        public AddNewItem(Database pDB)
        {
            InitializeComponent();
            DB = pDB;
            SupplierMenu.ItemsSource = DB.GetListofSuppliers();
            string[] Types = new string[7];
            Types[0] = "Ingredient";
            Types[1] = "Pastries";
            Types[2] = "Buns";
            Types[3] = "Cakes";
            Types[4] = "Bread";
            Types[5] = "Drinks";
            Types[6] = "Misc.";
            TypesComboBox.ItemsSource = Types;
        }
        private bool IsLetters(string Input)
        {
            if (Regex.IsMatch(Input, @"^[a-zA-Z]+$"))
            {
                return true;
            }
            return false;
        }
        private bool IsNumbers(string Input)
        {


            if (Regex.IsMatch(Input, @"^\d+$"))
            {
                return true;
            }
            return false;
        }
        private bool IsEmpty(string input)
        {
            if (input == "")
            {
                return false;
            }
            return true;
        }
        private void ValidateBtn_Click(object sender, RoutedEventArgs e)
        {
            ExternalBusiness Supplier = (ExternalBusiness)SupplierMenu.SelectedItem;
            string Type = "";
            string Name = NameTextBox.Text;
            int SupplierID = Supplier.getPersonID();
            string Units = UnitsTextBox.Text;
            double Quantity = double.Parse(QuantityTextBox.Text);
            decimal Cost = decimal.Parse(CostTextBox.Text);
            decimal Price = 0;
            Type = TypesComboBox.SelectedItem.ToString();
            try { Price = decimal.Parse(PriceTextBox.Text); }
            catch { IsProduct = false; };
            int Min = int.Parse(MinTextBox.Text);
            DB.AddItem(SupplierID, Name, Units, Quantity, Cost, Price,Type, Min, IsProduct);
            Content = new StockManagement(DB);
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Content = new StockManagement(DB);
        }
        private void ValidationCheck()
        {
            if (NameValid && CostValid && SupplierValid && UnitsValid && QuantityValid && TypeValid && Minvalid && PriceValid)
            {
                ValidateBtn.IsEnabled = true;
            }
            else
                ValidateBtn.IsEnabled = false;
        }
        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NameValid = false;
            string input = NameTextBox.Text;
            if (IsEmpty(input) == false)
            {
                NameTextBox.BorderBrush = Brushes.Red;
                NameValid = false;

            }
            else
            {
                NameTextBox.BorderBrush = Brushes.Black;
                NameValid = true;
            }
            ValidationCheck();
        }

        private void UnitsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UnitsValid = false;
            string input = UnitsTextBox.Text;
            if (IsEmpty(input) == false)
            {
                UnitsTextBox.BorderBrush = Brushes.Red;
                UnitsValid = false;
            }
            if (IsLetters(input) == true)
            {
                UnitsTextBox.BorderBrush = Brushes.Black;
                UnitsValid = true;  
            }
            ValidationCheck();
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QuantityValid = false;
            string input = QuantityTextBox.Text;
            if (IsEmpty(input) == false)
            {
                QuantityTextBox.BorderBrush = Brushes.Red;
                QuantityValid = false;
            }
            if (IsNumbers(input) == true)
            {
                QuantityTextBox.BorderBrush = Brushes.Black;
                QuantityValid = true;
            }
            else { QuantityTextBox.BorderBrush = Brushes.Red; }
            ValidationCheck();
        }
        private void CostTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = CostTextBox.Text;
            if (IsEmpty(input) == false)
            {
                CostValid = false;
                CostTextBox.BorderBrush = Brushes.Red;
            }
            if (IsNumbers(input) == true)
            {
                CostValid = true;
                CostTextBox.BorderBrush = Brushes.Black;
            }else { CostTextBox.BorderBrush = Brushes.Red; }
            ValidationCheck();
        }
        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PriceValid = false;
            string input = PriceTextBox.Text;
            if (IsEmpty(input) == false)
            {
                IsProduct = false;
                PriceValid = true;
                PriceTextBox.BorderBrush = Brushes.Black;

            }else if (IsNumbers(input) == true)
            {
                PriceValid = true;
                IsProduct = true;
                PriceTextBox.BorderBrush = Brushes.Black;
            }else
            {
                PriceTextBox.BorderBrush = Brushes.Red;
            }
            ValidationCheck();
        }

        private void SupplierMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SupplierValid = true;
            ValidationCheck();
        }

        private void TypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeValid = true;
            ValidationCheck();
        }

        private void MinTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string input = MinTextBox.Text;
            if (IsEmpty(input))
            {
                Minvalid = false;
                MinTextBox.BorderBrush = Brushes.Red;
            }
            if (IsNumbers(input))
            {
                Minvalid = true;
                MinTextBox.BorderBrush = Brushes.Black;
            }
            else { MinTextBox.BorderBrush = Brushes.Red; }
            ValidationCheck();
        }
    }
}
