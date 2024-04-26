using System.Linq;
using Services;
using Storage;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainCameraMovement : Singleton<MainCameraMovement>
{
        public Camera mainCamera;
        
        public Tilemap tilemap;
        
        public GameObject hexGO;
        
        [SerializeField]
        public float cameraSize = 1.5f;
        
        [SerializeField]
        public float cameraSpeed = 2f;
        
        [SerializeField]
        public float zoomSpeed = 5f;

        
        private GameMiddleware _middleware;
        private HexagonTileStorage _hexagonTileStorage;
        private Vector3 _lastMousePosition;


        public void Start()
        {
            var middleware = FindObjectOfType<GameMiddleware>();
            _hexagonTileStorage = hexGO.GetComponent<HexagonTileStorage>();

            var userCountry = _hexagonTileStorage.TilesData.FirstOrDefault(x => x.Value.Name == middleware.SelectedCountry);
            
            var tileWorldPosition = tilemap.GetCellCenterWorld(new Vector3Int((int)userCountry.Key.x, (int)userCountry.Key.y));
            
            var targetPosition = tileWorldPosition;
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
        }


        private void _mouseWheelScroll(float scrollDelta)
        {
            var mousePosition = Input.mousePosition;
            var scrollCenter = mainCamera.ScreenToWorldPoint(mousePosition);

            var newSize = mainCamera.orthographicSize - scrollDelta * zoomSpeed;
            newSize = Mathf.Clamp(newSize, 1f, float.MaxValue);

            mainCamera.orthographicSize = newSize;

            var delta = scrollCenter - mainCamera.ScreenToWorldPoint(mousePosition);

            mainCamera.transform.position += delta;
        }
        
        private void _handleClickAndMove()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                var delta = (Input.mousePosition - _lastMousePosition) * (cameraSpeed * Time.deltaTime);
                mainCamera.transform.Translate(-delta.x, -delta.y, 0);
                _lastMousePosition = Input.mousePosition;
            }
        }
}