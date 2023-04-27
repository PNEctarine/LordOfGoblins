using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ResourcesLevel
{
    [Tooltip("Стоимость улучшения")] public long Cost;
}

[CreateAssetMenu(menuName = "Config/ResourcesLevelConfig")]
public class ResourcesLevelConfig : ScriptableObject
{
    public ResourcesLevel[] ResourcesLevels;
}
