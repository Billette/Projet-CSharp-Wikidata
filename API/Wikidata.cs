﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Wikidata
    {
        static public Cities requestedCities { get; set; }
        
        static public async Task<string> Request(string _postalCodeMin, string _postalCodeMax, string _populationMin, string _populationMax, string _cityName)
        {
            Cities cities = new Cities();
            List<City> cityList = new List<City>();

            string postalCodeMin = _postalCodeMin;
            string postalCodeMax = _postalCodeMax;
            string populationMin = _populationMin;
            string populationMax = _populationMax;
            string cityName = _cityName;

            string Path;

            if(cityName == "")
            {
                Path = "https://query.wikidata.org/sparql?format=json&action=wbgetentities&query=SELECT%20DISTINCT%20%3Fcity%20%3FcityLabel%20%3Fcity_FR%20%3Fpopulation%20%3Fcode_postal%20WHERE%20%7B%0A%20%20%3Fcity%20wdt%3AP31%20wd%3AQ484170%20.%0A%20%20%3Fcity%20%20wdt%3AP1082%20%3Fpopulation%20.%0A%20%20%3Fcity%20wdt%3AP281%20%3Fcode_postal%20.%0A%20%20SERVICE%20wikibase%3Alabel%20%7B%20bd%3AserviceParam%20wikibase%3Alanguage%20%22fr%22.%0A%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%3Fcity%20rdfs%3Alabel%20%3Fcity_FR.%0A%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%20hint%3APrior%20hint%3ArunLast%20false.%0A%20%20%0A%20%20FILTER%20(%20(%3Fpopulation%20%3E%3D%20" + populationMin + "%20%26%26%20%3Fpopulation%20%3C%3D%20" + populationMax + ")%20%26%26%20(%3Fcode_postal%20%3E%3D%20%22" + postalCodeMin + "%22%20%26%26%20%3Fcode_postal%20%3C%3D%20%22" + postalCodeMax + "%22))%0A%7D";
            }
            else {
                Path = "https://query.wikidata.org/sparql?format=json&action=wbgetentities&query=SELECT%20DISTINCT%20%3Fcity%20%3FcityLabel%20%3Fcity_FR%20%3Fpopulation%20%3Fcode_postal%20WHERE%20%7B%0A%20%20%3Fcity%20wdt%3AP31%20wd%3AQ484170%20.%0A%20%20%3Fcity%20rdfs%3Alabel%20%22"+ cityName +"%22%40fr%20.%0A%20%20%3Fcity%20%20wdt%3AP1082%20%3Fpopulation%20.%0A%20%20%3Fcity%20wdt%3AP281%20%3Fcode_postal%20.%0A%20%20SERVICE%20wikibase%3Alabel%20%7B%20bd%3AserviceParam%20wikibase%3Alanguage%20%22fr%22.%0A%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%3Fcity%20rdfs%3Alabel%20%3Fcity_FR.%0A%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%20hint%3APrior%20hint%3ArunLast%20false.%20%20%0A%7D";
            }


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            //var result = await client.GetAsync(Path);
            string content = await client.GetStringAsync(Path);

            JObject jsonObj = JObject.Parse(content);

            //Console.WriteLine(jsonObj["results"]["bindings"][2]);
            char dbQuote = (char)34;

            // Black Magic
            foreach (Object o in jsonObj["results"]["bindings"])
            {
                String str = o.ToString();                
                String sectionToFind = dbQuote + "value" + dbQuote + ": ";

                var sentences = new List<String>();
                int position = 0;
                int start = 0;
                int length = sectionToFind.Length;

                while (position >= 0 )
                {
                    position = str.IndexOf(sectionToFind, start);
                    if(position >= 0)
                    {
                        String tmp = str.Substring(position + length + 2);
                        int endValueIndex = tmp.IndexOf(dbQuote);
                        String endStr = str.Substring(position + length + 1, length + endValueIndex - 8);
                        sentences.Add(endStr);
                        start = position + 1;
                    }         
                }

                City cityFound = new City();
                cityFound.cityRef = sentences[0];
                cityFound.population = sentences[1];
                cityFound.postalCode = sentences[2];
                cityFound.cityLabel = sentences[3];

                cityList.Add(cityFound);

            }

            cities.list = cityList;

            requestedCities = cities;
            return "OK";
        }
        
    }
}
