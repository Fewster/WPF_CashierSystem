using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Windows.Forms;

namespace LOVEiT_BAKERY
{
    public class Database : IXmlSerializable
    {
        private List<Customer> ListOfCustomers;
        private List<Staff> ListOfStaffMembers;
        private List<ExternalBusiness> ListofSuppliers;
        private List<ExternalBusiness> ListOfContractors;
        private List<CustomOrder> ListOfCustomOrders;
        private List<Promo> ListOfPromotions;
        protected List<Product> ListOfProducts;
        private List<Item> ListOfItems;
        private List<Transaction> ListOfTransactions;
        private int OrdersPerBaker;
        private int OrdersPerContractor;
        private decimal ContractorPercentage;
        private Session CurrentSession;
        private Checkout currentTransaction;

        public Database()
        {
            ListOfItems = new List<Item>();
            ListOfProducts = new List<Product>();
            ListofSuppliers = new List<ExternalBusiness>();
            ListOfContractors = new List<ExternalBusiness>();
            ListOfCustomers = new List<Customer>();
            ListOfStaffMembers = new List<Staff>();
            ListOfCustomOrders = new List<CustomOrder>();
            ListOfPromotions = new List<Promo>();
            ListOfProducts = new List<Product>();
            ListOfTransactions = new List<Transaction>();
            currentTransaction = new Checkout();
            //OrdersPerBaker = 0;
            //OrdersPerContractor = 0;
            //ContractorPercentage = 0;

            //Product newProd = new Product(1, 1, "Test", "Test", 12, 12m, 12m, "Buns", 12);
            //ListOfProducts.Add(newProd);
        }

        public void AddProductsToPromotions()
        {
            foreach (Promo promo in ListOfPromotions)
            {
                if (promo.getProduct() == null)
                {
                    foreach (Product product in ListOfProducts)
                    {
                        if (promo.getProductID() == product.GetID())
                        {
                            promo.setProduct(product);
                        }
                    }
                }
            }
        }

        public List<Transaction> GetListOfTransactions()
        {
            return ListOfTransactions;
        }

        public void AddTransaction(Transaction transaction)
        {
            ListOfTransactions.Add(transaction);
        }

        public List<Item> GetListOfItems()
        {
            return ListOfItems;
        }
        public void AddItem(int SupplierID, string Name, string Units, double Quantity, decimal Cost, decimal Price, string Type,int min, bool IsProduct)
        {
            int ID = 0;
            if (IsProduct)
            {
                if (ListOfProducts.Count != 0)
                {
                    ID = ListOfProducts[ListOfProducts.Count - 1].GetID() + 1;
                }
                Product Product = new Product(ID, SupplierID, Name, Units, Quantity, Cost, Price,Type, min);
                ListOfProducts.Add(Product);
            }
            else
            {
                if (ListOfItems.Count != 0)
                {
                    ID = ListOfItems[ListOfItems.Count - 1].GetID() + 1;
                }
                Item Item = new Item(ID, SupplierID, Name, Units, Quantity, Cost, min);
                ListOfItems.Add(Item);
            }
        }
        public List<ExternalBusiness> GetSuppliers()
        {
            return ListofSuppliers;
        }

        public List<ExternalBusiness> GetListofSuppliers()
        {
            return ListofSuppliers;
        }

        public List<ExternalBusiness> GetListOfContractors()
        {
            return ListOfContractors;
        }

        public void RemovePromotion(Promo promotion)
        {
            ListOfPromotions.Remove(promotion);
        }

        public void AddExternalBusiness(string FirstName, string Surname, string Address, string Number, string Email, string BusinessName, bool IsBaker)
        {
            int ID = 0;
            if (IsBaker)
            {
                if (ListOfContractors.Count != 0)
                {
                    ID = ListOfContractors[ListOfContractors.Count - 1].getPersonID() + 1;
                }

                ExternalBusiness Contractor = new ExternalBusiness(ID, FirstName, Surname, Address, Number, Email, BusinessName, IsBaker);
                ListOfContractors.Add(Contractor);
            }
            else
            {
                if (ListofSuppliers.Count != 0)
                {
                    ID = ListofSuppliers[ListofSuppliers.Count-1].getPersonID() + 1;
                }
                ExternalBusiness Supplier = new ExternalBusiness(ID, FirstName, Surname, Address, Number, Email, BusinessName, IsBaker);
                ListofSuppliers.Add(Supplier);
            }


        }
        public List<Customer> GetListOfCustomers()
        {
            return ListOfCustomers;
        }

