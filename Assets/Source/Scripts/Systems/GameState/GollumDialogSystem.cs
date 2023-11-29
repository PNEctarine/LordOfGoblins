using Kuhpik;
using UnityEngine;

public class GollumDialogSystem : GameSystemWithScreen<DialogUI>
{
    [SerializeField] private DialogsConfig _dialogsConfig;

    public override void OnStateExit()
    {
        player.IsDialogue = false;
        player.DialogStage++;
    }

    public override void OnStateEnter()
    {
        screen.GollumUIComponent.Dialogue(_dialogsConfig.State[player.DialogStage].Dialogs, player.DialogStage, _dialogsConfig.State[player.DialogStage].Actions);
        player.IsDialogue = true;
    }
}
