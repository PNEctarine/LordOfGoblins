using UnityEngine;

public class MazePoint : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;

    private MiniGame _miniGame;

    private Vector2 _offset;
    private Camera _mainCamera;

    private bool _bool;

    private void Awake()
    {
        _miniGame = GetComponentInParent<MiniGame>();
        gameObject.transform.position = _startTransform.position;

        _mainCamera = Camera.main;
    }

    public void OnMouseDown()
    {
        _bool = true;
        _offset = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    public void OnMouseUp()
    {
        _miniGame.Fail();

        gameObject.transform.position = _startTransform.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void OnMouseDrag()
    {
        if (_bool)
        {
            Vector2 _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            gameObject.GetComponent<Rigidbody2D>().MovePosition(_mousePosition - _offset);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //_bool = false;
        //gameObject.transform.position = _startTransform.position;
        //OnMouseUp();
    }
}
