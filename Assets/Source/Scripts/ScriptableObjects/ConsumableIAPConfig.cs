using System;
using UnityEngine;

[Serializable]
public struct ConsumableInfo
{
    [Tooltip("ID")] public string ID;
    [Tooltip("ID для Google Play")] public string GoogleID;
    [Tooltip("ID для App Store")] public string IosID;

    [Tooltip("Получаемое количество")] public double Quantity;
}

[CreateAssetMenu(menuName = "Config/ConsumableIAPConfig")]
public class ConsumableIAPConfig : ScriptableObject
{
    public ConsumableInfo[] ConsumableInfo;
}
