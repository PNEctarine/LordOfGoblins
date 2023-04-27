using UnityEngine;

public class DesiredObjectComponent : MonoBehaviour
{
    [SerializeField] private int _index;
    private FindObjectsMiniGame _findObjectsMiniGame;

    private void Awake()
    {
        _findObjectsMiniGame = GetComponentInParent<FindObjectsMiniGame>();
    }

    private void OnMouseDown()
    {
        _findObjectsMiniGame.MoveObject(gameObject.transform, _index);
    }
}
