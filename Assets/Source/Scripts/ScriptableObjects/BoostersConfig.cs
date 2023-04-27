using System;
using Code.Enums;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Booster
{
    [Tooltip("Ускорение в процентах")] public float Percent;
    [Tooltip("Время действия в минутах")] public float Duration;
    public Sprite BoosterSprite;

    [Tooltip("Тип бустера (ускорение сборщика, увеличение стоимости с продажи)")]
    public BoosterTypes BoosterType;

    public string Term()
    {
        string term = "";
        switch (BoosterType)
        {
            case BoosterTypes.CollectorSpeedUp:
                term = "IncreaseSpeed";
                break;

            case BoosterTypes.IncreaseCost:
                term = "IncreaseSale";
                break;
        }

        return term;
    }
}

[CreateAssetMenu(menuName = "Config/BoosterConfig")]
public class BoostersConfig : ScriptableObject
{
    [Tooltip("Бустеры")] public Booster[] Boosters;
}
