using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/LocationsConfig")]
public class LocationsConfig : ScriptableObject
{
    public LevelComponent[] LevelComponents;
}