using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class IncreaseUpdatesCost : MonoBehaviour
{
    public static long IncreasedCost(long cost, int locationLevel)
    {
        long finalCost = cost;
        int[] levelUpdateRise = Bootstrap.Instance.PlayerData.LevelUpdatesRise.ToArray();

        for (int i = 0; i <= locationLevel; i++)
        {
            finalCost += finalCost * levelUpdateRise[i] / 100;
        }

        return finalCost;
    }
}
