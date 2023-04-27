using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PassiveEarning
{
    [Tooltip("Длительность")] public int TimeOfAction;
    [Tooltip("Стоимость улучшения")] public long Cost;
}

[CreateAssetMenu(menuName = "Config/PassiveEarningsDurationConfig")]
public class PassiveEarningsDurationConfig : ScriptableObject
{
    public PassiveEarning[] PassiveEarnings;
}
