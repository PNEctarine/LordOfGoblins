using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarUIComponent : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite _pressed;
    [SerializeField] private Sprite _unPressed;
    [SerializeField] private Image _image;

    public void OnDrag(PointerEventData eventData)
    {
        _image.sprite = _pressed;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = _pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = _unPressed;
    }
}
