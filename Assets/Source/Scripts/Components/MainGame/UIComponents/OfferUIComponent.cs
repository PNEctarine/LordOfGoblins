using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using UnityEngine;

public class OfferUIComponent : MonoBehaviour
{
    public Vector2 CrossPosition { get; private set; }
    [SerializeField] private RectTransform _crossPosition;

    private void Awake()
    {
        CrossPosition = _crossPosition.position;
        gameObject.SetActive(false);
    }

    public void Purchase()
    {
        GameEvents.AddHardCurrency_E?.Invoke(200);
        GameEvents.BuyIAPUpgrades_E?.Invoke(IAPUpgradeTypes.ADCoupon, 10);
    }
}
