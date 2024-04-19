using UnityEngine;

namespace MainMenu
{
    public class StartGameButton : MonoBehaviour
    {
        private TilemapGenerator _tilemapGenerator;

        private void Start()
        {
            _tilemapGenerator = GetComponent<TilemapGenerator>();
        }

        public void StartGame(string selectedCountryName)
        {
            var countryTilePos = _tilemapGenerator.FindCountryTile(selectedCountryName);
            
            if (countryTilePos != Vector3Int.zero)
            {
                Debug.Log("Found country tile for: " + selectedCountryName);
                // SceneManager.LoadScene("SimpleScene");
            }
            else
            {
                Debug.Log("Country tile not found for: " + selectedCountryName);
            }
        }
    }
}