using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using Newtonsoft.Json;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : Singleton<MainMenu>
    {
        public Dropdown countryDropdown;
        
        private List<string> countryNames = new List<string>();
        public TextAsset countriesJson;

        [SerializeField]
        private GameMiddleware _helper;

        
        void Start()
        {
            LoadCountryNames();
            countryDropdown.AddOptions(countryNames);
        }
        
        
        void LoadCountryNames()
        {
            var countries = JsonConvert.DeserializeObject<List<CountryJsonModel>>(countriesJson.text);
        
            foreach (var country in countries.OrderBy(x => x.Country))
            {
                countryNames.Add(country.Country);
            }
        }

        public void OnStartClick()
        {
            _helper.SelectedCountry = countryDropdown.options[countryDropdown.value].text;
            SceneManager.LoadScene("GridMapScene");
        }
    }
}
