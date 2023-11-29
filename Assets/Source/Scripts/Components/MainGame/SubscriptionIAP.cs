using UnityEngine;
using UnityEngine.UI;

public class SubscriptionIAP : MonoBehaviour
{
    [SerializeField] private int _index;
    private Button _purchaseButton;
    private PurchaseManager _purchaseManager;

    private void Awake()
    {
        _purchaseButton = GetComponent<Button>();
        _purchaseManager = FindObjectOfType<PurchaseManager>();

        _purchaseButton.onClick.AddListener(() => _purchaseManager.BuySubscription(_index));
    }
}
