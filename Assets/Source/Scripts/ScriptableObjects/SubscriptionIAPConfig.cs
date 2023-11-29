using System;
using UnityEngine;

[Serializable]
public struct SubscriptionInfo
{
    [Tooltip("ID")] public string ID;
    [Tooltip("ID для Google Play")] public string GoogleID;
    [Tooltip("ID для App Store")] public string IosID;

    [Tooltip("Получаемое количество")] public double Quantity;
}

[CreateAssetMenu(menuName = "Config/SubscriptionIAPConfig")]
public class SubscriptionIAPConfig : ScriptableObject
{
    public SubscriptionInfo[] SubscriptionInfo;
}
