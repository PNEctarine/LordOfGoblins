using System;
using Code.Enums;
using UnityEngine;

[Serializable]
public struct Level
{
    [Tooltip("Стоимость открытия уровня")] public int Cost;

    [Tooltip("Тип уровня (скорость сборки, скорость поставки, новый ресурс, шанс сокровища, вместимость хранилища, стоимость продажи)")]
    public TypesOfWarehouseLevels LevelType;
}

[CreateAssetMenu(menuName = "Config/WarehouseLevelConfig")]
public class WarehouceLevelsConfig : ScriptableObject
{
    [Tooltip("Уровни")] public Level[] Levels;
}
