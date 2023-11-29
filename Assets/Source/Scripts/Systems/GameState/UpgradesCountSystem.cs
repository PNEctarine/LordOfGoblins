using Code.Enums;
using Kuhpik;
using UnityEngine;

public class UpgradesCountSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private DialogsConfig _dialogsConfig;
    [SerializeField] private OpenLocationSysem _openLocationSysem;
    [SerializeField] private GollumDialogSystem _gollumDialogSystem;

    public override void OnInit()
    {
        //if (player.IsDialogue)
        //{
        //    screen.CloseAll();
        //    Bootstrap.Instance.ChangeGameState(GameStateID.Dialog);
        //}
    }

    public void UpgradesCount()
    {
        AudioManager.Instance.PlayAudio(AudioTypes.Upgrade);

        screen.ShopComponent.SuccessfulUpgrage();
        player.UpgradesCounter[player.LocationLevel]++;
        _openLocationSysem.OpenLocation();

        if (_dialogsConfig.State.Length > player.DialogStage && _dialogsConfig.State[player.DialogStage].Location == player.LocationLevel && _dialogsConfig.State[player.DialogStage].Upgrades == player.UpgradesCounter[player.LocationLevel])
        {
            screen.CloseAll();
            Bootstrap.Instance.ChangeGameState(GameStateID.Dialog);
        }
    }
}
