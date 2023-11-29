using System;
using Code.Enums;
using UnityEngine;

[Serializable]
public struct Spell
{
    public SpellTypes SpellTypes;
    public int Percent;
    public float Duration;
}

[CreateAssetMenu(menuName = "Config/SpellsConfig")]
public class SpellsConfig : ScriptableObject
{
    public Spell[] Spell;
}
