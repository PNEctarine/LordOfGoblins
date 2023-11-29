using Code.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPUpgradeCardUIComponent : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _costTMP;
    [SerializeField] private IAPUpgradeTypes _IAPUpgrades;
    [SerializeField] private int quantity;

    [SerializeField] private IAPButton attachedButton;

    private void Start()
    {
        //_buyButton.onClick.AddListener(() => GameEvents.BuyIAPUpgrades_E(_IAPUpgrades, quantity));

        Product product = CodelessIAPStoreListener.Instance.GetProduct(attachedButton.productId);
        _costTMP.SetText(product.metadata.localizedPriceString);
    }

    public void Purchase()
    {
        GameEvents.BuyIAPUpgrades_E(_IAPUpgrades, quantity);
    }
}
