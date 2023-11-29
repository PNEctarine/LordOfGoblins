using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCheats : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.touchCount == 3)
        {
            GameEvents.AddCoins_E?.Invoke(10000);
        }
    }
}
