using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GoblinSpeedUp
{
    [Tooltip("Множитель")] public float Speed; // Less is faster
    [Tooltip("Стоимость улучшения")] public long Cost;
}

[CreateAssetMenu(menuName = "Config/GoblinSpeedUpConfig")]
public class GoblinSpeedUpConfig : ScriptableObject
{
    public GoblinSpeedUp[] GoblinSpeedUp; 
}
