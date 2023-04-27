using UnityEngine;

public class DrawComponent : MonoBehaviour
{
    private Transform _startPoint;
    private Transform[] _points;

    private Vector2 _offset;
    private Vector2 _lastMousePosition;

    private LineRenderer _currentLineRenderer;
    private Camera _mainCamera;

    private int _currentPoint;
    private float _lastDistance;

    private const float _swipeOffset = 0.005f;
    private const float _backOffset = -0.002f;
    private const float _pointOffset = 0.2f;

    private bool _isDrawing;

    private void Awake()
    {
        _currentLineRenderer = gameObject.GetComponent<LineRenderer>();
        _mainCamera = Camera.main;
    }

    public void SetFiqure(Transform startPoint, Transform[] points)
    {
        gameObject.SetActive(true);

        _startPoint = startPoint;
        _points = points;

        gameObject.transform.position = _startPoint.position;
    }

    public void OnMouseDown()
    {
        _isDrawing = true;
        _offset = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _lastMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _lastDistance = Vector2.Distance(gameObject.transform.position, _points[_currentPoint].position);
    }

    public void OnMouseUp()
    {
        gameObject.transform.position = _startPoint.position;
        _currentLineRenderer.positionCount = 0;
        _currentPoint = 0;
    }

    public void OnMouseDrag()
    {
        Vector2 _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (_isDrawing && Mathf.Abs(_lastMousePosition.magnitude - _mousePosition.magnitude) > _swipeOffset)
        {
            _lastMousePosition = _mousePosition;
            gameObject.transform.position = _mousePosition - _offset;

            _currentLineRenderer.positionCount++;

            int positionIndex = _currentLineRenderer.positionCount - 1;
            _currentLineRenderer.SetPosition(positionIndex, gameObject.transform.position);

            float currentDistance = Vector2.Distance(gameObject.transform.position, _points[_currentPoint].position);

            if (currentDistance <= _pointOffset)
            {
                _currentPoint++;

                if (_currentPoint < _points.Length)
                {
                    _lastDistance = Vector2.Distance(gameObject.transform.position, _points[_currentPoint].position);
                    currentDistance = _lastDistance;
                }

                else
                {
                    FinishDraw();
                }
            }

            else
            {
                if (_lastDistance - currentDistance < _backOffset)
                {
                    OnMouseUp();
                    _isDrawing = false;
                }

                else
                {
                    _lastDistance = currentDistance;
                }
            }
        }
    }

    private void FinishDraw()
    {
        gameObject.SetActive(false);
        GetComponentInParent<DrawMiniGameComponent>().FinishDraw();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isDrawing = false;
        gameObject.transform.position = _startPoint.position;
        _currentLineRenderer.positionCount = 0;
        OnMouseUp();
    }
}
