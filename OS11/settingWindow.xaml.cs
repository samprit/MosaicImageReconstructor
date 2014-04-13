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
using System.Windows.Shapes;

namespace OS11
{
    /// <summary>
    /// Interaction logic for settingsWindow.xaml
    /// </summary>
    
    public partial class settingsWindow : Window
    {
        public settingsWindow()
        {
            InitializeComponent();

            if (OS11.Properties.Settings.Default.algorithm == 0)
                javaOption.IsChecked = true;
            else
                pythonOption.IsChecked = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (javaOption.IsChecked == true)
            {
                OS11.Properties.Settings.Default.algorithm = 0;
            }
            else
            {
                OS11.Properties.Settings.Default.algorithm = 1;
            }

            OS11.Properties.Settings.Default.Save();
            this.Close();
        }
    }
}