using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MergeBonus
{
    [Tooltip("Множитель")] public float Bonus;
    [Tooltip("Стоимость улучшения")] public long Cost;
}

[CreateAssetMenu(menuName = "Config/MergeBonusConfig")]
public class MergeBonusConfig : ScriptableObject
{
    public MergeBonus[] MergeBonus;
}
