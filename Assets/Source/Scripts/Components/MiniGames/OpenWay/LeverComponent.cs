using DG.Tweening;
using UnityEngine;

public class LeverComponent : MonoBehaviour
{
    [SerializeField] private int _stage;
    [SerializeField] private float _rotateAngle;
    [SerializeField] private UnitComponent _unitComponent;

    private bool _isOpen;

    private void Start()
    {
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
        gameObject.transform.DORotate(new Vector3(0, 0, gameObject.transform.localRotation.eulerAngles.z + _rotateAngle), 1f).OnComplete(() => AstarPath.active.Scan());
        _unitComponent.NextStage();
    }
}
