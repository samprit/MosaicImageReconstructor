using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using OS11.Model;
using Newtonsoft.Json;

namespace OS11.ViewModel
{
    class MosaicViewModel : ViewModelBase
    {
        ObservableCollection<tornImage> _tornListOC = new ObservableCollection<tornImage>();
        PythonAppConfig _appConfig = new PythonAppConfig();
        bool _noiseReduction;
        bool _multiEdge;

        public bool noiseReduction
        {
            get
            {
                return this._noiseReduction;
            }
            set
            {
                this._noiseReduction = value;
                RaisePropertyChanged("noiseReduction");
            }

        }
        public bool multiEdge
        {
            get
            {
                return this._multiEdge;
            }
            set
            {
                this._multiEdge = value;
                RaisePropertyChanged("multiEdge");
            }

        }
        
        public PythonAppConfig appConfig
        {
            get
            {
                return this._appConfig;
            }
            set
            {
                this._appConfig = value;
                RaisePropertyChanged("appConfig");
            }
        }

        public ObservableCollection<tornImage> TornListOC
        {
            get
            {
                return this._tornListOC;
            }
        }


        public MosaicViewModel(){
            Messenger.Default.Register<List<string>>(this, m => { addImageToList(m); });
            Messenger.Default.Register<string>(this, m => { ExecuteMessage(m); });
            Messenger.Default.Register<Uri>(this, m => { removeFromList(m); });
            Messenger.Default.Register<PythonAppConfig>(this, m => { UpdatePythonConfig(m); });
        }

        private void addImageToList(List<string> pathnames){
			if(pathnames.Count>0){	
				foreach(string pathname in pathnames)
				{
					//var brush1 = new ImageBrush();
					//brush1.ImageSource = new BitmapImage(new Uri(pathname));
					tornImage TornImg = new tornImage(new Uri(pathname));
					TornImg.tornImageIndex = _tornListOC.Count-1;
	
					_tornListOC.Add(TornImg);
				}
				RaisePropertyChanged("TornListOC");
			}
        }

        private void removeFromList(Uri removeUri)
        {
            //tornImage TornImg = new tornImage(removeUri);
            //MessageBox.Show(removeUri.ToString());
            int index = 0, removeIndex=-1;
            foreach(tornImage TornImg in _tornListOC){
                if (TornImg.tornImageUri.ToString() == removeUri.ToString())
                {
                    removeIndex = index;
                    break;
                }
                index++;
            }
            if (removeIndex != -1)
            {
                _tornListOC.RemoveAt(removeIndex);
                RaisePropertyChanged("TornListOC");
            }
        }

        private void ExecuteMessage(string message)
        {
            if (message == "ClearTornImageList")
            {
                _tornListOC.Clear();
                RaisePropertyChanged("TornListOC");
            }
        }
        private void UpdatePythonConfig(PythonAppConfig app_config)
        {
            appConfig = app_config;
            if (app_config.noiseReduction == "1") noiseReduction = true;
            else noiseReduction = false;
            if (app_config.multiEdge == "1") multiEdge = true;
            else multiEdge = false;
        }
    }
}
