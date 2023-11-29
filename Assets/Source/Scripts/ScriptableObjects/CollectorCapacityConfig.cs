using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GoblinCapacity
{
    [Tooltip("Стоимость улучшения")] public long Cost;
    [Tooltip("Локация открытия")] public int LocationToOpen;
}

[CreateAssetMenu(menuName = "Config/CollectorCapacityConfig")]
public class CollectorCapacityConfig : ScriptableObject
{
    public GoblinCapacity[] GoblinCapacity;
}
