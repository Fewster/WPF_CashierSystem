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
    /// Interaction logic for ManageCustomOrders.xaml
    /// </summary>
    public partial class ManageCustomOrders : UserControl
    {
        private Database DB;
        private List<CustomOrder> displayList;

        public ManageCustomOrders(Database data)
        {
            DB = data;
            displayList = DB.GetListOfCustomOrders();
            InitializeComponent();
            OrderDetailsButton.IsEnabled = false;
            EditOrderButton.IsEnabled = false;

            PaidCmb.Items.Add("All");
            PaidCmb.Items.Add("Paid for");
            PaidCmb.Items.Add("Unpaid");
            PaidCmb.SelectedValue = "All";

            ReadyToCollectCmb.Items.Add("All");
            ReadyToCollectCmb.Items.Add("Ready to collect");
            ReadyToCollectCmb.Items.Add("Not ready to collect");
            ReadyToCollectCmb.SelectedValue = "All";

            CollectedCmb.Items.Add("All");
            CollectedCmb.Items.Add("Collected");
            CollectedCmb.Items.Add("Not collected");
            CollectedCmb.SelectedValue = "All";

            ActiveOrdersCmb.Items.Add("All");
            ActiveOrdersCmb.Items.Add("Active");
            ActiveOrdersCmb.Items.Add("Completed");
            ActiveOrdersCmb.SelectedValue = "All";

            AssignedCMB.Items.Add("All");
            AssignedCMB.Items.Add("Assigned");
            AssignedCMB.Items.Add("Not assigned");
            AssignedCMB.SelectedValue = "All";

            UpdateCustomOrdersListView();
        }

        private void UpdateCustomOrdersListView()
        {
            CustomOrdersView.ClearValue(ListView.ItemsSourceProperty);
            CustomOrdersView.ItemsSource = displayList;
        }

        private void PaidCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCustomOrders();
            UpdateCustomOrdersListView();
        }

        private void ReadyToCollectCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCustomOrders();
            UpdateCustomOrdersListView();
        }

        private void CollectedCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCustomOrders();
            UpdateCustomOrdersListView();
        }

        private void ActiveOrdersCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCustomOrders();
            UpdateCustomOrdersListView();
        }

        private void AssignedCMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCustomOrders();
            UpdateCustomOrdersListView();
        }

        private void FilterCustomOrders()
        {
            List<CustomOrder> oldList = DB.GetListOfCustomOrders();
            displayList = oldList;

            try
            {
                if (PaidCmb.SelectedValue.ToString() == "Paid for")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getIsPaid() == true)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                else if (PaidCmb.SelectedValue.ToString() == "Unpaid")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getIsPaid() == false)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                if (ReadyToCollectCmb.SelectedValue.ToString() == "Ready to collect")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getReadyToCollect() == true)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                else if (ReadyToCollectCmb.SelectedValue.ToString() == "Not ready to collect")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getReadyToCollect() == false)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                if (CollectedCmb.SelectedValue.ToString() == "Collected")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getCollected() == true)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                else if (CollectedCmb.SelectedValue.ToString() == "Not collected")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getCollected() == false)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                if (ActiveOrdersCmb.SelectedValue.ToString() == "Active")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getActiveOrder() == true)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                else if (ActiveOrdersCmb.SelectedValue.ToString() == "Completed")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getActiveOrder() == false)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                if (AssignedCMB.SelectedValue.ToString() == "Assigned")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getBakerID() != -1)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }

                else if (AssignedCMB.SelectedValue.ToString() == "Not assigned")
                {
                    displayList = new List<CustomOrder>();
                    foreach (CustomOrder customOrder in oldList)
                    {
                        if (customOrder.getBakerID() == -1)
                        {
                            displayList.Add(customOrder);
                        }
                    }
                    oldList = displayList;
                }
            }
            catch { }

            OrderDetailsButton.IsEnabled = false;
            EditOrderButton.IsEnabled = false;
        }

        private void OrderDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            CustomOrder custOrder = (CustomOrder)CustomOrdersView.SelectedItem;

            if (custOrder != null)
            {
                OrderDetails window = new OrderDetails(DB, custOrder);
                window.ShowDialog();
            }
        }

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            CustomOrder custOrder = (CustomOrder)CustomOrdersView.SelectedItem;

            if (custOrder != null)
            {
                EditCustomOrder window = new EditCustomOrder(DB, custOrder);
                window.ShowDialog();

                if (window.DialogResult != null)
                {
                    if ((bool)window.DialogResult)
                    {
                        UpdateCustomOrdersListView();
                    }
                }
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Database));
            using (TextWriter writer = new StreamWriter("outputTest.xml"))
            {
                xmlSerializer.Serialize(writer, DB);
            }

            Content = new Login(DB);
        }

        private void CustomOrdersView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderDetailsButton.IsEnabled = true;
            EditOrderButton.IsEnabled = true;
        }
    }
}
