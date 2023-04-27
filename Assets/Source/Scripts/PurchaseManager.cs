using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    private int currentProductIndex;

    [Tooltip("Не многоразовые товары. Больше подходит для отключения рекламы и т.п.")]
    public string[] NC_PRODUCTS;
    [SerializeField] private ConsumableIAPConfig _consumableIAPConfig;
    [SerializeField] private SubscriptionIAPConfig _subscriptionIAPConfig;

    public static event OnSuccessPurchase OnPurchaseSuccess;
    public static event OnFailedPurchase PurchaseFailed;

    private void Awake()
    {
        InitializePurchasing();
        StandardPurchasingModule.Instance().useFakeStoreAlways = true;
    }
    /// <summary>
    /// Проверить, куплен ли товар.
    /// </summary>
    /// <param name="id">Индекс товара в списке.</param>
    /// <returns></returns>
    public static bool CheckBuyState(string id)
    {
        if (m_StoreController != null)
        {
            Product product = m_StoreController.products.WithID(id);
            if (product.hasReceipt) { return true; }
            else { return false; }
        }

        else
            return false;
    }

    public void InitializePurchasing()
    {
        StandardPurchasingModule standardPurchasingModule = StandardPurchasingModule.Instance();
        var builder = ConfigurationBuilder.Instance(standardPurchasingModule);
        standardPurchasingModule.useFakeStoreAlways = true;

        foreach (ConsumableInfo productInfo in _consumableIAPConfig.ConsumableInfo)
        {
            builder.AddProduct(productInfo.ID, ProductType.Consumable,
                new IDs { { productInfo.GoogleID, GooglePlay.Name }, { productInfo.IosID, AppleAppStore.Name } },
                new PayoutDefinition(PayoutType.Currency.ToString(), productInfo.Quantity));
        }

        foreach (string s in NC_PRODUCTS)
        {
            builder.AddProduct(s, ProductType.NonConsumable);
        }

        foreach (SubscriptionInfo productInfo in _subscriptionIAPConfig.SubscriptionInfo)
        {
            builder.AddProduct(productInfo.ID, ProductType.Subscription,
                new IDs { { productInfo.GoogleID, GooglePlay.Name }, { productInfo.IosID, AppleAppStore.Name } },
                new PayoutDefinition(PayoutType.Currency.ToString(), productInfo.Quantity));
        }

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(_consumableIAPConfig.ConsumableInfo[index].ID);
    }

    public void BuyNonConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(NC_PRODUCTS[index]);
    }

    public void BuySubscription(int index)
    {
        currentProductIndex = index;
        BuyProductID(_subscriptionIAPConfig.SubscriptionInfo[index].ID);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }

            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
            }
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        ConsumableInfo consumableInfo = Array.Find(_consumableIAPConfig.ConsumableInfo, info => info.ID == args.purchasedProduct.definition.id);

        if (_consumableIAPConfig.ConsumableInfo.Length > 0 && consumableInfo.ID == args.purchasedProduct.definition.id)
        {
            int quantity = (int)Math.Round(args.purchasedProduct.definition.payout.quantity);
            GameEvents.AddHardCurrency_E?.Invoke(quantity);
            OnSuccess(args);
        }

        else if (NC_PRODUCTS.Length > 0 && string.Equals(args.purchasedProduct.definition.id, NC_PRODUCTS[currentProductIndex], StringComparison.Ordinal))
        {
            OnSuccess(args);
        }

        else if (_subscriptionIAPConfig.SubscriptionInfo.Length > 0 && string.Equals(args.purchasedProduct.definition.id, _subscriptionIAPConfig.SubscriptionInfo[currentProductIndex].ID, StringComparison.Ordinal))
        {
            GameEvents.Subscripe_E?.Invoke(false);
            Debug.Log("SUBCRIBED");
            OnSuccess(args);
        }


        else Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        {
            return PurchaseProcessingResult.Complete;
        }
    }

    public delegate void OnSuccessPurchase(PurchaseEventArgs args);
    protected virtual void OnSuccess(PurchaseEventArgs args)
    {
        if (OnPurchaseSuccess != null) OnPurchaseSuccess(args);
        Debug.Log(args.purchasedProduct.definition.id + " Buyed!");
    }
    //public delegate void OnSuccessNonConsumable(PurchaseEventArgs args);
    //protected virtual void OnSuccessNC(PurchaseEventArgs args)
    //{
    //    if (OnPurchaseNonConsumable != null) OnPurchaseNonConsumable(args);
    //    Debug.Log(NC_PRODUCTS[currentProductIndex] + " Buyed!");
    //}
    public delegate void OnFailedPurchase(Product product, PurchaseFailureReason failureReason);
    protected virtual void OnFailedP(Product product, PurchaseFailureReason failureReason)
    {
        if (PurchaseFailed != null) PurchaseFailed(product, failureReason);
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        OnFailedP(product, failureReason);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}