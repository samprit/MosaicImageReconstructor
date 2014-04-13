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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OS11.ViewModel;
using OS11.Model;

namespace OS11.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePageView : UserControl
    {
        Logger logger = new Logger();
        public HomePageView()
        {
            InitializeComponent();            

            string mosaiqPath;
            try
            {
                mosaiqPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MosaiQ";
                if (!Directory.Exists(mosaiqPath))
                {
                    Directory.CreateDirectory(mosaiqPath);
                }

                mosaiqPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MosaiQ\\Config";
                if (!Directory.Exists(mosaiqPath))
                {
                    Directory.CreateDirectory(mosaiqPath);
                }

                logger.Log("[INFO] Mosaiq Directory created");
            }
            catch (System.PlatformNotSupportedException ex)
            {
                logger.Log("[ERROR] System.PlatformNotSupportedException : " + ex.Message);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                logger.Log("[ERROR] System.IO.DirectoryNotFoundException : " + ex.Message);
            }
            catch (System.IO.PathTooLongException ex)
            {
                logger.Log("[ERROR] System.IO.PathTooLongException : " + ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] System.UnauthorizedAccessException : "+ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : " + ex.Message);
            }
        }

        private void NewReconstructionBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			 Messenger.Default.Send<string, MainViewModel>("homePage");
        }

        private void OpenSampleBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FolderBrowser dlg = new FolderBrowser();
                string userprofile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MosaiQ\\Samples";
                //MessageBox.Show(userprofile);
                dlg.InitialDirectory = userprofile;
                dlg.ShowDialog();

                string filepath = dlg.SelectedPath;
                if (filepath != "")
                {
                    Messenger.Default.Send<string>(filepath + "$homePage");
                }

                logger.Log("[INFO] OpenSampleBtn_Click executed");
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                logger.Log("[ERROR] DirectoryNotFoundException : "+ex.Message);
            }
            catch (System.IO.PathTooLongException ex)
            {
                logger.Log("[ERROR] PathTooLongException : "+ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] UnauthorizedAccessException : "+ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : "+ex.Message);
            }
        }

        private void OpenExisting_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FolderBrowser dlg = new FolderBrowser();
                string userprofile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MosaiQ";
                //MessageBox.Show(userprofile);
                dlg.InitialDirectory = userprofile;
                dlg.ShowDialog();

                string filepath = dlg.SelectedPath;
                if (filepath != "")
                {
                    Messenger.Default.Send<string>(filepath + "$homePage");
                }

                logger.Log("[INFO] OpenSampleBtn_Click executed");
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                logger.Log("[ERROR] DirectoryNotFoundException : " + ex.Message);
            }
            catch (System.IO.PathTooLongException ex)
            {
                logger.Log("[ERROR] PathTooLongException : " + ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] UnauthorizedAccessException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : " + ex.Message);
            }
        }

        private void HelpBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            logger.Log("[INFO] Help Button Clicked");
            Window HelpWin = new HelpPage();
            HelpWin.ShowDialog();    	
        }
    }
}
