using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotEnoughGoldUIComponent : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private ShopComponent _shopComponent;

    private void Awake()
    {
        GameEvents.NotEnoughGold_E += OpenWindow;

        _okButton.onClick.AddListener(() =>
        {
            _shopComponent.OpenTab(_shopComponent.HardCurrencyTab, _shopComponent.HardCurrencyeButton);
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }

    private void OpenWindow()
    {
        gameObject.SetActive(true);
    }
}
