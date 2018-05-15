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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database DB;
        public MainWindow()
        {
            DB = new Database();

            try
            {
                if (File.Exists("outputTest.xml"))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Database));
                    using (StreamReader reader = new StreamReader("outputTest.xml"))
                    {
                        DB = (Database)xmlSerializer.Deserialize(reader);
                    }
                }
            } catch { }

            DB.AddProductsToPromotions();
            InitializeComponent();
            MainContent.Content = new Login(DB);
        }
    }
}
