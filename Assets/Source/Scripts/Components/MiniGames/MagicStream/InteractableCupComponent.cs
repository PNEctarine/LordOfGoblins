using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractableCupComponent : MonoBehaviour
{
    [SerializeField] private float _rotateAngle;
    private BoxCollider2D _boxCollider2D;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        _boxCollider2D.enabled = false;
        gameObject.transform.DORotate(new Vector3(0, 0, gameObject.transform.localRotation.eulerAngles.z + _rotateAngle), 1f);
    }
}
