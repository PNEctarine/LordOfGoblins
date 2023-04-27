using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;
using UnityEngine.Device;

public class WaitingIsBoringSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private ResourcesConfig _resourcesConfig;

    public override void OnInit()
    {
        GameEvents.BuyWaitingIsBoring_E += BuyWaiting;
    }

    private void BuyWaiting(int cost, float multiplier)
    {
        if (player.HardCurrency >= cost)
        {
            player.HardCurrency -= cost;
            screen.AddHardCurrency(player.HardCurrency);

            int resourceLevel = player.ResurcesSpawnLevel[player.LocationLevel];
            float collectorSpeed = player.CollectorSpeed;
            float timeOfAction = player.PassiveEarningTime;
            float effectiveness = player.PassiveEarningsEffectiveness;

            int resourcesCost = 0;

            for (int i = resourceLevel; i < resourceLevel + 4; i++)
            {
                resourcesCost += _resourcesConfig.Resource[i].Cost[0];
            }

            resourcesCost /= 4;
            float totalEarning;
            int averageCost = resourcesCost * 10;
            int passiveEarning = 0;

            totalEarning = timeOfAction * 60 * 60 * averageCost / (collectorSpeed * 10);
            passiveEarning = Mathf.RoundToInt((totalEarning * effectiveness / 100) * multiplier);

            screen.CloseAll();
            screen.PassiveEarningsUIComponent.gameObject.SetActive(true);
            screen.PassiveEarningsUIComponent.SetInfo("Overlord, while you were gone, the goblins collected gold for you", passiveEarning, true);
        }
    }
}
