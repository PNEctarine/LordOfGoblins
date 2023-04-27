using DG.Tweening;
using UnityEngine;

public class RopeComponent : MonoBehaviour
{
    [SerializeField] private int _stage;

    [SerializeField] private float _scale;
    [SerializeField] private UnitComponent _unitComponent;

    private BoxCollider2D _boxCollider2D;
    private bool _isOpen;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        GameEvents.CheckStagePath_E += CheckStage;
    }

    private void CheckStage(int stage)
    {
        if (stage == _stage && _isOpen)
        {
            GameEvents.SetStagePath_E?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        _isOpen = true;
        _boxCollider2D.enabled = false;
        gameObject.transform.DOScaleY(_scale, 1f).OnComplete(() => _unitComponent.NextStage());
    }
}
