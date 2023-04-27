using System;
using UnityEngine;

[Serializable]
public struct Resource
{
    [Tooltip("Название ресурса")] public string ResourceName;
    [Tooltip("Уровень ресурса")] public int Level;
    [Tooltip("Стоимость продажи")] public int[] Cost;
    [Tooltip("Визуалы ресурса")] public Sprite[] Visuals;
    [Tooltip("Свечения ресурса")] public Sprite[] Glow;
}

[CreateAssetMenu(menuName = "Config/ResoucesConfig")]
public class ResourcesConfig : ScriptableObject
{
    [Tooltip("Элементы магазина")] public Resource[] Resource;
}
