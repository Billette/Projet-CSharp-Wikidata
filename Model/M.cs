using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Cities
    {
        public List<City> list { get; set; }
    }

    public class City
    {
        public string cityRef { get; set; }
        public string population { get; set; }
        public string postalCode { get; set; }
        public string cityLabel { get; set; }
    }

    // Make all inputs into objects
    public class Inputs
    {
        public string cityName { get; set; }
        public string minPop { get; set; }
        public string maxPop { get; set; }
        public string minPostCode { get; set; }
        public string maxPostCode { get; set; }
    }
}
