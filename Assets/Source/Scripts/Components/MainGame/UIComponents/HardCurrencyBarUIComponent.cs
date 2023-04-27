using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HardCurrencyBarUIComponent : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private Button _addGems;

    private void Start()
    {
        _addGems.onClick.AddListener(() =>
        {
            ShopComponent shopComponent = _gameUI.ShopComponent;
            _gameUI.OpenWindow(shopComponent.gameObject, _gameUI.ShopButton.gameObject, true, Vector2.zero);
            shopComponent.OpenTab(shopComponent.BuyHardCurrencyTab, shopComponent.HardCurrencyeButton);
        });
    }
}
