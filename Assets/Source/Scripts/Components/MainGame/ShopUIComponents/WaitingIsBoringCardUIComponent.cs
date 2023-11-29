using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingIsBoringCardUIComponent : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _costTMP;

    [SerializeField] private int _cost;
    [SerializeField] private float _multiplier;

    private void Start()
    {
        _costTMP.text = _cost.ToString();
        _buyButton.onClick.AddListener(() => GameEvents.BuyWaitingIsBoring_E?.Invoke(_cost, _multiplier));
    }
}
