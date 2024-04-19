using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SelectCountryButton : Singleton<SelectCountryButton>
    {
        public Dropdown countryDropdown;
        public Button startGameButton;
        public Text selectCountryButtonText;
        private StartGameButton _startGameButton;
        private List<string> countryNames = new List<string>();
        
        void Start()
        {
            _startGameButton = GetComponent<StartGameButton>();
        }
        
        
        void LoadCountryNames()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "countries.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Models.CountryJsonModel> countries = JsonUtility.FromJson<List<Models.CountryJsonModel>>(json);
                foreach (var country in countries)
                {
                    countryNames.Add(country.Country);
                }
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
            }
        }
        
        
        void UpdateDropdownOptions()
        {
            countryDropdown.ClearOptions();
            countryDropdown.AddOptions(countryNames);
        }
        
        
        public void OnCountrySelected(int index)
        {
            var selectedCountryName = countryNames[index];
            selectCountryButtonText.text = selectedCountryName;
            startGameButton.interactable = true;
            // _startGameButton.SetSelectedCountry(selectedCountryName);
        }
        
        
        public void OnStartGameButtonClicked()
        {
            var selectedCountryName = selectCountryButtonText.text;
            _startGameButton.StartGame(selectedCountryName);
        }


        void Update()
        {
        
        }
    }
}
