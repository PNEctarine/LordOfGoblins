using UnityEngine;

public class TurningThumbsMiniGame : MonoBehaviour
{
    [SerializeField] private ThumbComponent[] _thumbComponents;
    [SerializeField] private int _winCount;

    public void CheckWin(int count)
    {
        _winCount += count;

        if (_winCount >= _thumbComponents.Length)
        {
            Debug.Log("WIN");
        }
    }
}
