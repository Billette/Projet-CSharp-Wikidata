using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class COUNTRY
    {
        public string Name { get; set; }
        public string CapitalCity { get; set; }
        public COUNTRY (string n, string c) { Name = n; CapitalCity = c; }

    }



    class VM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }

        public VM()
        {
            Program.MyHttp();
            M model = new M();
            myText = "ECE Paris";

            collection = new ObservableCollection<COUNTRY>();
            COUNTRY c1 = new COUNTRY("France", "Paris");
            COUNTRY c2 = new COUNTRY("Spain", "Madrid");
            COUNTRY c3 = new COUNTRY("Italy", "Roma");
            collection.Add(c1);
            collection.Add(c2);
            collection.Add(c3);
        }

        private COUNTRY selectedCountry;
        public COUNTRY SelectedCountry
        {
            get {
                return this.selectedCountry;
            }
            set {
                selectedCountry = value;
                MyText2 = SelectedCountry.CapitalCity;
                OnPropertyChanged("SelectedCountry");
            }
        }


        private ObservableCollection<COUNTRY> collection;
        public ObservableCollection<COUNTRY> Collection
        {
            get { return this.collection; }
            set { this.collection = value; }
        }

        private string myText;
        public string MyText
        {
            get { return this.myText; }
            set
            {
                this.myText = value;
                OnPropertyChanged("MyText");
            }
        }

        private string myText2 = "No City selected";
        public string MyText2
        {
            get { return this.myText2; }
            set
            {

                this.myText2 = value;

                //Console.WriteLine(this.myText2);
                OnPropertyChanged("MyText2");
            }
        }

    }
}
