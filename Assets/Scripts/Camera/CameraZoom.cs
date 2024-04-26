using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float zoom;
    private float velocity = 0f;

    [SerializeField]
    private float zoomMultiplier = 4f;
    [SerializeField]
    private float minZoom = 2f;
    [SerializeField]
    private float maxZoom = 8f;
    [SerializeField]
    private float smoothTime = 0.25f;
    
    private Camera _mainCamera;
    private void Awake() => _mainCamera = Camera.main;

    private void Start()
    {
        zoom = _mainCamera.orthographicSize;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        zoom -= scroll / zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        _mainCamera.orthographicSize = Mathf.SmoothDamp(_mainCamera.orthographicSize, zoom, ref velocity, smoothTime);
    }
}