        public void AddCustomer(Customer customer)
        {
            ListOfCustomers.Add(customer);
        }

        public List<Staff> GetListOfStaffMembers()
        {
            return ListOfStaffMembers;
        }

        public void AddStaffMember(Staff staff)
        {
            ListOfStaffMembers.Add(staff);
        }

        public List<CustomOrder> GetListOfCustomOrders()
        {
            return ListOfCustomOrders;
        }

        public void AddCustomOrder(CustomOrder customOrder)
        {
            ListOfCustomOrders.Add(customOrder);
        }

        public List<Promo> GetListOfPromotions()
        {
            return ListOfPromotions;
        }
        public bool DeleteItem(int ID)
        {
            foreach (Item Cand in ListOfItems)
            {
                if (Cand.GetID() == ID)
                {
                    ListOfItems.Remove(Cand);
                    return true;

                }
            }
            return false;
        }
        public bool DeleteProduct(int ID)
        {
            foreach (Product Cand in ListOfProducts)
            {
                if (Cand.GetID() == ID)
                {
                    ListOfProducts.Remove(Cand);
                    return true;

                }
            }
            return false;
        }
        public void AddPromotion(Promo promo)
        {
            ListOfPromotions.Add(promo);
        }

        public List<Product> GetListOfProducts()
        {
            //foreach(Product Product in ListOfProducts)
            //{
            //    MessageBox.Show(Product.ToString());
            //}
            
            return ListOfProducts;
            
        }

        public void AddProduct(Product product)
        {
            ListOfProducts.Add(product);
        }

        public int GetOrdersPerBaker()
        {
            return OrdersPerBaker;
        }

        public void SetOrdersPerBaker(int pOPB)
        {
            OrdersPerBaker = pOPB;
        }

        public int GetOrdersPerContractor()
        {
            return OrdersPerContractor;
        }

        public void SetOrdersPerContractor(int pOPC)
        {
            OrdersPerContractor = pOPC;
        }

        public decimal GetContractorPercentage()
        {
            return ContractorPercentage;
        }

        public void SetContractorPercentage(decimal pContractorPercentage)
        {
            ContractorPercentage = pContractorPercentage;
        }

        public void AddItemToCheckout(Object Item)
        {
            try
            {
                CustomOrder CompareOrder = new CustomOrder(false);
                if (Item.GetType() == CompareOrder.GetType())
                {
                    currentTransaction.AddOrder(Item as CustomOrder);
                }
                else
                {
                    currentTransaction.AddProduct(Item as Product);
                }
            }
            catch { }
        }

        public void RemoveItemFromCheckout(Object Item)
        {
            try
            {
                CustomOrder CompareOrder = new CustomOrder(false);
                if (Item.GetType() == CompareOrder.GetType())
                {
                    currentTransaction.RemoveOrder(Item as CustomOrder);
                }
                else
                {
                    currentTransaction.RemoveProduct(((KeyValuePair<Product, int>)Item));
                }
            }
            catch { }
        }

        public void RemoveItemFromCheckoutCompletely(Object Item)
        {
            try
            {
                currentTransaction.RemoveProductCompletely(((KeyValuePair<Product, int>)Item));
            }
            catch { }
        }

        public Checkout GetCurrentCheckout()
        {
            return currentTransaction;
        }

        public void ClearCheckout()
        {
            currentTransaction = new Checkout();
        }

        public Session GetCurrentSession()
        {
            return CurrentSession;
        }


