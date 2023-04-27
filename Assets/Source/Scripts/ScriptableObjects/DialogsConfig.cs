using System;
using UnityEngine;

[Serializable]
public struct State
{
    public int Location;
    public int Upgrades;
    public int Actions;
    public string[] Dialogs;
}

[CreateAssetMenu(menuName = "Config/DialogsConfig")]
public class DialogsConfig : ScriptableObject
{
    public State[] State;
}
