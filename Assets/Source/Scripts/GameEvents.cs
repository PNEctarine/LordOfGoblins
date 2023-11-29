using System;
using Code.Enums;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action TutorialNextStage_E;
    public static Action TutorialMerge_E;
    public static Action TutorialCompleate_E;

    public static Action GatesAnimation_E;
    public static Action CartGoblinLayer_E;

    public static Action<int> CheckStagePath_E;
    public static Action<bool> ThumbColliderSwitch_E;
    public static Action SetStagePath_E;
    public static Action CloseWindow_E;
    public static Action WindowClosed_E;
    public static Action NotEnoughGold_E;
    public static Action LocationMap_E;

    public static Action FreeSpellCountDown_E;
    public static Action CheckResource_E;
    public static Action RestoreRaidChance_E;
    public static Action EnableBooster_E;
    public static Action CheapestImprovements_E;

    public static Action NewSquad_E;
    public static Action<int> RewardFromRaids_E;
    public static Action<int> RollbackOfRaids_E;
    public static Action<string> RaidResult_E;

    public static Action CatchHobbit_E;
    public static Action<bool> CouplesPhysics_E;

    public static Action<bool> SpellButtonsInteractable_E;
    public static Action<bool> Subscripe_E;
    public static Action<long> AddCoins_E;
    public static Action<long> CheapestUpdateCost_E;
    public static Action<int> AddHardCurrency_E;
    public static Action<float> CollectorBoost_E;
    public static Action<string> SpawnResource_E;
    public static Action<float, bool> CoinsBoost_E;
    public static Action<int, int, SpellTypes> BuySpell_E;
    public static Action<int, float> BuyWaitingIsBoring_E;
    public static Action<IAPUpgradeTypes, int> BuyIAPUpgrades_E;
    public static Action<int, SpellTypes> SpellPickUp_E;
    public static Action<SpellTypes, int, Sprite> SpellActivate_E;
    public static Action<bool, bool, SquadTypes> StartRaid_E;
    public static Action<UpdateTypes, UpdateCardUIComponent, bool> BuyUpgrade_E;
    public static Action GoblinCollectorUpgrade;
    public static Action<long> CheckCost_E;
    public static Action<SquadComponent> BuySquad_E;
    public static Action<OnboardingTasks, OnboardingTaskType[]> CheckTask_E;

    public static void ClearEvents()
    {
        TutorialNextStage_E = null;
        TutorialMerge_E = null;
        TutorialCompleate_E = null;

        GatesAnimation_E = null;
        CartGoblinLayer_E = null;

        CheckStagePath_E = null;
        ThumbColliderSwitch_E = null;
        SetStagePath_E = null;
        CloseWindow_E = null;
        WindowClosed_E = null;
        NotEnoughGold_E = null;
        LocationMap_E = null;

        FreeSpellCountDown_E = null;
        CheckResource_E = null;
        RestoreRaidChance_E = null;
        EnableBooster_E = null;
        CheapestImprovements_E = null;

        NewSquad_E = null;
        RewardFromRaids_E = null;
        RollbackOfRaids_E = null;
        RaidResult_E = null;

        CatchHobbit_E = null;
        CouplesPhysics_E = null;

        SpellButtonsInteractable_E = null;
        Subscripe_E = null;
        AddCoins_E = null;
        CheapestUpdateCost_E = null;
        AddHardCurrency_E = null;
        CollectorBoost_E = null;
        SpawnResource_E = null;
        CoinsBoost_E = null;
        BuySpell_E = null;
        BuyWaitingIsBoring_E = null;
        BuyIAPUpgrades_E = null;
        SpellPickUp_E = null;
        SpellActivate_E = null;
        StartRaid_E = null;
        BuyUpgrade_E = null;
        GoblinCollectorUpgrade = null;
        CheckCost_E = null;
        BuySquad_E = null;
        CheckTask_E = null;
    }
}