        public void SetCurrentSession(Session session)
        {
            CurrentSession = session;
        }

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.MoveToContent();
                bool isEmptyElement = reader.IsEmptyElement;
                reader.ReadStartElement();
                if (!isEmptyElement)
                {
                    OrdersPerBaker = reader.ReadElementContentAsInt("OrdersPerBaker", "");
                    OrdersPerContractor = reader.ReadElementContentAsInt("OrdersPerContractor", "");
                    ContractorPercentage = reader.ReadElementContentAsInt("ContractorPercentage", "");

                    var accountSerializer = new XmlSerializer(typeof(Staff));
                    reader.ReadToFollowing("Staff");
                    while (!reader.EOF && reader.Name == "Staff")
                    {
                        Staff staff = (Staff)accountSerializer.Deserialize(reader);
                        ListOfStaffMembers.Add(staff);
                    }
                    reader.ReadEndElement();

                    accountSerializer = new XmlSerializer(typeof(Customer));
                    reader.ReadToFollowing("Customer");
                    while (!reader.EOF && reader.Name == "Customer")
                    {
                        Customer customer = (Customer)accountSerializer.Deserialize(reader);
                        ListOfCustomers.Add(customer);
                    }
                    reader.ReadEndElement();

                    accountSerializer = new XmlSerializer(typeof(CustomOrder));
                    reader.ReadToFollowing("CustomOrder");
                    while (!reader.EOF && reader.Name == "CustomOrder")
                    {
                        CustomOrder customOrder = (CustomOrder)accountSerializer.Deserialize(reader);
                        ListOfCustomOrders.Add(customOrder);
                    }
                    reader.ReadEndElement();

                    /*accountSerializer = new XmlSerializer(typeof(PromotionPercentile));
                    reader.ReadToFollowing("PromotionPercentile");
                    while (!reader.EOF && reader.Name == "PromotionPercentile")
                    {
                        PromotionPercentile promo = (PromotionPercentile)accountSerializer.Deserialize(reader);
                        ListOfPromotions.Add(promo);
                    }
                    reader.ReadEndElement();

                    accountSerializer = new XmlSerializer(typeof(PromotionBGF));
                    reader.ReadToFollowing("PromotionBGF");
                    while (!reader.EOF && reader.Name == "PromotionBGF")
                    {
                        PromotionBGF promo = (PromotionBGF)accountSerializer.Deserialize(reader);
                        ListOfPromotions.Add(promo);
                    }
                    reader.ReadEndElement();

                    accountSerializer = new XmlSerializer(typeof(PromotionSave));
                    reader.ReadToFollowing("PromotionSave");
                    while (!reader.EOF && reader.Name == "PromotionSave")
                    {
                        PromotionSave promo = (PromotionSave)accountSerializer.Deserialize(reader);
                        ListOfPromotions.Add(promo);
                    }
                    reader.ReadEndElement();*/

                    accountSerializer = new XmlSerializer(typeof(Item));
                    reader.ReadToFollowing("Item");
                    while (!reader.EOF && reader.Name == "Item")
                    {
                        Item Item = (Item)accountSerializer.Deserialize(reader);
                        ListOfItems.Add(Item);
                    }
                    reader.ReadEndElement();

                    accountSerializer = new XmlSerializer(typeof(Product));
                    reader.ReadToFollowing("Product");
                    while (!reader.EOF && reader.Name == "Product")
                    {
                        Product Product = (Product)accountSerializer.Deserialize(reader);
                        ListOfProducts.Add(Product);
                    }
                    reader.ReadEndElement();

                    /*accountSerializer = new XmlSerializer(typeof(ExternalBusiness));
                    reader.ReadToFollowing("ExternalBusiness");
                    while (!reader.EOF && reader.Name == "ExternalBusiness")
                    {
                        ExternalBusiness contractor = (ExternalBusiness)accountSerializer.Deserialize(reader);
                        ListOfContractors.Add(contractor);
                    }
                    reader.ReadEndElement();*/

                    /*accountSerializer = new XmlSerializer(typeof(ExternalBusiness));
                    reader.ReadToFollowing("ExternalBusiness");
                    while (!reader.EOF && reader.Name == "ExternalBusiness")
                    {
                        ExternalBusiness contractor = (ExternalBusiness)accountSerializer.Deserialize(reader);
                        ListOfContractors.Add(contractor);
                    }
                    reader.ReadEndElement();

                    accountSerializer = new XmlSerializer(typeof(ExternalBusiness));
                    reader.ReadToFollowing("ExternalBusiness");
                    while (!reader.EOF && reader.Name == "ExternalBusiness")
                    {
                        ExternalBusiness supplier = (ExternalBusiness)accountSerializer.Deserialize(reader);
                        ListofSuppliers.Add(supplier);
                    }
                    reader.ReadEndElement();*/
                }
                reader.Close();
            } catch { }

        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("OrdersPerBaker", OrdersPerBaker.ToString());
            writer.WriteElementString("OrdersPerContractor", OrdersPerContractor.ToString());
            writer.WriteElementString("ContractorPercentage", ContractorPercentage.ToString());

