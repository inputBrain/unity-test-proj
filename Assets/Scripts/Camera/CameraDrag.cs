using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    public bool smoothDrag;
    
    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;
    private Vector3 _targetPosition;

    private Vector3 GetMousePosition() => _mainCamera.ScreenToWorldPoint(Input.mousePosition);

    private void Awake() => _mainCamera = Camera.main;
    
    private void Update()
    {
        if (smoothDrag)
        {
            SmoothDrag();
        }
        else
        {
            NormalDrag();
        }
    }

    private void SmoothDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _origin = GetMousePosition();
            _targetPosition = transform.position;
        }
        if (!Input.GetMouseButton(0)) return;
        
        _difference = GetMousePosition() - _origin;
        _targetPosition -= _difference;
        
        float lerpSpeed = 10f;
        Vector3 direction = _difference.normalized;
        
        var position = transform.position;

        _targetPosition.x = Mathf.Lerp(position.x, _targetPosition.x, Time.deltaTime * lerpSpeed * Mathf.Abs(direction.x));
        _targetPosition.y = Mathf.Lerp(position.y, _targetPosition.y, Time.deltaTime * lerpSpeed * Mathf.Abs(direction.y));
        _targetPosition.z = Mathf.Lerp(position.z, _targetPosition.z, Time.deltaTime * lerpSpeed * Mathf.Abs(direction.z));

        position = _targetPosition;
        transform.position = position;
    }

    private void NormalDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _origin = GetMousePosition();
        }
        if (!Input.GetMouseButton(0)) return;
        
        _difference = GetMousePosition() - _origin;
        transform.position -= _difference;
    }
}