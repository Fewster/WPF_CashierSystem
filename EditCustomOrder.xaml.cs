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
    /// Interaction logic for EditCustomOrder.xaml
    /// </summary>
    public partial class EditCustomOrder : Window
    {
        private Database DB;
        private CustomOrder customOrder;
        private bool BakerOK;
        private bool DescriptionOK;
        private bool PriceOK;
        private bool ReadyCheckboxOK;
        private bool CollectedCheckboxOK;
        private bool CheckboxesOK;

        public EditCustomOrder(Database data, CustomOrder custOrder)
        {
            DB = data;
            customOrder = custOrder;
            InitializeComponent();

            BakerContractorCmb.Items.Add("In-shop baker");
            BakerContractorCmb.Items.Add("Contractor");

            SizeCmb.Items.Add("Small");
            SizeCmb.Items.Add("Medium");
            SizeCmb.Items.Add("Large");

            BakerOK = true;
            DescriptionOK = true;
            PriceOK = true;
            CheckboxesOK = true;
            ReadyCheckboxOK = true;
            CollectedCheckboxOK = true;

            SaveButton.IsEnabled = false;

            LoadData();
        }

        public void LoadData()
        {
            Customer customer = null;
            string customerDetails = "";
            Staff baker = null;
            ExternalBusiness contractor = null;
            List<Staff> bakerList = new List<Staff>();
            List<ExternalBusiness> contractorList = new List<ExternalBusiness>();

            foreach (Customer cust in DB.GetListOfCustomers())
            {
                if (cust.getPersonID() == customOrder.getCustomerID())
                {
                    customer = cust;
                }
            }

            if (customer != null)
            {
                customerDetails = customer.getFirstName() + " " + customer.getSurname();
                if (customer.getPhone() != "") { customerDetails = customerDetails + ", " + customer.getPhone(); }
                if (customer.getEmail() != "") { customerDetails = customerDetails + ", " + customer.getEmail(); }
            }

            if (!customOrder.getOrderOutsourced())
            {
                foreach (Staff bkr in DB.GetListOfStaffMembers())
                {
                    if (bkr.getPersonID() == customOrder.getBakerID())
                    {
                        baker = bkr;
                    }

                    if (bkr.getIsBaker() == true)
                    {
                        bakerList.Add(bkr);
                    }
                }
            }

            if (customOrder.getOrderOutsourced())
            {
                foreach (ExternalBusiness cntrctr in DB.GetListOfContractors())
                {
                    if (cntrctr.getPersonID() == customOrder.getBakerID())
                    {
                        contractor = cntrctr;
                    }

                    contractorList.Add(cntrctr);
                }
            }
;
            CustIDCmb.ItemsSource = DB.GetListOfCustomers();
            if (customer != null) { CustIDCmb.SelectedItem = customer; }
            CustDetailsTextBlock.Text = customerDetails;

            if (customOrder.getOrderOutsourced())
            {
                BakerContractorCmb.SelectedItem = "Contractor";
                BakerIDCmb.ItemsSource = contractorList;
                if (contractor != null)
                {
                    BakerIDCmb.SelectedItem = contractor;
                    BakerDetailsTextBlock.Text = contractor.GetBusinessName() + ", " + contractor.getFirstName() + " " + contractor.getSurname();
                }
            }
            else
            {
                BakerContractorCmb.SelectedItem = "In-shop baker";
                BakerIDCmb.ItemsSource = bakerList;
                if (baker != null)
                {
                    BakerIDCmb.SelectedItem = baker;
                    BakerDetailsTextBlock.Text = baker.getFirstName() + " " + baker.getSurname();
                }
            }

            SizeCmb.SelectedItem = customOrder.getOrderSize();

            DescriptionTextBox.Text = customOrder.getDescription();
            PriceTextBox.Text = customOrder.getOrderPrice().ToString();
        }

        private void ValidateData()
        {
            if (BakerIDCmb.SelectedItem == null)
            {
                BakerOK = false;
                WartingBlock.Text = "Please select a baker";
            }

            if (ReadyCheckboxOK && CollectedCheckboxOK) { CheckboxesOK = true; } else { CheckboxesOK = false; }

            if (BakerOK && DescriptionOK && PriceOK && CheckboxesOK)
            {
                WartingBlock.Text = "";
                SaveButton.IsEnabled = true;
            }
            else { SaveButton.IsEnabled = false; }
        }

        private void CustIDCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer customer = (Customer)CustIDCmb.SelectedItem;
            string customerDetails = customer.getFirstName() + " " + customer.getSurname();
            if (customer.getPhone() != "") { customerDetails = customerDetails + ", " + customer.getPhone(); }
            if (customer.getEmail() != "") { customerDetails = customerDetails + ", " + customer.getEmail(); }
            CustDetailsTextBlock.Text = customerDetails;

            ValidateData();
        }

        private void BakerContractorCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BakerContractorCmb.SelectedItem != null)
            {
                if (BakerContractorCmb.SelectedItem == "In-shop baker")
                {
                    List<Staff> bakerList = new List<Staff>();
                    foreach (Staff bkr in DB.GetListOfStaffMembers())
                    {
                        if (bkr.getIsBaker()) { bakerList.Add(bkr); }
                    }

                    BakerIDCmb.ItemsSource = bakerList;

                    Staff baker = null;
                    if (!customOrder.getOrderOutsourced())
                    {
                        foreach (Staff bkr in bakerList)
                        {
                            if (bkr.getPersonID() == customOrder.getBakerID()) { baker = bkr; }
                        }
                        if (baker != null) { BakerIDCmb.SelectedItem = baker; }
                    }
                }

                if (BakerContractorCmb.SelectedItem == "Contractor")
                {
                    BakerIDCmb.ItemsSource = DB.GetListOfContractors();

                    ExternalBusiness contractor = null;
                    if (customOrder.getOrderOutsourced())
                    {
                        foreach (ExternalBusiness cntrctr in DB.GetListOfContractors())
                        {
                            if (cntrctr.getPersonID() == customOrder.getBakerID()) { contractor = cntrctr; }
                        }
                        if (contractor != null) { BakerIDCmb.SelectedItem = contractor; }
                    }
                }
            }

            ValidateData();
        }

        private void BakerIDCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BakerContractorCmb.SelectedItem == "Contractor" && BakerIDCmb.SelectedItem != null)
            {
                ExternalBusiness contractor = (ExternalBusiness)BakerIDCmb.SelectedItem;
                BakerDetailsTextBlock.Text = contractor.GetBusinessName() + ", " + contractor.getFirstName() + " " + contractor.getSurname();
                BakerOK = true;
                WartingBlock.Text = "";
            }
            else if (BakerContractorCmb.SelectedItem == "In-shop baker" && BakerIDCmb.SelectedItem != null)
            {
                Staff baker = (Staff)BakerIDCmb.SelectedItem;
                BakerDetailsTextBlock.Text = baker.getFirstName() + " " + baker.getSurname();
                BakerOK = true;
                WartingBlock.Text = "";
            }

            ValidateData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustIDCmb.SelectedItem;

            if (selectedCustomer.getPersonID() != customOrder.getCustomerID()) { customOrder.setCustomerID(selectedCustomer.getPersonID()); }

            if (BakerIDCmb.SelectedItem != null)
            {
                if (BakerContractorCmb.SelectedItem == "In-shop baker")
                {
                    Staff selectedBaker = (Staff)BakerIDCmb.SelectedItem;
                    if (selectedBaker.getPersonID() != customOrder.getBakerID() || customOrder.getOrderOutsourced())
                    {
                        if (customOrder.getOrderOutsourced())
                        {
                            foreach (ExternalBusiness contractor in DB.GetListOfContractors())
                            {
                                if (customOrder.getBakerID() == contractor.getPersonID())
                                {
                                    contractor.removeOrderID(customOrder.getOrderID());
                                }
                            }
                        }
                        else
                        {
                            foreach (Staff staff in DB.GetListOfStaffMembers())
                            {
                                if (customOrder.getBakerID() == staff.getPersonID())
                                {
                                    staff.removeOrderID(customOrder.getOrderID());
                                }
                            }
                        }

                        customOrder.setBakerID(selectedBaker.getPersonID());
                        selectedBaker.addOrderID(customOrder.getOrderID());
                        customOrder.setOrderOutsourced(false);
                    }
                }

                else if (BakerContractorCmb.SelectedItem == "Contractor")
                {
                    ExternalBusiness selectedBaker = (ExternalBusiness)BakerIDCmb.SelectedItem;
                    if (selectedBaker.getPersonID() != customOrder.getBakerID() || !customOrder.getOrderOutsourced())
                    {
                        if (customOrder.getOrderOutsourced())
                        {
                            foreach (ExternalBusiness contractor in DB.GetListOfContractors())
                            {
                                if (customOrder.getBakerID() == contractor.getPersonID())
                                {
                                    contractor.removeOrderID(customOrder.getOrderID());
                                }
                            }
                        }
                        else
                        {
                            foreach (Staff staff in DB.GetListOfStaffMembers())
                            {
                                if (customOrder.getBakerID() == staff.getPersonID())
                                {
                                    staff.removeOrderID(customOrder.getOrderID());
                                }
                            }
                        }

                        customOrder.setBakerID(selectedBaker.getPersonID());
                        selectedBaker.addOrderID(customOrder.getOrderID());
                        customOrder.setOrderOutsourced(true);
                    }
                }
            }

            if (SizeCmb.SelectedItem.ToString() != customOrder.getOrderSize())
            {
                customOrder.setOrderSize(SizeCmb.SelectedItem.ToString());
            }

            if (DescriptionTextBox.Text != customOrder.getDescription())
            {
                customOrder.setDescription(DescriptionTextBox.Text);
            }

            try { if (Decimal.Parse(PriceTextBox.Text) != customOrder.getOrderPrice()) { customOrder.setOrderPrice(Decimal.Parse(PriceTextBox.Text)); } }
            catch (FormatException exception) { MessageBox.Show("Please only use numbers for price!"); }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DescriptionTextBox.Text == "")
            {
                DescriptionOK = false;
                WartingBlock.Text = "Please input description";
            }
            else { DescriptionOK = true; WartingBlock.Text = ""; }

            ValidateData();
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal d;
            if (PriceTextBox.Text == "") { PriceOK = false; WartingBlock.Text = "Please input price!"; }
            else if (decimal.TryParse(PriceTextBox.Text, out d)) { PriceOK = true; WartingBlock.Text = ""; } else { PriceOK = false; WartingBlock.Text = "Invalid price"; }
            ValidateData();
        }

        private void ReadyToCollectCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)PaidCheckbox.IsChecked)
            {
                ReadyCheckboxOK = true; WartingBlock.Text = "";
            }
            else { ReadyCheckboxOK = false; WartingBlock.Text = "Order cannot be ready to collect before it is paid off."; }

            ValidateData();
        }

        private void CollectedCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)PaidCheckbox.IsChecked && (bool)ReadyToCollectCheckbox.IsChecked)
            {
                CollectedCheckboxOK = true; WartingBlock.Text = "";
            }
            else { CollectedCheckboxOK = false; WartingBlock.Text = "Order cannot be collected before it is paid and ready."; }

            ValidateData();
        }

        private void PaidCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)ReadyToCollectCheckbox.IsChecked) { ReadyCheckboxOK = true; }
            if ((bool)ReadyToCollectCheckbox.IsChecked && (bool)CollectedCheckbox.IsChecked) { CollectedCheckboxOK = true; }

            ValidateData();
        }

        private void PaidCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!(bool)ReadyToCollectCheckbox.IsChecked && !(bool)CollectedCheckbox.IsChecked) { ReadyCheckboxOK = true; CollectedCheckboxOK = true; }
            if ((bool)ReadyToCollectCheckbox.IsChecked) { ReadyCheckboxOK = false; WartingBlock.Text = "Order cannot be ready before it is paid."; }
            if ((bool)CollectedCheckbox.IsChecked) { CollectedCheckboxOK = false; WartingBlock.Text = "Order cannot be collected before it is paid and ready."; }

            ValidateData();
        }

        private void ReadyToCollectCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((bool)CollectedCheckbox.IsChecked) { CollectedCheckboxOK = false; WartingBlock.Text = "Order cannot be collected before it is paid and ready."; }
            ReadyCheckboxOK = true;

            ValidateData();
        }

        private void CollectedCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CollectedCheckboxOK = true;

            ValidateData();
        }
    }
}
