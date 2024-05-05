using System.Linq;
using Const;
using Services;
using Storage;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainCameraMovement : Singleton<MainCameraMovement>
{
     private ComponentShareService ComponentShareService => FindObjectOfType<ComponentShareService>();
     
        private Camera _camera;
        private GameMiddleware _middleware;
        private HexagonTileStorage _hexagonTileStorage;
        private Vector3 _lastMousePosition;
        
        [SerializeField]
        public float cameraSize = 1.5f;
        
        [SerializeField]
        public float cameraSpeed = 2f;
        
        [SerializeField]
        public float zoomSpeed = 5f;


        public void Start()
        {
            _camera = ComponentShareService.GetComponentByTypeAndTag<Camera>(Constants.MAIN_CAMERA);
            _camera.orthographicSize = cameraSize;
        }


        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                _handleClickAndMove();
            }
            
            var mousePosition = Input.mousePosition;
            var cameraPosition = _camera.transform.position;


            if (mousePosition.x <= 1)
            {
                var targetPosition = cameraPosition + Vector3.left * (cameraSpeed * Time.deltaTime);
                _camera.transform.position = targetPosition;
            }
            else if (mousePosition.x >= Screen.width - 1)
            {
                var targetPosition = cameraPosition + Vector3.right * (cameraSpeed * Time.deltaTime);
                _camera.transform.position = targetPosition;
            }

            if (mousePosition.y <= 1)
            {
                var targetPosition = cameraPosition + Vector3.down * (cameraSpeed * Time.deltaTime);
                _camera.transform.position = targetPosition;
            }
            else if (mousePosition.y >= Screen.height - 1)
            {
                var targetPosition = cameraPosition + Vector3.up * (cameraSpeed * Time.deltaTime);
                _camera.transform.position = targetPosition;
            }
            
            
            var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta != 0f)
            {
                _mouseWheelScroll(scrollDelta);
            }
        }


        private void _mouseWheelScroll(float scrollDelta)
        {
            var mousePosition = Input.mousePosition;
            var scrollCenter = _camera.ScreenToWorldPoint(mousePosition);

            var newSize = _camera.orthographicSize - scrollDelta * zoomSpeed;
            newSize = Mathf.Clamp(newSize, 1f, float.MaxValue);

            _camera.orthographicSize = newSize;

            var delta = scrollCenter - _camera.ScreenToWorldPoint(mousePosition);

            _camera.transform.position += delta;
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
                _camera.transform.Translate(-delta.x, -delta.y, 0);
                _lastMousePosition = Input.mousePosition;
            }
        }
}