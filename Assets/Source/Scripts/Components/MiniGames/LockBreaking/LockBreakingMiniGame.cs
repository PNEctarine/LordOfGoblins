using UnityEngine;

public class LockBreakingMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject[] _pins;

    private void Start()
    {
        int random = Random.Range(0, _pins.Length);

        _pins[random].transform.position = new Vector2(_pins[random].transform.position.x, _pins[random].transform.position.y + 0.5f);
        _pins[random].GetComponent<BoxCollider2D>().enabled = false;
    }
}
