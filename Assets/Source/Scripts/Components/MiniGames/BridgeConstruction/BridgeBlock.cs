using UnityEngine;

public class BridgeBlock : MonoBehaviour
{
    [SerializeField] private int Index;

    private Vector2 _offset;
    private LineRenderer _currentLineRenderer;
    private Camera _mainCamera;

    private bool _bool;
    private bool _isUp;

    private void Awake()
    {
        _currentLineRenderer = gameObject.GetComponent<LineRenderer>();
        _mainCamera = Camera.main;
    }

    public void OnMouseUp()
    {
        _isUp = true;
    }

    public void OnMouseDown()
    {
        _offset = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _isUp = false;
    }

    public void OnMouseDrag()
    {
        if (_bool == false)
        {
            Vector2 _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = _mousePosition - _offset;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ConstructionPoint constructionPoint) && _bool == false && _isUp && constructionPoint.Index == Index)
        {
            _bool = true;

            gameObject.transform.SetPositionAndRotation(collision.transform.position, collision.transform.rotation);
            gameObject.transform.parent = collision.transform;

            collision.enabled = false;
        }
    }
}
