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
    /// Interaction logic for AdministrativeView.xaml
    /// </summary>
    public partial class AdministrativeView : UserControl
    {
        private Database DB;
        List<string> ListOfFemaleNames;
        List<string> ListOfMaleNames;
        List<string> ListOfSurnames;
        List<string> ListOfEmailEndings;
        List<string> ListOfStreetNames;
        List<string> ListOfLetters;

        public AdministrativeView(Database data)
        {
            DB = data;
            InitializeComponent();
            ListOfFemaleNames = new List<string>();
            ListOfMaleNames = new List<string>();
            ListOfSurnames = new List<string>();
            ListOfEmailEndings = new List<string>();
            ListOfStreetNames = new List<string>();
            ListOfLetters = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string filename = "RandomData.xml";
            XmlDocument inputFile = new XmlDocument();
            inputFile.Load(filename);
            LoadAllRandomData(inputFile);
            UpdateEmployeesView();
            UpdateContractorsView();
            OrdersPerBakerTextBox.Text = DB.GetOrdersPerBaker().ToString();
            OrdersPerContractorTextBox.Text = DB.GetOrdersPerContractor().ToString();
            ContractorsPercentageTextBox.Text = DB.GetContractorPercentage().ToString();
            EditEmployee.IsEnabled = false;
            RemoveEmployee.IsEnabled = false;
            EditContractor.IsEnabled = false;
            RemoveContractor.IsEnabled = false;
        }

        public void UpdateEmployeesView()
        {
            EmployeesView.ClearValue(ListView.ItemsSourceProperty);
            EmployeesView.ItemsSource = DB.GetListOfStaffMembers();
        }

        public void UpdateContractorsView()
        {
            ContractorsView.ClearValue(ListView.ItemsSourceProperty);
            ContractorsView.ItemsSource = DB.GetListOfContractors();
        }

        public void LoadAllRandomData(XmlDocument inputFile)
        {
            XmlNode femaleNames = inputFile.FirstChild.FirstChild;
            XmlNode maleNames = inputFile.FirstChild.FirstChild.NextSibling;
            XmlNode surnames = inputFile.FirstChild.FirstChild.NextSibling.NextSibling;
            XmlNode emailEndings = inputFile.FirstChild.FirstChild.NextSibling.NextSibling.NextSibling;
            XmlNode streetNames = inputFile.FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling;

            XmlNodeList femaleNamesList = femaleNames.ChildNodes;
            ListOfFemaleNames = LoadSpecificData("FemaleName", femaleNamesList);

            XmlNodeList maleNamesList = maleNames.ChildNodes;
            ListOfMaleNames = LoadSpecificData("MaleName", maleNamesList);

            XmlNodeList surnamesList = surnames.ChildNodes;
            ListOfSurnames = LoadSpecificData("Surname", surnamesList);

            XmlNodeList emailEndingsList = emailEndings.ChildNodes;
            ListOfEmailEndings = LoadSpecificData("EmailEnding", emailEndingsList);

            XmlNodeList streetNamesList = streetNames.ChildNodes;
            ListOfStreetNames = LoadSpecificData("StreetName", streetNamesList);
        }

        public List<string> LoadSpecificData(string searchFor, XmlNodeList inList)
        {
            List<string> populatedList = new List<string>();

            for (int i = 0; i < inList.Count; i++)
            {
                XmlNode currentNode = inList[i];

                if (currentNode.Name == searchFor)
                {
                    string newItem = currentNode.InnerText;
                    populatedList.Add(newItem);
                }
            }

            return populatedList;
        }

        public void GenerateNewPerson(string type)
        {
            Random random = new Random();
            int gender = random.Next(0, 2);
            string FirstName = "";
            string Surname = "";
            string Address = "";
            string PhoneNumber = "";
            string Email = "";

            if (gender == 0)
            {
                FirstName = ListOfFemaleNames[random.Next(0, ListOfFemaleNames.Count)];
            }
            else if (gender == 1)
            {
                FirstName = ListOfMaleNames[random.Next(0, ListOfMaleNames.Count)];
            }

            Surname = ListOfSurnames[random.Next(0, ListOfSurnames.Count)];

            string postcode = "";
            for (int i = 0; i < 8; i++)
            {
                string c = "";
                if (i < 2 || i > 5) { c = ListOfLetters[random.Next(0, ListOfLetters.Count)]; }
                else if (i == 3) { c = " "; }
                else { c = random.Next(0, 10).ToString(); }
                postcode = postcode + c;
            }
            Address = random.Next(1, 201).ToString() + " " + ListOfStreetNames[random.Next(0, ListOfStreetNames.Count)] + ", " + postcode;
            Email = FirstName.ToLower() + "." + Surname.ToLower() + ListOfEmailEndings[random.Next(0, ListOfEmailEndings.Count)];

            if (type == "Customer")
            {
                int ID = 0;
                if (DB.GetListOfCustomers().Count != 0) { ID = DB.GetListOfCustomers()[DB.GetListOfCustomers().Count - 1].getPersonID() + 1; }
                Customer newCustomer = new Customer(ID, FirstName, Surname, Address, PhoneNumber, Email);
                DB.AddCustomer(newCustomer);
            }
            else if (type == "Staff")
            {
                int ID = 0;
                if (DB.GetListOfStaffMembers().Count != 0) { ID = DB.GetListOfStaffMembers()[DB.GetListOfStaffMembers().Count - 1].getPersonID() + 1; }
                int isBaker = random.Next(0, 2);
                bool baker = false;
                if (isBaker == 0) { baker = false; }
                else { baker = true; }

                Staff newStaff = new Staff(ID, FirstName, Surname, Address, PhoneNumber, Email, baker);
                DB.AddStaffMember(newStaff);

                UpdateLoginDatabase(newStaff);
            }
            else if (type == "Contractor")
            {
                DB.AddExternalBusiness(FirstName, Surname, Address, PhoneNumber, Email, "ExternalBusiness", true);
            }
        }

        public void GenerateNewCustomOrder()
        {
            Random random = new Random();
            int orderID = 0;
            int custID = 0;

            if (DB.GetListOfCustomOrders().Count != 0) { orderID = DB.GetListOfCustomOrders()[DB.GetListOfCustomOrders().Count - 1].getOrderID() + 1; }
            if (DB.GetListOfCustomers().Count != 0) { custID = DB.GetListOfCustomers()[DB.GetListOfCustomers().Count - 1].getPersonID(); }
            int outsourced = random.Next(0, 2);
            bool orderOutsourced = false;
            if (outsourced == 0) { orderOutsourced = true; }
            int orderSize = random.Next(0, 3);
            string Size = "";
            if (orderSize == 0) { Size = "Small"; } else if (orderSize == 1) { Size = "Medium"; } else { Size = "Large"; }
            string description = "Description";
            decimal price = random.Next(10, 50);

            CustomOrder newCustOrder = new CustomOrder(orderID, custID, Size, orderOutsourced, description, price);
            if (!newCustOrder.AssignBaker(DB)) { newCustOrder.setBakerID(-1); }

            if (orderOutsourced)
            {
                foreach (ExternalBusiness contractor in DB.GetListOfContractors())
                {
                    if (newCustOrder.getBakerID() == contractor.getPersonID())
                    { contractor.addOrderID(newCustOrder.getOrderID()); }
                }
            }
            else
            {
                foreach (Staff staff in DB.GetListOfStaffMembers())
                {
                    if (newCustOrder.getBakerID() == staff.getPersonID())
                    { staff.addOrderID(newCustOrder.getOrderID()); }
                }
            }

            DB.AddCustomOrder(newCustOrder);
        }

        public void UpdateLoginDatabase(Staff newStaff)
        {
            LoginDatabase loginDB = new LoginDatabase();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoginDatabase));
            using (StreamReader reader = new StreamReader("loginDetails.xml"))
            {
                loginDB = (LoginDatabase)xmlSerializer.Deserialize(reader);
            }

            LoginDatabase newLoginDB = new LoginDatabase();

            if (loginDB.getExistingUsers().Count != 0 && loginDB.getExistingUsers()[0].getUsername() == "admin")
            {
                newLoginDB.AddUser(loginDB.getExistingUsers()[0]);
            }
            else
            {
                newLoginDB.createAdminAccount();
            }

            foreach (Staff staff in DB.GetListOfStaffMembers())
            {
                bool staffAdded = false;
                foreach (Session user in loginDB.getExistingUsers())
                {
                    if (staff.getPersonID() == user.getUserID() && !staffAdded)
                    {
                        string username = staff.getFirstName() + "." + staff.getSurname() + staff.getPersonID().ToString();
                        if (username != user.getUsername())
                        {
                            user.setUsername(username);
                            user.changePassword("password");
                        }
                        newLoginDB.AddUser(user);
                        staffAdded = true;
                    }
                }
            }

            Session newUser = new Session(newStaff);
            newLoginDB.AddUser(newUser);

            using (TextWriter writer = new StreamWriter("loginDetails.xml"))
            {
                xmlSerializer.Serialize(writer, newLoginDB);
            }
        }

        private void RandomEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewPerson("Staff");
            UpdateEmployeesView();
        }

        private void RandomCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewPerson("Customer");
        }

        private void RandomContractorButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewPerson("Contractor");
            UpdateContractorsView();
        }

        private void WipeDataButton_Click(object sender, RoutedEventArgs e)
        {
            DB = new Database();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Database));
            using (TextWriter writer = new StreamWriter("outputTest.xml"))
            {
                xmlSerializer.Serialize(writer, DB);
            }

            LoginDatabase loginDB = new LoginDatabase();
            loginDB.createAdminAccount();

            xmlSerializer = new XmlSerializer(typeof(LoginDatabase));
            using (TextWriter writer = new StreamWriter("loginDetails.xml"))
            {
                xmlSerializer.Serialize(writer, loginDB);
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddingNewEmployee window = new AddingNewEmployee(DB);
            window.ShowDialog();

            if (window.DialogResult != null)
            {
                if ((bool)window.DialogResult)
                {
                    UpdateEmployeesView();
                }
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Staff staff = (Staff)EmployeesView.SelectedItem;

            if (staff != null)
            {
                EditEmployee window = new EditEmployee(staff);
                window.ShowDialog();

                if (window.DialogResult != null)
                {
                    if ((bool)window.DialogResult)
                    {
                        UpdateEmployeesView();
                    }
                }
            }
        }

        private void RemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            Staff staff = (Staff)EmployeesView.SelectedItem;

            if (staff != null)
            {
                if (staff.getOrderIDsCount() != 0) { MessageBox.Show("Please reassign their orders to other bakers!"); }
                else if (MessageBox.Show("Are you sure you want to delete this employee from the system?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DB.GetListOfStaffMembers().Remove(staff);
                    UpdateEmployeesView();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Content = new StockManagement(DB);
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new MainMenu(DB);
        }

        private void SaveAndLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Database));
            using (TextWriter writer = new StreamWriter("outputTest.xml"))
            {
                xmlSerializer.Serialize(writer, DB);
            }

            Content = new Login(DB);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            DB.SetOrdersPerBaker(Int32.Parse(OrdersPerBakerTextBox.Text));
            DB.SetOrdersPerContractor(Int32.Parse(OrdersPerContractorTextBox.Text));
            DB.SetContractorPercentage(Decimal.Parse(ContractorsPercentageTextBox.Text));
        }

        private void OrdersPerBakerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (!int.TryParse(OrdersPerBakerTextBox.Text, out number))
            {
                ApplyButton.IsEnabled = false;
                WarningsTextBlock.Text = "Input a number of orders per baker";
            }
            else
            {
                ApplyButton.IsEnabled = true;
                WarningsTextBlock.Text = "";
            }
        }

        private void OrdersPerContractorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (!int.TryParse(OrdersPerContractorTextBox.Text, out number))
            {
                ApplyButton.IsEnabled = false;
                WarningsTextBlock.Text = "Input a number of orders per contractor";
            }
            else
            {
                ApplyButton.IsEnabled = true;
                WarningsTextBlock.Text = "";
            }
        }

        private void ContractorsPercentageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (!int.TryParse(ContractorsPercentageTextBox.Text, out number))
            {
                ApplyButton.IsEnabled = false;
                WarningsTextBlock.Text = "Input a number for contractors' percentage cut";
            }
            else
            {
                ApplyButton.IsEnabled = true;
                WarningsTextBlock.Text = "";
            }
        }

        private void EmployeesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Staff staff = (Staff)EmployeesView.SelectedItem;

            if (staff != null)
            {
                EditEmployee.IsEnabled = true;
                RemoveEmployee.IsEnabled = true;
            }
        }

        private void ContractorsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExternalBusiness contractor = (ExternalBusiness)ContractorsView.SelectedItem;

            if (contractor != null)
            {
                EditContractor.IsEnabled = true;
                RemoveContractor.IsEnabled = true;
            }
        }

        private void AddContractor_Click(object sender, RoutedEventArgs e)
        {
            AddNewContractor window = new AddNewContractor(DB, false);
            window.ShowDialog();

            if (window.DialogResult != null)
            {
                if ((bool)window.DialogResult)
                {
                    UpdateContractorsView();
                }
            }
        }

        private void EditContractor_Click(object sender, RoutedEventArgs e)
        {
            ExternalBusiness contractor = (ExternalBusiness)ContractorsView.SelectedItem;

            if (contractor != null)
            {
                EditContractor window = new EditContractor(contractor);
                window.ShowDialog();

                if (window.DialogResult != null)
                {
                    if ((bool)window.DialogResult)
                    {
                        UpdateContractorsView();
                    }
                }
            }
        }

        private void RemoveContractor_Click(object sender, RoutedEventArgs e)
        {
            ExternalBusiness contractor = (ExternalBusiness)ContractorsView.SelectedItem;

            if (contractor != null)
            {
                if (contractor.getOrderIDsCount() != 0) { MessageBox.Show("Please reassign their orders to other bakers!"); }
                else if (MessageBox.Show("Are you sure you want to delete this contractor from the system?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DB.GetListOfContractors().Remove(contractor);
                    UpdateContractorsView();
                }
            }
        }

        private void RandomCumsomOrderButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewCustomOrder();
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            AddNewContractor window = new AddNewContractor(DB, true);
            window.ShowDialog();

            if (window.DialogResult != null)
            {
                if ((bool)window.DialogResult)
                {
                    UpdateContractorsView();
                }
            }
        }
    }
}