            writer.WriteStartElement("StaffMembers");
            foreach (Staff staff in ListOfStaffMembers)
            {
                var accountSerializer = new XmlSerializer(typeof(Staff));
                accountSerializer.Serialize(writer, staff);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Customers");
            foreach (Customer customer in ListOfCustomers)
            {
                var accountSerializer = new XmlSerializer(typeof(Customer));
                accountSerializer.Serialize(writer, customer);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("CustomOrders");
            foreach (CustomOrder customOrder in ListOfCustomOrders)
            {
                var accountSerializer = new XmlSerializer(typeof(CustomOrder));
                accountSerializer.Serialize(writer, customOrder);
            }
            writer.WriteEndElement();

            SortPromotions();
            writer.WriteStartElement("Promotions");
            foreach (Promo promotion in ListOfPromotions)
            {
                if (promotion.GetType() == typeof(PromotionPercentile))
                {
                    var accountSerializer = new XmlSerializer(typeof(PromotionPercentile));
                    accountSerializer.Serialize(writer, promotion);
                }

                else if (promotion.GetType() == typeof(PromotionBGF))
                {
                    var accountSerializer = new XmlSerializer(typeof(PromotionBGF));
                    accountSerializer.Serialize(writer, promotion);
                }

                else if (promotion.GetType() == typeof(PromotionSave))
                {
                    var accountSerializer = new XmlSerializer(typeof(PromotionSave));
                    accountSerializer.Serialize(writer, promotion);
                }
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Stock");
            /*writer.WriteStartElement("Suppliers");
            foreach (ExternalBusiness Supplier in ListofSuppliers)
            {
                var accountserializer = new XmlSerializer(typeof(ExternalBusiness));
                accountserializer.Serialize(writer, Supplier);
            }
            writer.WriteEndElement();*/

            writer.WriteStartElement("Ingredients");
            foreach (Item Item in ListOfItems)
            {
                var accountSerializer = new XmlSerializer(typeof(Item));
                accountSerializer.Serialize(writer, Item);
            }
            writer.WriteEndElement();
            writer.WriteStartElement("Products");
            foreach (Product product in ListOfProducts)
            {
                var accountSerializer = new XmlSerializer(typeof(Product));
                accountSerializer.Serialize(writer, product);
            }
            writer.WriteEndElement();
            writer.WriteEndElement();

            /*writer.WriteStartElement("Contractors");
            foreach (ExternalBusiness contractor in ListOfContractors)
            {
                var accountSerializer = new XmlSerializer(typeof(ExternalBusiness));
                accountSerializer.Serialize(writer, contractor);
            }
            writer.WriteEndElement();*/

        }

        public void SortPromotions()
        {
            List<Promo> SortedList = new List<Promo>();

            foreach (Promo promo in ListOfPromotions)
            {
                if (promo.GetType() == typeof(PromotionPercentile))
                {
                    SortedList.Add(promo);
                }
            }

            foreach (Promo promo in ListOfPromotions)
            {
                if (promo.GetType() == typeof(PromotionBGF))
                {
                    SortedList.Add(promo);
                }
            }

            foreach (Promo promo in ListOfPromotions)
            {
                if (promo.GetType() == typeof(PromotionSave))
                {
                    SortedList.Add(promo);
                }
            }

            ListOfPromotions = SortedList;
        }
    }
}
