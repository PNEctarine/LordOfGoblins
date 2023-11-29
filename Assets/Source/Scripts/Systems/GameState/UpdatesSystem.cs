using Code.Enums;
using Kuhpik;
using UnityEngine;

public class UpdatesSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private PassiveEarningsDurationConfig _passiveEarningsDurationConfig;
    [SerializeField] private PassiveEarningsEffectivenessConfig _passiveEarningsEffectivenessConfig;
    [SerializeField] private TreasureConfig _treasureConfig;
    [SerializeField] private ResourcesLevelConfig _resourcesLevelConfig;
    [SerializeField] private GoblinSpeedUpConfig _goblinSpeedUpConfig;
    [SerializeField] private LuckySaleChanceConfig _luckySaleChanceConfig;
    [SerializeField] private LuckySaleIncomeConfig _luckySaleIncomeConfig;
    [SerializeField] private MergeBonusConfig _mergeBonusConfig;
    [SerializeField] private IncreasingMergePointsConfig _increasingMergePointsConfig;
    [SerializeField] private CollectorCapacityConfig _collectorCapacityConfig;
    [SerializeField] private InstantSaleConfig _instantSaleConfig;

    [Space(10)]
    [SerializeField] private UpgradesCountSystem _upgradesCountSystem;

    private bool _isBuy;
    private UpdateCardUIComponent _updateCardUIComponent;

    public override void OnInit()
    {
        GameEvents.BuyUpgrade_E += BuyUpdrade;
        screen.ShopComponent.SetUp();

        GameEvents.CheckCost_E?.Invoke(player.Gold[player.LocationLevel]);
    }

    private void BuyUpdrade(UpdateTypes updateType, UpdateCardUIComponent updateCardUIComponent, bool isBuy)
    {
        _isBuy = isBuy;
        _updateCardUIComponent = updateCardUIComponent;

        if (isBuy)
            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.BuyUpgrades, new OnboardingTaskType[] { OnboardingTaskType.None });

        switch (updateType)
        {
            case UpdateTypes.PassiveEarningsDurationUpdate:
                PassiveEarningsDurationUpdate();
                break;

            case UpdateTypes.PassiveEarningsEffectivenessUpdate:
                PassiveEarningsEffectivenessUpdate();
                break;

            case UpdateTypes.NewResource:
                NewResource();
                break;

            case UpdateTypes.CollectorSpeedUp:
                CollectorSpeedUp();
                break;

            case UpdateTypes.LuckySaleChance:
                LuckySaleChance();
                break;

            case UpdateTypes.LuckySaleIncome:
                LuckySaleIncome();
                break;

            case UpdateTypes.MergeBonus:
                MergeBonus();
                break;

            case UpdateTypes.IncreasingMergePoints:
                IncreasingMergePoints();
                break;

            case UpdateTypes.CollectorCapacity:
                CollectorCapacity();
                break;

            case UpdateTypes.InstantSale:
                InstantSale();
                Debug.Log($"Type of update: {updateType}");
                break;

            default:
                Debug.Log("TYPE ERROR!");
                break;
        }
    }

    private void PassiveEarningsDurationUpdate()
    {
        if (_passiveEarningsDurationConfig.PassiveEarnings.Length > player.PassiveEarningsDurationLevel[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel] + 1].Cost, player.LocationLevel)
            && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.PassiveEarningsDurationLevel[player.LocationLevel]++;
            player.PassiveEarningTime = _passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel]].TimeOfAction;

            if (_passiveEarningsDurationConfig.PassiveEarnings.Length > player.PassiveEarningsDurationLevel[player.LocationLevel] + 1)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel]].Cost, player.LocationLevel),
                _passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel]].TimeOfAction,
                _passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel] + 1].TimeOfAction);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.BuyUpgrades, new OnboardingTaskType[] { OnboardingTaskType.PassiveEarningsUpgrade });
            GameEvents.AddCoins_E?.Invoke(-_passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
        }

        else if (_passiveEarningsDurationConfig.PassiveEarnings.Length - 1 <= player.PassiveEarningsDurationLevel[player.LocationLevel])
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
                _passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel]].TimeOfAction,
                _passiveEarningsDurationConfig.PassiveEarnings[player.PassiveEarningsDurationLevel[player.LocationLevel] + 1].TimeOfAction);
        }
    }

    private void PassiveEarningsEffectivenessUpdate()
    {
        if (_passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses.Length > player.PassiveEarningsEffectivenessLevel[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel] + 1].Cost, player.LocationLevel)
            && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.PassiveEarningsEffectivenessLevel[player.LocationLevel]++;
            player.PassiveEarningsEffectiveness = _passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel]].Percent;

            if (_passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses.Length > player.PassiveEarningsEffectivenessLevel[player.LocationLevel] + 1
                && _passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel] + 1].LocationToOpen - 1 <= player.LocationLevel)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel] + 1].Cost, player.LocationLevel));
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.BuyUpgrades, new OnboardingTaskType[] { OnboardingTaskType.PassiveEarningsUpgrade });
            GameEvents.AddCoins_E?.Invoke(-_passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
        }

        else if (_passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses.Length - 1 <= player.PassiveEarningsEffectivenessLevel[player.LocationLevel]
            || _passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel] + 1].LocationToOpen - 1 <= player.LocationLevel)
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_passiveEarningsEffectivenessConfig.PassiveEarningEffectivenesses[player.PassiveEarningsEffectivenessLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);
            _updateCardUIComponent.SetInfo(finalCost);
        }
    }

    private void NewResource()
    {
        if (_resourcesLevelConfig.ResourcesLevels.Length > player.ResurcesSpawnLevel[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_resourcesLevelConfig.ResourcesLevels[player.ResurcesSpawnLevel[player.LocationLevel] + 1].Cost, player.LocationLevel)
            && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.ResurcesSpawnLevel[player.LocationLevel]++;
            player.TreasureLevel[player.LocationLevel]++;
            int cost = 0;

            for (int lines = 0; lines < 3; lines++)
            {
                for (int i = 0; i < game.ResourceBlockComponents.Length; i++)
                {
                    if (game.ResourceBlockComponents[i].ResourceLevel < player.ResurcesSpawnLevel[player.LocationLevel] * 4 + 4
                        && game.ResourceBlockComponents[i].ResourceComponent != null)
                    {
                        int resourceMergeLevel = game.ResourceBlockComponents[i].ResourceComponent.MergeLevel;
                        cost += game.ResourceBlockComponents[i].ResourceComponent.Cost[resourceMergeLevel];
                        game.ResourceBlockComponents[i].IsResource = false;
                        Destroy(game.ResourceBlockComponents[i].ResourceComponent.gameObject);
                    }
                }
            }

            if (_resourcesLevelConfig.ResourcesLevels.Length > player.ResurcesSpawnLevel[player.LocationLevel] + 1)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_resourcesLevelConfig.ResourcesLevels[player.ResurcesSpawnLevel[player.LocationLevel] + 1].Cost,
                    player.LocationLevel), player.ResurcesSpawnLevel[player.LocationLevel] + 2);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.SpawnResource_E?.Invoke(player.ResurcesSpawnLevel.ToString());
            GameEvents.AddCoins_E?.Invoke(cost);
            GameEvents.AddCoins_E?.Invoke(-_resourcesLevelConfig.ResourcesLevels[player.ResurcesSpawnLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
        }

        else if (_resourcesLevelConfig.ResourcesLevels.Length - 1 <= player.ResurcesSpawnLevel[player.LocationLevel])
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_resourcesLevelConfig.ResourcesLevels[player.ResurcesSpawnLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);
            _updateCardUIComponent.SetInfo(finalCost, player.ResurcesSpawnLevel[player.LocationLevel] + 1, player.ResurcesSpawnLevel[player.LocationLevel] + 2);
        }
    }

    private void CollectorSpeedUp()
    {
        if (_goblinSpeedUpConfig.GoblinSpeedUp.Length > player.GoblinSpeedLevel[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel] + 1].Cost, player.LocationLevel)
            && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.GoblinSpeedLevel[player.LocationLevel]++;
            player.CollectorSpeed = _goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel]].Speed;

            if (_goblinSpeedUpConfig.GoblinSpeedUp.Length > player.GoblinSpeedLevel[player.LocationLevel] + 1)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel] + 1].Cost, player.LocationLevel),
                _goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel]].Speed,
                _goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel] + 1].Speed);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.BuyUpgrades, new OnboardingTaskType[] { OnboardingTaskType.SpeedUpgrade });
            GameEvents.AddCoins_E?.Invoke(-_goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
            GameEvents.GoblinCollectorUpgrade?.Invoke();
        }

        else if (_goblinSpeedUpConfig.GoblinSpeedUp.Length - 1 <= player.GoblinSpeedLevel[player.LocationLevel])
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
                _goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel]].Speed,
                _goblinSpeedUpConfig.GoblinSpeedUp[player.GoblinSpeedLevel[player.LocationLevel] + 1].Speed);
        }
    }

    private void LuckySaleChance()
    {
        if (_luckySaleChanceConfig.LuckySaleChances.Length > player.LuckySaleChanceLevel[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel] + 1].Cost, player.LocationLevel) && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.LuckySaleChanceLevel[player.LocationLevel]++;
            player.LuckySaleChance = _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel]].Chance;

            if (_luckySaleChanceConfig.LuckySaleChances.Length > player.LuckySaleChanceLevel[player.LocationLevel] + 1
                && _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel] + 1].LocationToOpen - 1 <= player.LocationLevel)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel] + 1].Cost, player.LocationLevel),
               _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel]].Chance,
               _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel] + 1].Chance);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.BuyUpgrades, new OnboardingTaskType[] { OnboardingTaskType.LuckySaleUpgrade });
            GameEvents.AddCoins_E?.Invoke(-_luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
            GameEvents.GoblinCollectorUpgrade?.Invoke();
        }

        else if (_luckySaleChanceConfig.LuckySaleChances.Length - 1 <= player.LuckySaleChanceLevel[player.LocationLevel] ||
            _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel] + 1].LocationToOpen - 1 <= player.LocationLevel)
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
               _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel]].Chance,
               _luckySaleChanceConfig.LuckySaleChances[player.LuckySaleChanceLevel[player.LocationLevel] + 1].Chance);
        }
    }

    private void LuckySaleIncome()
    {
        if (_luckySaleIncomeConfig.LuckySaleIncomes.Length > player.LuckySaleIncomeLevel[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel] + 1].Cost, player.LocationLevel) && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.LuckySaleIncomeLevel[player.LocationLevel]++;
            player.LuckySaleIncome = _luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel]].IncomeMultiplier;

            if (_luckySaleIncomeConfig.LuckySaleIncomes.Length > player.LuckySaleIncomeLevel[player.LocationLevel] + 1)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel] + 1].Cost, player.LocationLevel),
                _luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel]].IncomeMultiplier,
                _luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel] + 1].IncomeMultiplier);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.BuyUpgrades, new OnboardingTaskType[] { OnboardingTaskType.LuckySaleUpgrade });
            GameEvents.AddCoins_E?.Invoke(-_luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
            GameEvents.GoblinCollectorUpgrade?.Invoke();
        }

        else if (_luckySaleIncomeConfig.LuckySaleIncomes.Length - 1 <= player.LuckySaleIncomeLevel[player.LocationLevel])
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
                _luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel]].IncomeMultiplier,
                _luckySaleIncomeConfig.LuckySaleIncomes[player.LuckySaleIncomeLevel[player.LocationLevel] + 1].IncomeMultiplier);
        }
    }

    private void MergeBonus()
    {
        if (_mergeBonusConfig.MergeBonus.Length > player.MergeBonusLevel[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel] + 1].Cost, player.LocationLevel) && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.MergeBonusLevel[player.LocationLevel]++;
            player.MergeBonus = _mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel]].Bonus;

            if (_mergeBonusConfig.MergeBonus.Length > player.MergeBonusLevel[player.LocationLevel] + 1)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel] + 1].Cost, player.LocationLevel),
                _mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel]].Bonus,
                _mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel] + 1].Bonus);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.AddCoins_E?.Invoke(-_mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
            GameEvents.GoblinCollectorUpgrade?.Invoke();
        }

        else if (_mergeBonusConfig.MergeBonus.Length - 1 <= player.MergeBonusLevel[player.LocationLevel])
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
                _mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel]].Bonus,
                _mergeBonusConfig.MergeBonus[player.MergeBonusLevel[player.LocationLevel] + 1].Bonus);
        }
    }

    private void IncreasingMergePoints()
    {
        if (_increasingMergePointsConfig.MergePointsUpdates.Length - 1 >= player.MergePointsLevel[player.LocationLevel]
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_increasingMergePointsConfig.MergePointsUpdates[player.MergePointsLevel[player.LocationLevel]].Cost, player.LocationLevel) && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.MergePointsLevel[player.LocationLevel]++;
            game.LevelComponent.MergePointsComponent.AddPlace(game.LevelComponent.MergePointsComponent.LockComponents.Length - player.MergePointsLevel[player.LocationLevel]);

            if (_increasingMergePointsConfig.MergePointsUpdates.Length - 1 >= player.MergePointsLevel[player.LocationLevel])
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_increasingMergePointsConfig.MergePointsUpdates[player.MergePointsLevel[player.LocationLevel]].Cost, player.LocationLevel),
                    player.MergePointsLevel[player.LocationLevel],
                    player.MergePointsLevel[player.LocationLevel] + 1);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.BuyUpgrades, new OnboardingTaskType[] { OnboardingTaskType.OpenLock });
            GameEvents.AddCoins_E?.Invoke(-_increasingMergePointsConfig.MergePointsUpdates[player.MergePointsLevel[player.LocationLevel] - 1].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
        }

        else if (_increasingMergePointsConfig.MergePointsUpdates.Length - 1 < player.MergePointsLevel[player.LocationLevel])
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_increasingMergePointsConfig.MergePointsUpdates[player.MergePointsLevel[player.LocationLevel]].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
                player.MergePointsLevel[player.LocationLevel],
                player.MergePointsLevel[player.LocationLevel] + 1);
        }
    }

    private void CollectorCapacity()
    {
        if (_collectorCapacityConfig.GoblinCapacity.Length > player.CollectorCapacity[player.LocationLevel] + 1
            && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_collectorCapacityConfig.GoblinCapacity[player.CollectorCapacity[player.LocationLevel] + 1].Cost, player.LocationLevel)
            && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.CollectorCapacity[player.LocationLevel]++;
            game.GoblinsCollectors[0].GetComponent<GoblinCollectorComponent>().MaxResources = player.CollectorCapacity[player.LocationLevel] + 1;

            if (_collectorCapacityConfig.GoblinCapacity.Length > player.CollectorCapacity[player.LocationLevel] + 1
                && _collectorCapacityConfig.GoblinCapacity[player.CollectorCapacity[player.LocationLevel] + 1].LocationToOpen - 1 <= player.LocationLevel)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_collectorCapacityConfig.GoblinCapacity[player.CollectorCapacity[player.LocationLevel] + 1].Cost, player.LocationLevel),
                    player.CollectorCapacity[player.LocationLevel] + 1,
                    player.CollectorCapacity[player.LocationLevel] + 2);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.AddCoins_E?.Invoke(-_collectorCapacityConfig.GoblinCapacity[player.CollectorCapacity[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
            GameEvents.GoblinCollectorUpgrade?.Invoke();
        }

        else if (_collectorCapacityConfig.GoblinCapacity.Length - 1 <= player.CollectorCapacity[player.LocationLevel]
            || _collectorCapacityConfig.GoblinCapacity[player.CollectorCapacity[player.LocationLevel] + 1].LocationToOpen - 1 <= player.LocationLevel)
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_collectorCapacityConfig.GoblinCapacity[player.CollectorCapacity[player.LocationLevel] + 1].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
                player.CollectorCapacity[player.LocationLevel] + 1,
                player.CollectorCapacity[player.LocationLevel] + 2);
        }
    }

    private void InstantSale()
    {
        if (_instantSaleConfig.InstantSales.Length > player.InstantSaleLevel[player.LocationLevel] + 1
           && player.Gold[player.LocationLevel] >= IncreaseUpdatesCost.IncreasedCost(_instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel] + 1].Cost, player.LocationLevel) && _isBuy)
        {
            _upgradesCountSystem.UpgradesCount();
            player.InstantSaleLevel[player.LocationLevel]++;

            if (_instantSaleConfig.InstantSales.Length > player.InstantSaleLevel[player.LocationLevel] + 1 && _instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel] + 1].LocationToOpen - 1 <= player.LocationLevel)
            {
                _updateCardUIComponent.SetInfo(IncreaseUpdatesCost.IncreasedCost(_instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel] + 1].Cost, player.LocationLevel),
               _instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel]].Chance,
               _instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel] + 1].Chance);
            }

            else
            {
                _updateCardUIComponent.MaxLevel();
            }

            GameEvents.AddCoins_E?.Invoke(_instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel]].Cost);
            GameEvents.CheapestImprovements_E?.Invoke();
            GameEvents.GoblinCollectorUpgrade?.Invoke();
        }

        else if (_instantSaleConfig.InstantSales.Length - 1 <= player.InstantSaleLevel[player.LocationLevel] ||
            _instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel]].LocationToOpen - 1 > player.LocationLevel)
        {
            _updateCardUIComponent.MaxLevel();
        }

        else if (_isBuy == false)
        {
            long finalCost = IncreaseUpdatesCost.IncreasedCost(_instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel] + 1].Cost, player.LocationLevel);

            _updateCardUIComponent.SetInfo(finalCost,
                _instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel]].Chance,
                _instantSaleConfig.InstantSales[player.InstantSaleLevel[player.LocationLevel]].Chance + 1);
        }

    }
}