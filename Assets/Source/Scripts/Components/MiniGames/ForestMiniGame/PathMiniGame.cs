using UnityEngine;

public class PathMiniGame : MonoBehaviour
{
    public void ScanPath()
    {
        AstarPath.active.Scan();
    }
}
