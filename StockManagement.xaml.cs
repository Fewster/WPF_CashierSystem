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
using Microsoft.Office.Interop.Excel;

namespace LOVEiT_BAKERY
{
    /// <summary>
    /// Interaction logic for StockManagement.xaml
    /// </summary>
    public partial class StockManagement : UserControl
    {
        Database DB = new Database();
        Item ActiveItem;
        Product ActiveProduct;
        List<object> Inventory = new List<object>();
        public StockManagement(Database pDB)
        {
            InitializeComponent();
            DB = pDB;
            GenerateInventory();
            LstBoxItems.ItemsSource = Inventory; ;
        }

        private void GenerateInventory()
        {
            Inventory.Clear();
            foreach (Item Item in DB.GetListOfItems())
            {
                Inventory.Add(Item);
            }
            foreach (Product product in DB.GetListOfProducts())
            {
                Inventory.Add(product);
            }
            for (int i = 0; i < Inventory.Count; i++)
            {

                for (int j = 0; j < Inventory.Count - 1; j++)
                    if (string.Compare(Inventory[i].ToString(), Inventory[j].ToString()) < 0)
                    {
                        object Temp = Inventory[i];
                        Inventory[i] = Inventory[j];
                        Inventory[j] = Temp;
                    }
            }
        }
        private void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Item? This action cannot be undone", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            { }
            else if (ActiveItem != null && DB.DeleteItem(ActiveItem.GetID()))
            {
            }
            else if (DB.DeleteProduct(ActiveProduct.GetID()))
            {
            }
            GenerateInventory();
            LstBoxItems.ItemsSource = null;
            LstBoxItems.Items.Clear();
            LstBoxItems.ItemsSource = Inventory;
        }
        private void CommandBinding_New(object sender, ExecutedRoutedEventArgs e)
        {
            Content = new AddNewItem(DB);
        }
        private void LstBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Product ComparatorProd = new Product();
                Object Select = LstBoxItems.SelectedItem;
                if (Select.GetType() == ComparatorProd.GetType())
                {
                    ActiveProduct = LstBoxItems.SelectedItem as Product;
                    PriceDisplay.Visibility = System.Windows.Visibility.Visible;
                    LblPrice.Visibility = System.Windows.Visibility.Visible;
                    LblType.Visibility = System.Windows.Visibility.Visible;
                    TypeDisplay.Visibility = System.Windows.Visibility.Visible;
                    SupplierDisplay.Text = ActiveProduct.GetSupplierName(DB);
                    this.DataContext = ActiveProduct;
                }
                else
                {
                    ActiveItem = LstBoxItems.SelectedItem as Item;
                    PriceDisplay.Visibility = System.Windows.Visibility.Collapsed;
                    LblPrice.Visibility = System.Windows.Visibility.Collapsed;
                    LblType.Visibility = System.Windows.Visibility.Collapsed;
                    TypeDisplay.Visibility = System.Windows.Visibility.Collapsed;
                    SupplierDisplay.Text = ActiveItem.GetSupplierName(DB);
                    this.DataContext = ActiveItem;
                }
            }
            catch { }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Item> LowSupplyItems = new List<Item>();

            foreach (Item Item in DB.GetListOfItems())
            {
                if (Item.GetQuantity() < 10)
                {
                    LowSupplyItems.Add(Item);
                }
            }
            foreach (Product Product in DB.GetListOfProducts())
            {
                if (Product.GetQuantity() < 10)
                {
                    Item Item = Product.ToItem(DB);
                    LowSupplyItems.Add(Item);
                }
            }
            for (int i = 0; i < LowSupplyItems.Count; i++)
            {
                for (int j = 0; j < LowSupplyItems.Count - 1; j++)
                    if (LowSupplyItems[i].GetSupplierID(DB) < LowSupplyItems[j].GetSupplierID(DB))
                    {
                        Item temp = LowSupplyItems[j];
                        LowSupplyItems[j] = LowSupplyItems[j + 1];
                        LowSupplyItems[j + 1] = temp;
                    }

            }
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            app.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;

            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];
            DateTime CurrentDate = DateTime.Now;
            int Acc = 3;
            ws.Range["A1"].Value = "Date:";
            ws.Range["B1"].Value = CurrentDate;
            ws.Range["A3"].Value = "Stock";
            ws.Range["B2"].Value = "Name";
            ws.Range["C2"].Value = "Remain";
            ws.Range["D2"].Value = "Cost";
            ws.Range["E2"].Value = "Supplier";
            ws.Range["F2"].Value = "Email";
            ws.Range["G2"].Value = "Tel.";

            foreach (Item Item in LowSupplyItems)
            {
                ws.Range["B" + Acc].Value = Item.GiveName();
                ws.Range["C" + Acc].Value = Item.GetQuantity() + " " + Item.GetUnits();
                ws.Range["D" + Acc].Value = "£" + Item.GetCost();
                ws.Range["E" + Acc].Value = Item.GetSupplierName(DB);
                ws.Range["F" + Acc].Value = Item.GetSupplierEmail(DB);
                ws.Range["G" + Acc].Value = Item.GetSupplierPhone(DB);
                Acc++;
            }
            int Max = Acc -1;
            Acc = 0;

            ws.Range["A3:A" + Max].BorderAround();
            ws.Range["B2:G2"].BorderAround2();



            string FileNameLocation = "G:\\SPBL ACW\\LOVEiT_BAKERY\\LOVEiT_BAKERY\\Order_Reports\\" + "Order_Report" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + DateTime.Now.ToString("HH-mm-ss") + ".xlsx";
            wb.SaveAs(FileNameLocation);
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
        }
    }

}
