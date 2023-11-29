using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrossButtonComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite _pressed;
    [SerializeField] private Sprite _unPressed;

    private GameObject _window;
    private Button _button;

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();
    }

    public void SetWindow(GameObject window)
    {
        _window = window;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            _button.image.sprite = _pressed;
            gameObject.transform.localScale = Vector3.one * 0.9f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            _button.image.sprite = _unPressed;
            gameObject.transform.localScale = Vector3.one;
            _window.SetActive(false);
        }
    }
}
