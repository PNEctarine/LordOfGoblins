using System;
using UnityEngine;

[Serializable]
public struct MergePointsUpdates
{
    [Tooltip("Стоимость улучшения")] public long Cost;
}

[CreateAssetMenu(menuName = "Config/IncreasingMergePointsConfig")]
public class IncreasingMergePointsConfig : ScriptableObject
{
    public MergePointsUpdates[] MergePointsUpdates;
}
