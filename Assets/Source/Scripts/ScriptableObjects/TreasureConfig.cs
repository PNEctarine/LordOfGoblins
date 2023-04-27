using System;
using UnityEngine;

[Serializable]
public struct Treasure
{
    [Tooltip("Стоимость продажи")] public int[] Cost;
    [Tooltip("Визуалы сокровища")] public Sprite[] Visual;
    [Tooltip("Эффекты сокровища")] public Sprite[] Glow;
}

[CreateAssetMenu(menuName = "Config/TreasureConfig")]
public class TreasureConfig : ScriptableObject
{
    [Tooltip("Элементы сокровища")] public Treasure[] Treasures;
}
