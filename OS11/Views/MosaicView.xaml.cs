using System;
using sysdraw = System.Drawing;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using OS11.ViewModel;
using Microsoft.Expression.Media.Effects;
using System.Runtime.InteropServices;
using winform = System.Windows.Forms;
using Newtonsoft.Json;
using OS11.Model;

namespace OS11.Views
{
    /// <summary>
    /// Interaction logic for MosaicView.xaml
    /// </summary>
    public partial class MosaicView : System.Windows.Controls.UserControl
    {
        Logger logger = new Logger();
        struct ImgProperty
        {
            public double zoom;
            public double X, Y;

            public ImgProperty(double z, double x, double y)
            {
                zoom = z;
                X = x; Y = y;
            }
        };

        public BitmapSource imgCopy;
        private Point origin;
        private Point start;
        private Dictionary<Uri, ImgProperty> imgList;
        public bool notTornPieceGrid;
        public int doNotSelectImage;
        public string constrPathname;
        public string myDocsPath;
        public bool isProjectSavable;
        public int isConstructed;


        public MosaicView()
        {
            InitializeComponent();

            

            try
            {
                imgList = new Dictionary<Uri, ImgProperty>();

                myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                
                PythonAppConfig appConfig = new PythonAppConfig();
                appConfig.arcLengthMultiplier = "10";
                appConfig.noiseReduction = "1";
                appConfig.scaleFactor = "1000";
                appConfig.approxPolyparameter = "0.07";
                appConfig.multiEdge = "1";
                appConfig.backgroundThreshold = "2";
                string result = JsonConvert.SerializeObject(appConfig);
                //MessageBox.Show("Edited to:\n" + result);
                System.IO.File.WriteAllText(myDocsPath + "\\MosaiQ\\Config\\config.json", result);

                constrPathname = "";
                doNotSelectImage = 0;
                isProjectSavable = false;
                zoom.Value = 1;
                //tornListView.Background = Brushes.Azure;
                TransformGroup group = new TransformGroup();
                ScaleTransform xform = new ScaleTransform();
                group.Children.Add(xform);

                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);

                OutputImage.RenderTransform = group;

                //leftRotate.Visibility = Visibility.Hidden;
                //rightRotate.Visibility = Visibility.Hidden;
                //zoom.Visibility = Visibility.Hidden;

                notTornPieceGrid = true;
                isConstructed = 0;

                //READ Json Config for Python
                readJsonFromConfigFile();


                if (OS11.ViewModel.MainViewModel.filepath != "")
                {
                    loadSamples(MainViewModel.filepath);
                    isConstructed = 1;
                }
                Mouse.SetCursor(Cursors.Arrow);
                //constructButton.Visibility = Visibility.Hidden;

                AddSampleBtn.Click += new RoutedEventHandler(AddSampleButton_Click);
                //AddBtn.Click += new RoutedEventHandler(AddButton_Click);
                SaveBtn.Click += new RoutedEventHandler(SaveBtn_Click);


                InputImageListBox.AllowDrop = true;
                InputImageListBox.DragEnter += new DragEventHandler(tornListView_DragEnter);
                InputImageListBox.DragLeave += new DragEventHandler(tornListView_DragLeave);
                InputImageListBox.Drop += new DragEventHandler(tornListView_Drop);
                watcher.Changed += new FileSystemEventHandler(OnFileChanged);

                CenterRightGrid.AllowDrop = true;
                CenterRightGrid.DragEnter += new DragEventHandler(CenterRightGrid_DragEnter);
                CenterRightGrid.DragLeave += new DragEventHandler(CenterRightGrid_DragLeave);
                CenterRightGrid.Drop += new DragEventHandler(tornListView_Drop);


                isProjectSavable = true;
                logger.Log("[INFO] MosaicView Initialized");
            }
            catch (System.PlatformNotSupportedException ex)
            {
                logger.Log("[ERROR] System.PlatformNotSupportedException : " + ex.Message);
            }
            catch (System.ArgumentException ex)
            {
                logger.Log("[ERROR] System.ArgumentException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void addConstructedImage(string pathname)
        {
            try
            {   
                logger.Log("[SUCCESS] Image OK");
                BitmapImage temp = new BitmapImage();
                temp.BeginInit();
                temp.UriSource = new Uri(pathname);
                temp.CreateOptions  = BitmapCreateOptions.IgnoreImageCache;
                temp.CacheOption = BitmapCacheOption.OnLoad;
                temp.EndInit();
                OutputImage.Tag = pathname;

                OutputImage.Source = null;
                OutputImage.Source = temp;
                
                if (!imgList.ContainsKey(new Uri(pathname)))
                {
                    ImgProperty imgproperty = new ImgProperty(1, 0, 0);
                    //imgList.Add(new Uri(pathname), imgproperty);
                    zoom.Value = 1;
                    TransformGroup transformGroup = (TransformGroup)OutputImage.RenderTransform;
                    ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];
                    transform.ScaleX = zoom.Value;
                    transform.ScaleY = zoom.Value;
                }
                else
                {
                    zoom.Value = 1;
                    TransformGroup transformGroup = (TransformGroup)OutputImage.RenderTransform;
                    ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];
                    transform.ScaleX = zoom.Value;
                    transform.ScaleY = zoom.Value;
                }

                var tt = (TranslateTransform)((TransformGroup)OutputImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
                tt.X = imgList[new Uri(pathname)].X;
                tt.Y = imgList[new Uri(pathname)].Y;
                imgCopy = (BitmapSource)OutputImage.Source;
                //leftRotate.Visibility = Visibility.Visible;
                //rightRotate.Visibility = Visibility.Visible;
                //zoom.Visibility = Visibility.Visible;
                constrPathname = pathname;
                logger.Log("[SUCCESS] Constructed Image Added");
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] ArgumentNullException : " + ex.Message);
                imgList.Remove(new Uri(pathname));
                return;
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isConstructed == 1)
                {
                    MessageBox.Show("Cannot Add Image now! Please Start a new Project", "Mosaiq", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.Multiselect = true;
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".jpg"; // Default file extension
                dlg.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png)|*.jpg; *.jpeg; *.bmp; *.png"; // Filter files by extension 

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results 
                if (result == true)
                {
                    Mouse.SetCursor(Cursors.Wait);
                    List<string> pathnames = dlg.FileNames.ToList<string>();
                    addImagesToList(pathnames);
                }
                Mouse.SetCursor(Cursors.Arrow);
                logger.Log("[INFO] Add Button Clicked");
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void addImagesToList(List<string> pathnames)
        {
            try
            {
                List<string> finalPathnames = new List<string>();
                List<string> errorList = new List<string>();
                foreach (string pathname in pathnames)
                {
                    if ((!pathname.EndsWith("jpg")) && (!pathname.EndsWith("jpeg")) && (!pathname.EndsWith("bmp") && (!pathname.EndsWith("png"))))
                    {
                        errorList.Add("The given file (\"" + pathname +
                            "\") format is not supported. Please select one of the following ['jpg', 'jpeg', 'png', 'bmp']");
                        continue;
                    }
                    if (imgList.ContainsKey(new Uri(pathname)) == true)
                    {
                        errorList.Add("The given file (\"" + pathname + "\") already exists in the process queue");
                        continue;
                    }
                    else
                    {
                        try
                        {
                            BitmapImage ba = new BitmapImage(new Uri(pathname));
                            logger.Log("[SUCCESS] Image OK");
                        }
                        catch (Exception ex)
                        {
                            logger.Log("[ERROR] Exception " + ex.Message);
                            errorList.Add("The given file (\"" + pathname + "\") cannot be opened");
                            continue;
                        }
                    }
                    ImgProperty imgproperty = new ImgProperty(1, 0, 0);
                    imgList.Add(new Uri(pathname), imgproperty);
                    finalPathnames.Add(pathname);

                }
                if (finalPathnames.Count > 0)
                {
                    Messenger.Default.Send<List<string>, MosaicViewModel>(finalPathnames);
                }
                else
                {
                    errorList.Add("No compatible files found. Please try again.");
                }
                if (errorList.Count > 0)
                {
                    string totalErrors = "MosaiQ found following errors : \n";
                    int i = 1;
                    foreach (string error in errorList)
                    {
                        totalErrors += "\n" + i + ".) " + error + "\n";
                        i++;
                    }
                    MessageBox.Show(totalErrors, "Error - Mosaiq",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //isProjectSavable = true;
                //saveProjectDialog();
                logger.Log("[SUCCESS] Images Added To List");
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] ArgumentNullException : " + ex.Message);
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void saveProjectDialog()
        {
            try
            {
                if (!isProjectSavable)
                {
                    MessageBox.Show("Project cannot be saved right now.", "Mosaiq", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string initialSavePath = myDocsPath + "\\MosaiQ\\Projects";
                if (!Directory.Exists(initialSavePath))
                {
                    Directory.CreateDirectory(initialSavePath);
                }

                List<String> DirPathnames = Directory.GetDirectories(initialSavePath).ToList<string>();
                Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
                saveDialog.FileName = "Project" + (DirPathnames.Count + 1).ToString();
                saveDialog.InitialDirectory = initialSavePath;
                //MessageBox.Show(saveFolder);
                Nullable<bool> result = saveDialog.ShowDialog();

                if (result == true)
                {
                    string savePath = saveDialog.FileName;
                    System.IO.Directory.CreateDirectory(savePath);

                    System.IO.Directory.CreateDirectory(savePath + "\\Input");

                    List<string> delim1 = new List<string>();
                    delim1.Add("\\");
                    delim1.Add(".png");
                    delim1.Add(".jpg");
                    delim1.Add(".jpeg");
                    delim1.Add(".bmp");

                    List<string> splitList1 = (saveDialog.FileName).Split(delim1.ToArray(), StringSplitOptions.None).ToList<string>();
                    string projName = splitList1.Last();
                    //System.IO.Directory.CreateDirectory(saveFolder + "\\" + last1);

                    foreach (KeyValuePair<Uri, ImgProperty> entry in imgList)
                    {
                        //if (entry.Key == constrPathname)
                        //    continue;

                        List<string> delim = new List<string>();
                        delim.Add("\\");
                        List<string> splitList = (entry.Key.ToString()).Split(delim.ToArray(), StringSplitOptions.None).ToList<string>();
                        string last = splitList.Last();
                        System.IO.File.Copy(entry.Key.ToString(), savePath + "\\Input\\" + last, true);
                    }

                    //System.IO.File.Copy(constrPathname, savePath + "\\MosaiQed\\Constructed Image.png", true);
                    createXMLFile(savePath, projName);
                    logger.Log("[SUCESS] Project Saved");
                }
                else
                {
                    //TODO Not Saved Handling
                }
            }
            catch (System.IO.IOException ex)
            {
                logger.Log("[ERROR] System.IO.IOException : " + ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] System.UnauthorizedAccessException : " + ex.Message);
            }
            catch (System.ArgumentException ex)
            {
                logger.Log("[ERROR] System.ArgumentException : " + ex.Message);
            }
            catch (System.NotSupportedException ex)
            {
                logger.Log("[ERROR] System.NotSupportedException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void createXMLFile(string pathname, string projectName)
        {
            try
            {
                string savePathXML = pathname;

                savePathXML += "\\" + projectName + ".msqp";

                XmlDocument xmlDoc = new XmlDocument();
                //creating new XML document
                XmlTextWriter xmlWriter = new XmlTextWriter(savePathXML, System.Text.Encoding.UTF8);
                //creating XmlTestWriter, and passing file name and encoding type as argument
                xmlWriter.Formatting = System.Xml.Formatting.Indented;
                //setting XmlWriter formating to be indented
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                //writing version and encoding type of XML in file.
                xmlWriter.WriteStartElement("ImageList");
                //writing first element
                xmlWriter.Close();
                //closing writer

                xmlDoc.Load(savePathXML);
                //loading XML file
                XmlNode root = xmlDoc.DocumentElement;
                //creating child nodes.
                XmlElement childNode1 = xmlDoc.CreateElement("Image1");
                XmlElement childNode2 = xmlDoc.CreateElement("Image2");
                XmlElement childNode3 = xmlDoc.CreateElement("Image3");
                //adding child node to root.
                root.AppendChild(childNode1);
                childNode1.InnerText = "image1";
                //assigning innertext of childnode to text of combobox.
                root.AppendChild(childNode2);
                childNode2.InnerText = "";
                root.AppendChild(childNode3);
                childNode3.InnerText = "";
                xmlDoc.Save(savePathXML);
                logger.Log("[SUCCESS] XML File Created");
            }
            catch (System.ArgumentException ex)
            {
                logger.Log("[ERROR] System.ArgumentException : " + ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] System.UnauthorizedAccessException : " + ex.Message);
            }
            catch (System.IO.IOException ex)
            {
                logger.Log("[ERROR] System.IO.IOException : " + ex.Message);
            }
            catch (System.Security.SecurityException ex)
            {
                logger.Log("[ERROR] System.Security.SecurityException : " + ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (System.Xml.XmlException ex)
            {
                logger.Log("[ERROR] System.Xml.XmlException : " + ex.Message);
            }
            catch (System.NotSupportedException ex)
            {
                logger.Log("[ERROR] System.NotSupportedException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void AddSampleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isConstructed == 0)
                {
                    MessageBox.Show("You need to reconstruct atleast once to add project to samples.", "Mosaiq", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (!(System.IO.Directory.Exists(myDocsPath + "\\MosaiQ\\Samples")))
                {
                    System.IO.Directory.CreateDirectory(myDocsPath + "\\MosaiQ\\Samples");
                }
                int numDirectories = (Directory.GetDirectories(myDocsPath + "\\MosaiQ\\Samples")).Count() + 1;

                string newSampleFolder = myDocsPath + "\\MosaiQ\\Samples\\Sample " + numDirectories;
                System.IO.Directory.CreateDirectory(newSampleFolder);
                System.IO.Directory.CreateDirectory(newSampleFolder + "\\torn pieces");

                foreach (KeyValuePair<Uri, ImgProperty> entry in imgList)
                {
                    if (entry.Key.ToString() == (new Uri(constrPathname)).ToString())
                    {
                        continue;
                    }
                    if (entry.Key.ToString() == constrPathname)
                        continue;

                    List<string> delim = new List<string>();
                    delim.Add("/");
                    List<string> splitList = (entry.Key.ToString()).Split(delim.ToArray(), StringSplitOptions.None).ToList<string>();
                    string last = splitList.Last();
                    //MessageBox.Show(ConvertUriToPathname(entry.Key));
                    System.IO.File.Copy(ConvertUriToPathname(entry.Key), newSampleFolder + "\\torn pieces\\" + last, true);
                }

                System.IO.File.Copy(constrPathname, newSampleFolder + "\\Constructed Image.png", true);
                createXMLFile(newSampleFolder, "Sample" + numDirectories);
                MessageBox.Show("Saved as Sample " + numDirectories);
                logger.Log("[SUCCESS] Added To Samples");
            }
            catch (System.IO.IOException ex)
            {
                logger.Log("[ERROR] System.IO.IOException : " + ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] System.UnauthorizedAccessException : " + ex.Message);
            }
            catch (System.ArgumentException ex)
            {
                logger.Log("[ERROR] System.ArgumentException : " + ex.Message);
            }
            catch (System.NotSupportedException ex)
            {
                logger.Log("[ERROR] System.NotSupportedException : " + ex.Message);
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void loadSamples(string pathname)
        {
            try
            {
                doNotSelectImage = 1;
                string filepathTorn = pathname + "//torn pieces";
                //string filepathConstr = pathname + "//MosaiQed";

                if (!Directory.Exists(filepathTorn))
                {
                    return;
                }

                Mouse.SetCursor(Cursors.Wait);
                //AddButton.Visibility = Visibility.Hidden;
                //AddFoldersButton.Visibility = Visibility.Hidden;
                //string[] pathnames = Directory.GetFiles(filepath, "*.*").Where(file => file.ToLower().EndsWith("aspx") || file.ToLower().EndsWith("ascx"));
                List<String> pathnames = Directory.GetFiles(filepathTorn, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("jpeg") || file.ToLower().EndsWith("bmp") || file.ToLower().EndsWith("png")).ToList();
                List<String> constrPathnames = Directory.GetFiles(pathname, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("jpeg") || file.ToLower().EndsWith("bmp") || file.ToLower().EndsWith("png")).ToList();
                addImagesToList(pathnames);
                addConstructedImage(constrPathnames[0]);
                logger.Log("[SUCCESS] Samples Loaded");
            }
            catch (System.IO.IOException ex)
            {
                logger.Log("[ERROR] System.IO.IOException : " + ex.Message);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] System.UnauthorizedAccessException : " + ex.Message);
            }
            catch (System.ArgumentException ex)
            {
                logger.Log("[ERROR] System.ArgumentException : " + ex.Message);
            }
            catch (System.NotSupportedException ex)
            {
                logger.Log("[ERROR] System.NotSupportedException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }


        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {

            ParametersGrid.Width = 250;

            try
            {
                Window settingsWin = new settingsWindow();
                settingsWin.ShowDialog();
                logger.Log("[INFO] Settings Button Clicked");
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            logger.Log("[INFO] Save Button Clicked");
            if (isConstructed == 0)
            {
                MessageBox.Show("You need to reconstruct atleast once to save the project.","Mosaiq", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            saveProjectDialog();
        }

        private void RemoveImgFromList(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (isConstructed == 1)
                {
                    return;
                }
                if (doNotSelectImage == 1)
                {
                    return;
                }
                notTornPieceGrid = false;
                Button b = sender as Button;

                string tempKey = "";
                bool flagRemove = false;
                foreach (KeyValuePair<Uri, ImgProperty> entry in imgList)
                {
                    Uri tempUri = entry.Key;
                    if (tempUri.ToString() == b.Tag.ToString())
                    {
                        tempKey = entry.Key.ToString();
                        flagRemove = true;
                        Messenger.Default.Send<Uri, MosaicViewModel>(tempUri);
                        break;
                    }
                }
                if (flagRemove)
                {
                    imgList.Remove(new Uri(tempKey));
                    string constrImageTag = (string)OutputImage.Tag;
                    Uri tempUri = new Uri(tempKey);
                    string imgStr = tempUri.ToString();
                    if (imgStr == constrImageTag)
                    {
                        OutputImage.Source = null;
                        imgCopy = null;
                        //zoom.Visibility = Visibility.Hidden;
                        //leftRotate.Visibility = Visibility.Hidden;
                        //rightRotate.Visibility = Visibility.Hidden;

                        zoom.Value = 1;
                        TransformGroup transformGroup = (TransformGroup)OutputImage.RenderTransform;
                        ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

                        double zom = zoom.Value;
                        transform.ScaleX = zom;
                        transform.ScaleY = zom;

                        OutputImage.Tag = "";
                    }
                }

                //if(constrImageTag==tagGridTag)
                //{
                //	constrImage.Tag = "";
                //}
                logger.Log("[INFO] Removed Image From List");
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void HelpBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            logger.Log("[INFO] Help Button Clicked");
			Window HelpWin = new HelpPage();
            HelpWin.ShowDialog();    	
        }

        private void TornImage_Selected(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show(sender.GetType().ToString());
            logger.Log("[INFO] Torn Image Selected");
            // TODO: Add event handler implementation here.
        }

        private void TornImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (isConstructed == 1)
                {
                    return;
                }
                if (doNotSelectImage == 1)
                {
                    return;
                }

                if (notTornPieceGrid == true)
                {
                    Button b = sender as Button;
                    addConstructedImage(b.Tag.ToString());

                    string pathname = b.Tag.ToString();
                    string imgPath = OutputImage.Tag.ToString();

                    if ((new Uri(imgPath)).ToString() != pathname)
                    {
                        //zoom.Value = 1;
                        addConstructedImage(pathname);
                        TransformGroup transformGroup = (TransformGroup)OutputImage.RenderTransform;
                        ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

                        //double zom = e.Delta > 0 ? .2 : -.2;

                        zoom.Value = imgList[new Uri(constrPathname)].zoom;
                        //System.Console.Write(zoom.Value + "\n");
                        double zom = zoom.Value;
                        transform.ScaleX = zom;
                        transform.ScaleY = zom;
                    }
                }
                else
                {
                    notTornPieceGrid = true;
                }
                logger.Log("[INFO] Torn Image Clicked");
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void rotateLeftBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (OutputImage.Source != null)
                {
                    BitmapSource temp = (BitmapSource)OutputImage.Source;

                    CachedBitmap cache = new CachedBitmap(temp, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    TransformedBitmap tb = new TransformedBitmap(cache, new RotateTransform(270));
                    OutputImage.Source = (BitmapSource)tb;
                    cache = null;
                    tb = null;

                    imgCopy = (BitmapSource)OutputImage.Source;
                    var tt = (TranslateTransform)((TransformGroup)OutputImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
                    tt.X = 0;
                    tt.Y = 0;
                    logger.Log("[INFO] Left Rotate Button Clicked");
                }
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void rotateRightBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (OutputImage.Source != null)
                {
                    BitmapSource temp = (BitmapSource)OutputImage.Source;

                    CachedBitmap cache = new CachedBitmap(temp, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    TransformedBitmap tb = new TransformedBitmap(cache, new RotateTransform(90));
                    OutputImage.Source = (BitmapSource)tb;
                    cache = null;
                    tb = null;

                    imgCopy = (BitmapSource)OutputImage.Source;
                    var tt = (TranslateTransform)((TransformGroup)OutputImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
                    tt.X = 0;
                    tt.Y = 0;
                    logger.Log("[INFO] Right Rotate Button Clicked");
                }
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void tornListView_Drop(object sender, System.Windows.DragEventArgs e)
        {
            try
            {
                if (isConstructed == 1)
                {
                    return;
                }
                //MessageBox.Show("yes");
                if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
                {
                    BrushConverter bc = new BrushConverter();
                    CenterRightGrid.Background = ( System.Windows.Media.Brush)bc.ConvertFrom("#FFFDFDFD");
                    InputImageListBox.Background = Brushes.White;

                    string[] pathnames = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                    addImagesToList(pathnames.ToList<string>());
                    logger.Log("[INFO] File Dropped");
                }
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.NotSupportedException ex)
            {
                logger.Log("[ERROR] System.NotSupportedException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void tornListView_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            InputImageListBox.Background = Brushes.AliceBlue;

        }

        private void CenterRightGrid_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            CenterRightGrid.Background = Brushes.AliceBlue;
        }

        private void CenterRightGrid_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            try
            {
                BrushConverter bc = new BrushConverter();
                CenterRightGrid.Background = (Brush)bc.ConvertFrom("#FFFDFDFD");
                logger.Log("[INFO] Center Right Grid Drag Left");
            }
            catch (System.NotSupportedException ex)
            {
                logger.Log("[ERROR] System.NotSupportedException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void tornListView_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            InputImageListBox.Background = Brushes.White;
        }

        private void OutputImage_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                if (!OutputImage.IsMouseCaptured) return;
                Vector v = start - e.GetPosition(OutputGrid);

                var tt = (TranslateTransform)((TransformGroup)OutputImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
                tt.X = origin.X - v.X;
                tt.Y = origin.Y - v.Y;

                if (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value) > 0)
                {
                    tt.X = 0;
                }
                else
                {
                    if (tt.X < (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2)
                    {
                        tt.X = (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2;
                    }
                    if (tt.X > -((OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2))
                    {
                        tt.X = -((OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2);
                    }
                }

                if (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value) > 0)
                {
                    tt.Y = 0;
                }
                else
                {
                    if (tt.Y < (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2)
                    {
                        tt.Y = (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2;
                    }
                    if (tt.Y > -((OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2))
                    {
                        tt.Y = -((OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2);
                    }
                }
                ImgProperty imgproperty = new ImgProperty(imgList[new Uri(constrPathname)].zoom, tt.X, tt.Y);
                imgList[new Uri(constrPathname)] = imgproperty;

                //System.Console.Write(border.ActualHeight + " " + constrImage.ActualHeight + " " + (border.ActualHeight - constrImage.ActualHeight * (zoom.Value)) + "\n");
                //System.Console.Write(tt.Y + "\n");
                logger.Log("[INFO] Output Image Mouse Moved");
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] System.UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void OutputImage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            LayoutRoot.Cursor = Cursors.Arrow;
            this.Cursor = Cursors.Arrow;
        }

        private void OutputImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            LayoutRoot.Cursor = Cursors.Hand;
            this.Cursor = Cursors.Hand;
        }

        private void OutputImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            zoom.Focus();
        }

        private void OutputImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Hand;
                this.Cursor = Cursors.Hand;
                OutputImage.CaptureMouse();
                var tt = (TranslateTransform)((TransformGroup)OutputImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
                start = e.GetPosition(OutputGrid);
                origin = new Point(tt.X, tt.Y);
                logger.Log("[INFO] Output Image Mouse Left Button Down");
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void OutputImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LayoutRoot.Cursor = Cursors.Hand;
            this.Cursor = Cursors.Hand;
            OutputImage.ReleaseMouseCapture();
        }


        private void OutputImage_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {
                if (imgCopy != null)
                {
                    TransformGroup transformGroup = (TransformGroup)OutputImage.RenderTransform;
                    ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

                    double zom = e.Delta > 0 ? .15 : -.15;
                    double temp = zoom.Value + zom;
                    if (temp >= 1 && temp <= 5)
                    {
                        zoom.Value += zom;
                        transform.ScaleX = zoom.Value;
                        transform.ScaleY = zoom.Value;
                    }
                    ImgProperty imgproperty = new ImgProperty(zoom.Value, imgList[new Uri(constrPathname)].X, imgList[new Uri(constrPathname)].Y);
                    imgList[new Uri(constrPathname)] = imgproperty;
                    //System.Console.Write(imgList[constrPathname]+"\n");

                    //Point v = start;
                    var tt = (TranslateTransform)((TransformGroup)OutputImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
                    //tt.X = origin.X - v.X;
                    //tt.Y = origin.Y - v.Y;
                    if (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value) > 0)
                    {
                        tt.X = 0;
                    }
                    else
                    {
                        if (tt.X < (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2)
                        {
                            tt.X = (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2;
                        }
                        if (tt.X > -((OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2))
                        {
                            tt.X = -((OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2);
                        }
                    }

                    if (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value) > 0)
                    {
                        tt.Y = 0;
                    }
                    else
                    {
                        if (tt.Y < (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2)
                        {
                            tt.Y = (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2;
                        }
                        if (tt.Y > -((OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2))
                        {
                            tt.Y = -((OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2);
                        }
                    }
                    imgproperty = new ImgProperty(imgList[new Uri(constrPathname)].zoom, tt.X, tt.Y);
                    imgList[new Uri(constrPathname)] = imgproperty;
                    logger.Log("[INFO] Output Image Mouse Wheel");
                }
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] System.UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        private void zoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (imgCopy != null)
                {
                    TransformGroup transformGroup = (TransformGroup)OutputImage.RenderTransform;
                    ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

                    //double zom = e.Delta > 0 ? .2 : -.2;
                    double zom = zoom.Value;
                    transform.ScaleX = zom;
                    transform.ScaleY = zom;
                    ImgProperty imgproperty = new ImgProperty(zoom.Value, imgList[new Uri(constrPathname)].X, imgList[new Uri(constrPathname)].Y);
                    imgList[new Uri(constrPathname)] = imgproperty;

                    //Point v = start;
                    var tt = (TranslateTransform)((TransformGroup)OutputImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
                    //tt.X = origin.X - v.X;
                    //tt.Y = origin.Y - v.Y;
                    if (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value) > 0)
                    {
                        tt.X = 0;
                    }
                    else
                    {
                        if (tt.X < (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2)
                        {
                            tt.X = (OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2;
                        }
                        if (tt.X > -((OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2))
                        {
                            tt.X = -((OutputGrid.ActualWidth - OutputImage.ActualWidth * (zoom.Value)) / 2);
                        }
                    }

                    if (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value) > 0)
                    {
                        tt.Y = 0;
                    }
                    else
                    {
                        if (tt.Y < (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2)
                        {
                            tt.Y = (OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2;
                        }
                        if (tt.Y > -((OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2))
                        {
                            tt.Y = -((OutputGrid.ActualHeight - OutputImage.ActualHeight * (zoom.Value)) / 2);
                        }
                    }
                    imgproperty = new ImgProperty(imgList[new Uri(constrPathname)].zoom, tt.X, tt.Y);
                    imgList[new Uri(constrPathname)] = imgproperty;
                    logger.Log("[INFO] Zoom Value Changed");
                }
            }
            catch (System.ArgumentNullException ex)
            {
                logger.Log("[ERROR] System.ArgumentNullException : " + ex.Message);
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] System.InvalidOperationException : " + ex.Message);
            }
            catch (System.UriFormatException ex)
            {
                logger.Log("[ERROR] System.UriFormatException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception " + ex.Message);
            }
        }

        FileSystemWatcher watcher = new FileSystemWatcher();
        // define the event handlers.
        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                watcher.EnableRaisingEvents = false;
                //watcher.Path = null;
                LayoutRoot.Dispatcher.Invoke((Action)(() =>
                {
                    ReconstructAgainBtn.Visibility = System.Windows.Visibility.Visible;
                    readOutputJson();
                    LoadingGrid.Visibility = Visibility.Collapsed;
                    LayoutRoot.Cursor = (Cursors.Arrow);
                    //output op = readOutputJson();
                    //MessageBox.Show(op.nFiles.ToString());

                    string constrPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MosaiQ\\Temp\\output1.jpg";
                    addConstructedImage(constrPath);
                    doNotSelectImage = 1;
                    AddBtn.IsEnabled = true;
                    //AddFoldersButton.Visibility = Visibility.Hidden;
                    MosaicItBtn.IsEnabled = true;
                }));
                isConstructed = 1;    
                string addrPath = myDocsPath + "\\MosaiQ\\Temp\\addr.txt";
                if (File.Exists(addrPath))
                {
                    File.Delete(addrPath);
                }
                addrPath = myDocsPath + "\\MosaiQ\\finish.txt";
                if (File.Exists(addrPath))
                {
                    File.Delete(addrPath);
                }
                logger.Log("[INFO] Construction completion information detected");
            }
            catch (System.ObjectDisposedException ex)
            {
                logger.Log("[ERROR] System.ObjectDisposedException : " + ex.Message);
            }
            catch (System.PlatformNotSupportedException ex)
            {
                logger.Log("[ERROR] System.PlatformNotSupportedException : " + ex.Message);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                logger.Log("[ERROR] System.IO.FileNotFoundException : " + ex.Message);
            }
            catch (System.ArgumentException ex)
            {
                logger.Log("[ERROR] System.ArgumentException : " + ex.Message);
            }
            catch(System.UnauthorizedAccessException ex)
            {
                logger.Log("[ERROR] System.UnauthorizedAccessException : "+ex.Message);
            }
            catch(System.IO.PathTooLongException ex)
            {
                logger.Log("[ERROR] System.IO.PathTooLongException : "+ex.Message);
            }
            catch(System.NotSupportedException ex)
            {
                logger.Log("[ERROR] System.NotSupportedException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : " + ex.Message);
            }
        }

        private string ConvertUriToPathname(Uri uri)
        {
            string pathname;

            try
            {
                pathname = uri.ToString().Substring(8);
                return pathname;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {    
                logger.Log("[ERROR] System.ArgumentOutOfRangeException : "+ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : " + ex.Message);
                return null;
            }
        }

        private void MosaicItBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MosaicItBtn_Func();
        }

        private void MosaicItBtn_Func()
        {
            try
            {
                if (isConstructed == 1)
                {
                    MessageBox.Show("The given inputs have already been reconstructed. If you want to perform the reconstruction again, Click on \"Reconstruct Again\" button.", "Mosaiq - Info",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (imgList.Count < 2)
                {
                    MessageBox.Show("Please add more than one images to start reconstruction.", "Mosaiq - Info",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (OS11.Properties.Settings.Default.algorithm == 0)
                {
                    MessageBox.Show("Could not find Java Implementation. Please check installation or choose Python version from settings.", "Mosaiq - Info",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;//run_cmd("java");
                }
                else
                {
                    if (!checkPython()) return;
                    if (checkImports() == 0) return;
                }
                writeJsonToConfigFile();
                doNotSelectImage = 1;
                string addrPath = myDocsPath + "\\MosaiQ\\Temp\\addr.txt";
                if (File.Exists(addrPath))
                {
                    File.Delete(addrPath);
                }
                addrPath = myDocsPath + "\\MosaiQ\\finish.txt";
                if (File.Exists(addrPath))
                {
                    File.Delete(addrPath);
                }

                OutputImage.Source = null;
                LoadingGrid.Visibility = Visibility.Visible;
                NextStepGrid.Visibility = Visibility.Hidden;

                LayoutRoot.Cursor = (Cursors.Wait);
                string tempPath = myDocsPath + "\\MosaiQ\\Temp";
                bool isExists = System.IO.Directory.Exists(tempPath);
                if (!isExists)
                {
                    System.IO.Directory.CreateDirectory(tempPath);
                }

                string waitPath = myDocsPath + "\\MosaiQ";
                addrPath = myDocsPath + "\\MosaiQ\\Temp\\addr.txt";
                FileStream fs1 = new FileStream(addrPath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter tw = new StreamWriter(fs1);
                foreach (KeyValuePair<Uri, ImgProperty> entry in imgList)
                {

                    //MessageBox.Show(entry.Key);
                    tw.WriteLine(ConvertUriToPathname(entry.Key));
                }
                tw.Close();


                waitPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MosaiQ";
                watcher.Path = waitPath;
                /* watch for changes in lastaccess and lastwrite times, and
                the renaming of files or directories. */
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                //| notifyfilters.filename | notifyfilters.directoryname;
                // only watch text files.
                watcher.Filter = "finish.txt";
                // add event handlers.
                //watcher.Changed += new FileSystemEventHandler(OnFileChanged);
                //watcher.Created += new FileSystemEventHandler(OnFileChanged);
                //watcher.Deleted += new FileSystemEventHandler(OnFileChanged);
                // begin watching.
                watcher.EnableRaisingEvents = true;

                AddBtn.IsEnabled = false;
                //AddFoldersButton.Visibility = Visibility.Hidden;
                MosaicItBtn.IsEnabled = false;
                doNotSelectImage = 1;

                if (OS11.Properties.Settings.Default.algorithm == 0)
                {
                    run_cmd("java");
                }
                else
                {
                    run_cmd("python");
                }

                logger.Log("[INFO] Mosaic button clicked");
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
                logger.Log("[ERROR] System.IO.UnauthorizedAccessException : " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : " + ex.Message);
            }
        }

        private int checkImports()
        {
            string addrPath = ".\\Python\\checkImports.pyw ";
            if (!File.Exists(addrPath))
            {
                MessageBox.Show("Required File checkImports.py was not found. Please check installation", "Mosaiq - Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }

            var p = new Process();
            p.StartInfo.FileName = "python";
            p.StartInfo.Arguments = ".\\Python\\checkImports.pyw ";

            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (!output.Contains("Success"))
            {
                MessageBox.Show(output, "Python Packages not found - Mosaiq", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
            return 1;
        }

        private bool checkPython()
        {
            try
            {
				System.Collections.IDictionary environmentVariables = Environment.GetEnvironmentVariables();
				string pathVariable = environmentVariables["Path"] as string;
				if (pathVariable != null)
				{
					string[] allPaths = pathVariable.Split(';');
					foreach (var path in allPaths)
					{
						string pythonPathFromEnv = path + "\\python.exe";
                        //MessageBox.Show(pythonPathFromEnv);
						if (File.Exists(pythonPathFromEnv))
						{
						   //Call Numpy and OpenCV here
							return true;

						}
					}

					logger.Log("[SUCCESS] checkPython : Python installation determined successfully");
				}
			}
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] InvalidOperationException " + ex.Message);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                logger.Log("[ERROR] FileNotFoundException " + ex.Message);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                logger.Log("[ERROR] System.ComponentModel.Win32Exception : "+ ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : " + ex.Message);
            }
            MessageBox.Show("Python was not found on this machine. Please install Python for using \"Python mode\" on Mosaiq", 
                "Error - Mosaiq - Module not found", MessageBoxButton.OK, MessageBoxImage.Stop);
            return false;
        }

        private void run_cmd(string algo)
        {
            //checkPython();
            try
            {
                string waitPath = myDocsPath + "\\MosaiQ\\Temp\\";
                string addrPath = myDocsPath + "\\MosaiQ\\Temp\\addr.txt";
                var p = new Process();
                p.StartInfo.FileName = @"python";
                //p.StartInfo.FileName = algo;
                p.StartInfo.Arguments = ".\\Python\\mosaiq.pyw " + addrPath + " " + waitPath;
                //MessageBox.Show(p.StartInfo.Arguments);
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                //string er = p.StandardError.ReadToEnd();
                //string output = p.StandardOutput.ReadToEnd();
                //p.WaitForExit();
                //MessageBox.Show(er+" "+output+"sajhs");

                logger.Log("[SUCCESS] Construction initiated successfully");
            }
            catch (System.InvalidOperationException ex)
            {
                logger.Log("[ERROR] InvalidOperationException : " + ex.Message);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                logger.Log("[ERROR] System.ComponentModel.Win32Exception : "+ex.Message);
            }
            catch (Exception ex)
            {
                logger.Log("[ERROR] Exception : " + ex.Message);
            }
        }

        private void ReconstructAgainBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //readJsonFromConfigFile();
            writeJsonToConfigFile();
            //BackBtn_Click(sender,e);
            writeJsonToConfigFile();
            isConstructed = 0;
            OutputImage.Source = null;
            DirectoryInfo dir = new DirectoryInfo(myDocsPath+"\\MosaiQ\\Temp");
            string path = myDocsPath+"\\MosaiQ\\Temp\\output1.jpg";

            foreach (FileInfo fi in dir.GetFiles())
            {
                try
                {
                    fi.Delete();
                }
                catch
                {
                   // MessageBox.Show("Couldnot delete "+fi.Name);
                }
            }


            MosaicItBtn_Func();
            //MosaicItBtn_Click(sender,e);
        }

        private PythonAppConfig readJsonFromConfigFile()
        {
            if (!File.Exists(myDocsPath+"\\MosaiQ\\Config\\config.json"))
            {
                MessageBox.Show("Python Config File not found.", "Mosaiq - Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(myDocsPath + "\\MosaiQ\\Config\\config.json"))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string config_contents = sb.ToString();
            PythonAppConfig appConfig = JsonConvert.DeserializeObject<PythonAppConfig>(config_contents);
            //update UI Also here
            Messenger.Default.Send<PythonAppConfig, MosaicViewModel>(appConfig);
            return appConfig;
        }

        private void writeJsonToConfigFile()
        {
            PythonAppConfig appConfig = new PythonAppConfig();
            
            appConfig.arcLengthMultiplier = ArcLengthMultiplierSettingSlider.Value.ToString();
            appConfig.scaleFactor = ScaleFactorSettingSlider.Value.ToString();
            appConfig.approxPolyparameter = ApproxPolygonParamSlider.Value.ToString();
            appConfig.backgroundThreshold = BackgroundThresholdSlider.Value.ToString();
            appConfig.noiseReduction = (NoiseReductionCheckBox.IsChecked == true) ? "1" : "0";
            appConfig.multiEdge = (MultiEdgeParamCheckBox.IsChecked == true) ? "1" : "0";
            
            string result = JsonConvert.SerializeObject(appConfig);
            //MessageBox.Show("Edited to:\n" + result);
            System.IO.File.WriteAllText(myDocsPath+"\\MosaiQ\\Config\\config.json", result);
            logger.Log("[INFO] Update the python config file successfully!");
        }

        private output readOutputJson()
        {
            if (!File.Exists(myDocsPath + "\\MosaiQ\\Temp\\output.json"))
            {
                MessageBox.Show("Python output File not found.", "Mosaiq - Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(myDocsPath + "\\MosaiQ\\Temp\\output.json"))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string config_contents = sb.ToString();
            output op = JsonConvert.DeserializeObject<output>(config_contents);
            //update UI Also here
            //Messenger.Default.Send<PythonAppConfig, MosaicViewModel>(op);
            MessageBox.Show("Status : "+op.status+ "\n \n" +op.msg+"\n No of files created :"+op.nFiles.ToString()
                    +". Showing first file. \n\n All files present in "+myDocsPath+"\\MosaiQ");
            return op;
        }

        private void BackBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(myDocsPath + "\\MosaiQ\\Temp");

            foreach (FileInfo fi in dir.GetFiles())
            {
                try
                {
                    fi.Delete();
                }
                catch
                {
                    // MessageBox.Show("Couldnot delete "+fi.Name);
                }
            }
        	Messenger.Default.Send<string, MainViewModel>("mainPage");
			// TODO: Add event handler implementation here.
        }

        

        
        //private void AddFoldersButton_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    FolderBrowser dlg = new FolderBrowser();
        //    dlg.ShowDialog();

        //    string filepath = dlg.SelectedPath;

        //    if (filepath != "")
        //    {
        //        Mouse.SetCursor(Cursors.Wait);
        //        //string[] pathnames = Directory.GetFiles(filepath, "*.*").Where(file => file.ToLower().EndsWith("aspx") || file.ToLower().EndsWith("ascx"));
        //        List<String> pathnames = Directory.GetFiles(filepath, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("jpeg") || file.ToLower().EndsWith("bmp")).ToList();

        //        addImagesToList(pathnames);
        //    }
        //    Mouse.SetCursor(Cursors.Arrow);
        //}


        //private void constrImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    zoom.Focus();
        //}

        
        //private int pid=0;
        //private void cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if(pid==0)
        //    {
        //        return;
        //    }
        //    Process p = Process.GetProcessById(pid);
        //    p.Kill();
        //    //MessageBox.Show("Process was killed " + pid.ToString());
        //}

        //private void newConstruction_Click(object sender, RoutedEventArgs e)
        //{
        //    imgCopy = null;
        //    imgList.Clear();
        //    tornListView.Items.Clear();
        //    notTornPieceGrid = true;
        //    constrPathname = "";
        //    doNotSelectImage = 0;

        //    AddButton.Visibility = Visibility.Visible;
        //    AddFoldersButton.Visibility = Visibility.Visible;
        //    constrImage.Source = null;

        //    leftRotate.Visibility = Visibility.Hidden;
        //    rightRotate.Visibility = Visibility.Hidden;
        //    zoom.Visibility = Visibility.Hidden;

        //    notTornPieceGrid = true;
        //    constructButton.Visibility = Visibility.Hidden;
        //}

        //private void ExitApplication_Click(object sender, RoutedEventArgs e)
        //{
        //    Environment.Exit(0);
        //}

    }

}