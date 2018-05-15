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
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Window
    {
        private CustomOrder customOrder;
        private Database DB;
        public OrderDetails(Database data, CustomOrder custOrder)
        {
            DB = data;
            customOrder = custOrder;
            InitializeComponent();
            DisplayOrderDetails();
        }

        public void DisplayOrderDetails()
        {
            Customer orderCustomer = null;
            Staff orderStaff = null;
            ExternalBusiness orderContractor = null;
            string customerDetails = "";
            string bakerDetails = "";

            foreach (Customer customer in DB.GetListOfCustomers())
            {
                if (customer.getPersonID() == customOrder.getCustomerID())
                {
                    orderCustomer = customer;
                }
            }

            if (customOrder.getOrderOutsourced())
            {
                foreach (ExternalBusiness contractor in DB.GetListOfContractors())
                {
                    if (contractor.getPersonID() == customOrder.getBakerID()) { orderContractor = contractor; }
                }
            }
            else
            {
                foreach (Staff baker in DB.GetListOfStaffMembers())
                {
                    if (baker.getPersonID() == customOrder.getBakerID())
                    {
                        orderStaff = baker;
                    }
                }
            }

            if (customOrder.getActiveOrder()) { ActiveOrderTextBlock.Text = "Active"; } else { ActiveOrderTextBlock.Text = "Inactive"; }
            IDTextBlock.Text = customOrder.getOrderID().ToString();
            CustIDTextBlock.Text = customOrder.getCustomerID().ToString();
            if (orderCustomer != null)
            {
                customerDetails = orderCustomer.getFirstName() + " " + orderCustomer.getSurname();
                if (orderCustomer.getPhone() != "") { customerDetails = customerDetails + ", " + orderCustomer.getPhone(); }
                if (orderCustomer.getEmail() != "") { customerDetails = customerDetails + ", " + orderCustomer.getEmail(); }
            }
            CustDetailsTextBlock.Text = customerDetails;

            BakerIDTextBlock.Text = customOrder.getBakerID().ToString();
            if (orderStaff != null)
            {
                BakerContractorTextBlock.Text = "In-shop baker";
                bakerDetails = orderStaff.getFirstName() + " " + orderStaff.getSurname();
            }
            else
            {
                try
                {
                    BakerContractorTextBlock.Text = "Contractor";
                    if (orderContractor.GetBusinessName() != "") { bakerDetails = orderContractor.GetBusinessName() + ", "; }
                    bakerDetails = bakerDetails + orderContractor.getFirstName() + " " + orderContractor.getSurname();
                }
                catch { }
            }
            BakerDetailsTextBlock.Text = bakerDetails;

            SizeTextBlock.Text = customOrder.getOrderSize();
            DescriptionTextBlock.Text = customOrder.getOrderDescription();
            PriceTextBlock.Text = "£" + customOrder.getOrderPrice().ToString();
            if (customOrder.getIsPaid()) { PaidTextBlock.Text = "Paid"; } else { PaidTextBlock.Text = "Not paid"; }
            if (customOrder.getReadyToCollect()) { ReadyToCollectTextBlock.Text = "Ready for collection"; } else { ReadyToCollectTextBlock.Text = "Not ready"; }
            if (customOrder.getCollected()) { CollectedBlock.Text = "Collected"; } else { CollectedBlock.Text = "Not collected"; }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
