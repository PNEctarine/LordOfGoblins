using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using Kuhpik;
using UnityEngine;

public class TutorialSpellSystem : GameSystemWithScreen<TutorialUI>
{
    public override void OnInit()
    {
        if (player.IsNotFirstLaunch == false)
        {
            player.IsNotFirstLaunch = true;
            GameEvents.SpellPickUp_E?.Invoke(20, SpellTypes.IncreaseIncome);
        }
    }
}
