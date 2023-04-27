using UnityEngine;
using UnityEngine.UI;

public class ButtonClickMask : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float _alphaLevel = 1f;
    private Image _image;

    void Start()
    {
        _image = gameObject.GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = _alphaLevel;
    }
}
