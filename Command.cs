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

namespace LOVEiT_BAKERY
{
    public static class Command
    {
         public static readonly RoutedUICommand Delete = new RoutedUICommand
       (
         "Delete",
         "Delete",
          typeof(StockManagement),
          new InputGestureCollection()
            {
                new KeyGesture(Key.Delete)
            }
            );
        public static readonly RoutedUICommand New = new RoutedUICommand
       (
         "Add New",
         "Add New",
          typeof(StockManagement),
          new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
            );

    }
}
