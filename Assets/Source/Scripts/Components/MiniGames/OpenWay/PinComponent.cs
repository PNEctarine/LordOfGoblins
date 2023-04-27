using UnityEngine;

public class PinComponent : MonoBehaviour
{
    [SerializeField] private Transform[] PathTransform;
    [SerializeField] private float _maxLength;
    [SerializeField] private bool _isEnemy;

    private OpenWay _openWay;
    private float _length;
    private Vector2 _startMousePosition;
    private Vector2 _lastMousePosition;
    private Vector2 _startPosition;
    private Vector3[] _path;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _startPosition = gameObject.transform.position;
        _openWay = GetComponentInParent<OpenWay>();

        _path = new Vector3[PathTransform.Length];

        for (int i = 0; i < PathTransform.Length; i++)
        {
            _path[i] = PathTransform[i].position;
        }
    }

    public void OnMouseDown()
    {
        _startMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _lastMousePosition = _startMousePosition;
    }

    public void OnMouseUp()
    {
        gameObject.transform.position = _startPosition;
    }

    private void OnMouseDrag()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 position = mousePosition - _lastMousePosition;

        _length = Mathf.Abs(mousePosition.magnitude - _startMousePosition.magnitude);

        if (_length < _maxLength)
        {
            gameObject.transform.position += gameObject.transform.up.normalized * position.magnitude;
            _lastMousePosition = mousePosition;
            Debug.Log(gameObject.transform.up.normalized * position.magnitude);
        }

        else
        {
            //if (_isEnemy)
            //{
            //    Debug.Log("LOSE");
            //}

            _openWay.SetPath(_path);
            Destroy(gameObject);
        }
    }
}
