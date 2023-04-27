using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LuckySaleIncome
{
    [Tooltip("Множитель")] public float IncomeMultiplier;
    [Tooltip("Стоимость улучшения")] public long Cost;
}

[CreateAssetMenu(menuName = "Config/LuckySaleIncomeConfig")]
public class LuckySaleIncomeConfig : ScriptableObject
{
    public LuckySaleIncome[] LuckySaleIncomes;
}
