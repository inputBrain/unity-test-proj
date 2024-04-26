using System.Linq;
using Services;
using Storage;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainCameraMovement : Singleton<MainCameraMovement>
{
        public Camera mainCamera;
        public Tilemap tilemap;
        
        private GameMiddleware _middleware;
        private HexagonTileStorage _hexagonTileStorage;
        public GameObject hexGO;
        
        [SerializeField]
        public float cameraSize = 1.5f;
        
        [SerializeField]
        public float cameraSpeed = 2f;


        [SerializeField]
        public float zoomSpeed = 5f;
        

        public void Start()
        {
                var middleware = FindObjectOfType<GameMiddleware>();
                _hexagonTileStorage = hexGO.GetComponent<HexagonTileStorage>();

                var userCountry = _hexagonTileStorage.TilesData.FirstOrDefault(x => x.Value.Name == middleware.SelectedCountry);
                
                Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(new Vector3Int((int)userCountry.Key.x, (int)userCountry.Key.y));
                
                Vector3 targetPosition = tileWorldPosition;
                targetPosition.z = mainCamera.transform.position.z; 
                mainCamera.transform.position = targetPosition;


                mainCamera.orthographicSize = cameraSize;
        }


        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var cameraPosition = mainCamera.transform.position;


            if (mousePosition.x <= 1)
            {
                var targetPosition = cameraPosition + Vector3.left * (cameraSpeed * Time.deltaTime);
                mainCamera.transform.position = targetPosition;
            }
            else if (mousePosition.x >= Screen.width - 1)
            {
                var targetPosition = cameraPosition + Vector3.right * (cameraSpeed * Time.deltaTime);
                mainCamera.transform.position = targetPosition;
            }

            if (mousePosition.y <= 1)
            {
                var targetPosition = cameraPosition + Vector3.down * (cameraSpeed * Time.deltaTime);
                mainCamera.transform.position = targetPosition;
            }
            else if (mousePosition.y >= Screen.height - 1)
            {
                var targetPosition = cameraPosition + Vector3.up * (cameraSpeed * Time.deltaTime);
                mainCamera.transform.position = targetPosition;
            }
            
            
            var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta != 0f)
            {
                MouseWheelScroll(scrollDelta);
            }
        }


        private void MouseWheelScroll(float scrollDelta)
        {
            float newSize = mainCamera.orthographicSize - scrollDelta * zoomSpeed;
                
            newSize = Mathf.Max(newSize, 1f);

            mainCamera.orthographicSize = newSize;
        }
}