using UnityEngine;

public class MasterKey : MonoBehaviour
{
    [SerializeField] private float _maxLength;
    [SerializeField] private bool _isEnemy;

    private float _length;

    private Camera _mainCamera;
    private Vector2 _startMousePosition;
    private Vector2 _startPosition;
    private Vector2 _offset;

    private void Awake()
    {
        _startPosition = gameObject.transform.position;
        _mainCamera = Camera.main;
    }

    public void OnMouseDown()
    {
        _startMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _offset = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    public void OnMouseUp()
    {
        gameObject.transform.position = _startPosition;
    }

    private void OnMouseDrag()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        _length = Mathf.Abs(mousePosition.magnitude - _startMousePosition.magnitude);
        Debug.Log($"{_length} {mousePosition.magnitude - _startMousePosition.magnitude}");

        if (_length < _maxLength)
        {
            gameObject.transform.position = mousePosition - _offset;
        }
    }
}
