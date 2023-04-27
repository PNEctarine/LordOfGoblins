using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InstantSale
{
    [Tooltip("Шанс выпадения")] public int Chance;
    [Tooltip("Стоимость улучшения")] public long Cost;
    [Tooltip("Локация открытия")] public int LocationToOpen;
}

[CreateAssetMenu(menuName = "Config/InstantSaleConfig")]
public class InstantSaleConfig : ScriptableObject
{
    public InstantSale[] InstantSales;
}
