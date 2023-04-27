using System.Collections.Generic;
using Code.Enums;
using Kuhpik;

public class IAPUpgradesSystem : GameSystemWithScreen<GameUI>
{
    public override void OnInit()
    {
        GameEvents.BuyIAPUpgrades_E += BuyIAPUpgrade;

        player.IAPUpgrades ??= new Dictionary<IAPUpgradeTypes, int>
        {
            { IAPUpgradeTypes.ADCoupon, 0 },
        };
    }

    private void BuyIAPUpgrade(IAPUpgradeTypes upgradeType, int quantity)
    {
        if (player.IAPUpgrades.ContainsKey(upgradeType))
        {
            player.IAPUpgrades[upgradeType] += quantity;
        }

        else if (upgradeType == IAPUpgradeTypes.HardCurrency)
        {
            GameEvents.AddHardCurrency_E?.Invoke(quantity);
        }
    }
}
