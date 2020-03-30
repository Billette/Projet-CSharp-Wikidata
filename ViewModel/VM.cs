using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    class Requester : ICommand
    {
        #region ICommand Members  

        private VM viewModel;
        public VM ViewModel
        {
            get { return this.viewModel; }
            set
            {
                this.viewModel = value;
            }
        }

        public Requester(VM _viewModel)
        {
            ViewModel = _viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if(parameter != null)
            {
                Inputs inputs = (Inputs)parameter;
                
                string cityName = inputs.cityName ?? "";
                string minPop = inputs.minPop ?? "";
                string maxPop = inputs.maxPop ?? "";
                string minPostCode = inputs.minPostCode ?? "";
                string maxPostCode = inputs.maxPostCode ?? "";
                
                ViewModel.MakeRequest(minPostCode, maxPostCode, minPop, maxPop, cityName);
            } else
            {
                Console.WriteLine("No parameter");
            }
            
            //Console.WriteLine("Clicked");
        }
        #endregion
    }

    class Hyperlinker : ICommand
    {
        #region ICommand Members  

        private VM viewModel;
        public VM ViewModel
        {
            get { return this.viewModel; }
            set
            {
                this.viewModel = value;
            }
        }

        public Hyperlinker(VM _viewModel)
        {
            ViewModel = _viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                string url = (string)parameter;
                Console.WriteLine("Navigation to URL : {0}", url);
                System.Diagnostics.Process.Start(url);
            }
            else
            {
                Console.WriteLine("No parameter");
            }

            //Console.WriteLine("Clicked");
        }
        #endregion
    }


    class VM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }


        public async void MakeRequest(string _postalCodeMin, string _postalCodeMax, string _populationMin, string _populationMax, string _cityName)
        {
            await Wikidata.Request(_postalCodeMin, _postalCodeMax, _populationMin, _populationMax, _cityName);
            ObservableCollection<City> CityCollection = new ObservableCollection<City>();

            // Visualisation
            if (Wikidata.requestedCities != null)
            {
                if (Wikidata.requestedCities.list.Count == 0)
                    Console.WriteLine("No result found.");
                else
                {
                    foreach (City c in Wikidata.requestedCities.list)
                    {
                        Console.WriteLine("City name : " + c.cityLabel);
                        Console.WriteLine("Population : " + c.population);
                        Console.WriteLine("Postal code : " + c.postalCode);
                        Console.WriteLine("Wikidata Reference : " + c.cityRef);
                        Console.WriteLine("----");

                        CityCollection.Add(c);
                    }

                    Collection = CityCollection;
                }
            }
            else
            {
                Console.WriteLine("Error : Bad Request");
            }
        }


        public VM()
        {
            
        }

        private ICommand mRequester;
        public ICommand RequestCommand
        {
            get
            {
                if (mRequester == null)
                    mRequester = new Requester(this);
                return mRequester;
            }
            set
            {
                mRequester = value;
            }
        }

        private ICommand mHyperlinker;
        public ICommand HyperlinkCommand
        {
            get
            {
                if (mHyperlinker == null)
                    mHyperlinker = new Hyperlinker(this);
                return mHyperlinker;
            }
            set
            {
                mHyperlinker = value;
            }
        }

        private City selectedCity;
        public City SelectedCity
        {
            get {
                return this.selectedCity;
            }
            set {
                selectedCity = value;
                if(selectedCity != null)
                {
                    DisplayPostalCode = SelectedCity.postalCode;
                } else
                {
                    DisplayPostalCode = "No City selected";
                }
                OnPropertyChanged("SelectedCity");
            }
        }


        private ObservableCollection<City> collection;
        public ObservableCollection<City> Collection
        {
            get { return this.collection; }
            set {
                this.collection = value;
                OnPropertyChanged("Collection");
            }
        }

        private string inputCityName = "";
        public string InputCityName
        {
            get { return this.inputCityName; }
            set
            {
                this.inputCityName = value;
                AllInputs.cityName = value;
                OnPropertyChanged("InputCityName");
            }
        }

        private string inputMinPop;
        public string InputMinPop
        {
            get { return this.inputMinPop; }
            set
            {
                this.inputMinPop = value;
                AllInputs.minPop = value;
                OnPropertyChanged("InputMinPop");
            }
        }

        private string inputMaxPop;
        public string InputMaxPop
        {
            get { return this.inputMaxPop; }
            set
            {
                this.inputMaxPop = value;
                AllInputs.maxPop = value;
                OnPropertyChanged("InputMinPop");
            }
        }

        private string inputMinPostCode;
        public string InputMinPostCode
        {
            get { return this.inputMinPostCode; }
            set
            {
                this.inputMinPostCode = value;
                AllInputs.minPostCode = value;
                OnPropertyChanged("InputMinPostCode");
            }
        }

        private string inputMaxPostCode;
        public string InputMaxPostCode
        {
            get { return this.inputMaxPostCode; }
            set
            {
                this.inputMaxPostCode = value;
                AllInputs.maxPostCode = value;
                OnPropertyChanged("InputMaxPostCode");
            }
        }

        // Make all the inputs into a single object
        private Inputs allInputs;
        public Inputs AllInputs
        {
            get {
                if(allInputs == null)
                {
                    allInputs = new Inputs();
                    return allInputs;
                } else {
                    return this.allInputs;
                }
            }
            set
            {
                this.allInputs = value;
            }
        }

        private string displayPostalCode = "No City selected";
        public string DisplayPostalCode
        {
            get { return this.displayPostalCode; }
            set
            {
                this.displayPostalCode = value;
                OnPropertyChanged("DisplayPostalCode");
            }
        }

    }
}
