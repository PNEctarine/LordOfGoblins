using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private int _stage;
    [SerializeField] private GameObject _enemy;

    private UnitComponent _unitComponent;
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

    public void Action(UnitComponent unitComponent)
    {
        _unitComponent = unitComponent;
        StartCoroutine(StartAction());
    }

    private IEnumerator StartAction()
    {
        yield return new WaitForSeconds(1f);
        _isOpen = true;
        _enemy.SetActive(false);
        _unitComponent.NextStage();
    }
}
