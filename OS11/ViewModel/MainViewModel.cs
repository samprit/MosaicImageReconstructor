using System.Windows.Input;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace OS11.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public static string filepath;
        Logger logger = new Logger();
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public ICommand HomePageViewCommand { get; private set; }
        public ICommand MosaicViewCommand { get; private set; }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            //HomePageViewCommand = new RelayCommand(() => ExecuteHomePageViewCommand());
            //MosaicViewCommand = new RelayCommand(() => ExecuteMosaicViewCommand("none"));
            CurrentViewModel = new HomePageViewModel();

            Messenger.Default.Register<string>(this, vm => { ExecuteMessage(vm); });
            //CurrentViewModel = MainViewModel._mosaicViewModel;
            
        }

        private void ExecuteMessage(string message)
        {
            try
            {
                if (message == "homePage")
                {
                    filepath = "";
                    CurrentViewModel = new MosaicViewModel();
                }
				 if (message == "mainPage")
                {
                    filepath = "";
                    CurrentViewModel = new HomePageViewModel();
                }
                else if (message.EndsWith("$homePage"))
                {
                    filepath = message.Substring(0, message.IndexOf("$homePage"));
                    CurrentViewModel = new MosaicViewModel();
                }
                logger.Log("[SUCCESS] MosaicViewModel successfully loaded");
            }
            catch(System.Exception ex)
            {
                logger.Log("[ERROR] Exception : "+ ex.Message);
            }
        }
    }
}