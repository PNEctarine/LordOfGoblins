using System.Collections.Generic;
using Code.Enums;
using Kuhpik;
using UnityEngine;

public class BuyingSpellsSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private SpellsConfig _spellsConfig;

    private void Start()
    {
        GameEvents.BuySpell_E += BuySpell;
    }

    private void BuySpell(int cost, int percent, SpellTypes spellType)
    {
        if (player.HardCurrency >= cost)
        {
            player.HardCurrency -= cost;
            screen.AddHardCurrency(player.HardCurrency);

            switch (spellType)
            {
                case SpellTypes.RandomCoupons:
                    GameEvents.SpellPickUp_E?.Invoke(percent, spellType);
                    break;

                case SpellTypes.IncreaseIncome:
                    GameEvents.SpellPickUp_E?.Invoke(percent, spellType);
                    break;

                case SpellTypes.CollectorSpeedup:
                    GameEvents.SpellPickUp_E?.Invoke(percent, spellType);
                    break;
            }
        }
    }

    private void RandomCoupons()
    {
        List<int> freeSpells = new List<int>();

        for (int i = 1; i < _spellsConfig.Spell.Length; i++)
        {
            freeSpells.Add(i);
        }

        for (int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, freeSpells.Count);

            switch (_spellsConfig.Spell[freeSpells[random]].SpellTypes)
            {
                case SpellTypes.IncreaseIncome:
                    GameEvents.SpellPickUp_E?.Invoke(_spellsConfig.Spell[freeSpells[random]].Percent, _spellsConfig.Spell[freeSpells[random]].SpellTypes);
                    freeSpells.Remove(freeSpells[random]);
                    continue;

                case SpellTypes.CollectorSpeedup:
                    GameEvents.SpellPickUp_E?.Invoke(_spellsConfig.Spell[freeSpells[random]].Percent, _spellsConfig.Spell[freeSpells[random]].SpellTypes);
                    freeSpells.Remove(freeSpells[random]);
                    continue;
            }
        }
    }
}
