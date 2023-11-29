using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class BoostersLoadingSystem : GameSystem
{
    [SerializeField] private LocationsConfig _locationsConfig;
    [SerializeField] private LuckySaleIncomeConfig _luckySaleIncomeConfig;
    [SerializeField] private PassiveEarningsEffectivenessConfig _passiveEarningsEffectivenessConfig;
    [SerializeField] private MergeBonusConfig _mergeBonusConfig;
    [SerializeField] private PassiveEarningsDurationConfig _passiveEarningsDurationConfig;
    [SerializeField] private LuckySaleChanceConfig _luckySaleChanceConfig;
    [SerializeField] private InstantSaleConfig _instantSaleConfig;
    [SerializeField] private GoblinSpeedUpConfig _goblinSpeedUpConfig;

    public override void OnInit()
    {
        Application.targetFrameRate = 30;

        IninArrays();

        player.LuckySaleIncome = _luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel]].IncomeMultiplier;
        player.PassiveEarningsEffectiveness = _passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel]].Percent;
        player.MergeBonus = _mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel]].Bonus;
        player.PassiveEarningTime = _passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel]].TimeOfAction;
        player.LuckySaleChance = _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel]].Chance;
        player.InstantSaleChance = _instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel]].Chance;
        player.CollectorSpeed = _goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel]].Speed;
    }

    private void IninArrays()
    {
        int levelsCount = _locationsConfig.LevelComponents.Length;

        player.LevelIncomeBoost = new List<int> { 0, 20, 60, 100, 140 };
        player.LevelUpdatesRise = new List<int> { 0, 10, 30, 50, 90 };
        player.UpgradesToOpen = new List<int> { 0, 30, 40, 50, 50 };

        if (player.Gold != null && player.Gold.Count < levelsCount)
        {
            Add();
        }

        else if (player.Gold == null)
        {
            player.Gold = new List<long>();
            player.ResurcesSpawnLevel = new List<int>();
            player.UpgradesCounter = new List<int>();
            player.PassiveEarningsDurationLevel = new List<int>();
            player.PassiveEarningsEffectivenessLevel = new List<int>();
            player.LuckySaleChanceLevel = new List<int>();
            player.InstantSaleLevel = new List<int>();
            player.LuckySaleIncomeLevel = new List<int>();
            player.MergeBonusLevel = new List<int>();
            player.GoblinSpeedLevel = new List<int>();
            player.MergePointsLevel = new List<int>();
            player.TreasureLevel = new List<int>();
            player.CollectorCapacity = new List<int>();

            for (int i = 0; i < levelsCount; i++)
            {
                Add();
            }
        }
    }

    private void Add()
    {
        for (int i = player.Gold.Count; i < _locationsConfig.LevelComponents.Length; i++)
        {
            player.Gold.Add(0);
            player.ResurcesSpawnLevel.Add(0);
            player.UpgradesCounter.Add(0);
            player.PassiveEarningsDurationLevel.Add(0);
            player.PassiveEarningsEffectivenessLevel.Add(0);
            player.LuckySaleChanceLevel.Add(0);
            player.InstantSaleLevel.Add(0);
            player.LuckySaleIncomeLevel.Add(0);
            player.MergeBonusLevel.Add(0);
            player.GoblinSpeedLevel.Add(0);
            player.MergePointsLevel.Add(0);
            player.TreasureLevel.Add(0);
            player.CollectorCapacity.Add(0);
        }
    }
}
