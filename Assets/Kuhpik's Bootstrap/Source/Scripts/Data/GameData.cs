using System;
using UnityEngine;
using System.Collections.Generic;

namespace Kuhpik
{
    /// <summary>
    /// Used to store game data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class GameData
    {
        public int LastResourceIndex;

        public List<GameObject> GoblinsCollectors;
        public List<GoblinMovementComponent> GoblinMovementComponents;

        public GoblinWithCartComponent GoblinWithCartComponent;
        public LevelComponent LevelComponent;
        public Transform[] SellPoints;
        public Transform[] CartWaypoints;
        public ResourceBlockComponent[] ResourceBlockComponents;
        public ResourceComponent[] ResourceComponents;
        // Example (I use public fields for data, but u free to use properties\methods etc)
        // public float LevelProgress;
        // public Enemy[] Enemies;
    }
}