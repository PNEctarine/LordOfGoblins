using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GoblinsNumber
{
    [Tooltip("Длительность")] public int LevelToOpen;
    [Tooltip("Стоимость улучшения")] public long Cost;
}

[CreateAssetMenu(menuName = "Config/NumberOfGoblins")]
public class NumberOfGoblinsConfig : ScriptableObject
{
    public GoblinsNumber[] GoblinsNumbers;
}
