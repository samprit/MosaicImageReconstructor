using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace OS11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Logger logger = new Logger();
        public MainWindow()
        {
            try
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                InitializeComponent();
                logger.Log("[SUCCESS] MainWindow loaded");
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] " + ex.Message);
            }
        }
    }
}
