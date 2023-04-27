using System;
using System.Collections.Generic;
using Code.Enums;
using UnityEngine;

namespace Kuhpik
{
    /// <summary>
    /// Used to store player's data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public List<SquadComponent> SquadComponents = new List<SquadComponent>();

        public List<long> Gold;
        public int HardCurrency;
        public int AdCoupons;

        public int OnboardingTaskLevel;
        public List<int> OnboardingTaskProgress;

        public int LocationLevel;
        public int DialogStage;

        public int NextLevelTotalGold;
        public int NextLevelCurrentGold;
        public List<int> ResurcesSpawnLevel;
        public List<int> TreasureLevel;
        public List<int> CollectorCapacity;
        public List<int> UpgradesCounter;
        public IDictionary<IAPUpgradeTypes, int> IAPUpgrades;

        public List<int> LevelIncomeBoost;
        public List<int> LevelUpdatesRise;
        public List<int> UpgradesToOpen;

        public List<int> PassiveEarningsDurationLevel;
        public List<int> PassiveEarningsEffectivenessLevel;
        public List<int> LuckySaleChanceLevel;
        public List<int> LuckySaleIncomeLevel;
        public List<int> MergeBonusLevel;
        public List<int> GoblinSpeedLevel;
        public List<int> MergePointsLevel;
        public List<int> InstantSaleLevel;

        public List<Quest> GeneratedQuest;
        public List<int> QuestDifficulty;

        public int LuckySaleChance;
        public int InstantSaleChance;
        public float CollectorSpeed; // Less is faster
        public float CartSpeed; // Less is faster
        public float ChanceOfTreasure;
        public float PassiveEarningTime; // In hours
        public float PassiveEarningsEffectiveness; // Persent
        public float LuckySaleIncome;
        public float MergeBonus;

        public bool IsDialogue;
        public bool IsTutorialCompelete;
        public bool IsNotFirstLaunch;
        public bool IsWarehouseMaxLevel;
        public bool IsCastleMaxLevel;
        public bool IsOffer;

        public Sprite SpellTimerSprite;

        // Example (I use public fields for data, but u free to use properties\methods etc)
        // [BoxGroup("level")] public int level;
        // [BoxGroup("currency")] public int money;
    }
}