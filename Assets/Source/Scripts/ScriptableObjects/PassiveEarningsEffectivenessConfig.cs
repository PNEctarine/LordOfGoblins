using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PassiveEarningEffectiveness
{
    [Tooltip("Процент")] public int Percent;
    [Tooltip("Стоимость улучшения")] public long Cost;
    [Tooltip("Локация открытия")] public int LocationToOpen;
}

[CreateAssetMenu(menuName = "Config/PassiveEarningsEffectivenessConfig")]
public class PassiveEarningsEffectivenessConfig : ScriptableObject
{
    public PassiveEarningEffectiveness[] PassiveEarningEffectivenesses;
}
