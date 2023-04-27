using Kuhpik;

public class OpenLocationSysem : GameSystemWithScreen<GameUI>
{
    public override void OnInit()
    {
        for (int i = 0; i < player.UpgradesCounter.Count; i++)
        {
            if (player.LocationLevel == i)
            {
                screen.LocationsUIComponent.CloseCurrentLocation(player.LocationLevel);
            }

            if (player.UpgradesToOpen.Count > i + 1 && player.UpgradesCounter[i] >= player.UpgradesToOpen[i + 1])
            {
                screen.LocationsUIComponent.OpenLocation(i + 1, false);
            }
        }

        if (player.DialogStage >= 1)
            screen.LocationsButton.gameObject.SetActive(true);

        else
            screen.LocationsButton.gameObject.SetActive(false);
    }

    public void OpenLocation()
    {
        if (player.LocationLevel < player.UpgradesToOpen.Count - 1 && player.UpgradesCounter[player.LocationLevel] >= player.UpgradesToOpen[player.LocationLevel + 1])
        {
            screen.LocationsUIComponent.OpenLocation(player.LocationLevel + 1, true);
        }
    }
}
