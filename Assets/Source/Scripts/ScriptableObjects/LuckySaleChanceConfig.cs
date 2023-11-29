using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LuckySaleChance
{
    [Tooltip("Шанс")] public int Chance;
    [Tooltip("Стоимость улучшения")] public long Cost;
    [Tooltip("Локация открытия")] public int LocationToOpen;
}

[CreateAssetMenu(menuName = "Config/LuckySaleChanceConfig")]
public class LuckySaleChanceConfig : ScriptableObject
{
    public LuckySaleChance[] LuckySaleChances;
}