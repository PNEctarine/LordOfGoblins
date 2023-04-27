using System;
using Code.Enums;
using UnityEngine;

[Serializable]
public struct CastleLevel
{
    [Tooltip("Стоимость открытия уровня")] public int Cost;

    [Tooltip("Тип уровня (скорость сборки, скорость поставки, новый ресурс, шанс сокровища, вместимость хранилища, стоимость продажи)")]
    public TypesOfCastleLevels LevelType;

}

[CreateAssetMenu(menuName = "Config/CastleLevelsConfig")]
public class CastleLevelsConfig : ScriptableObject
{
    public CastleLevel[] CastleLevels;
}
