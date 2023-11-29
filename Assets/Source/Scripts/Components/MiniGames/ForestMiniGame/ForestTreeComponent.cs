using DG.Tweening;
using UnityEngine;

public class ForestTreeComponent : MonoBehaviour
{
    [SerializeField] private int _health;

    private void OnMouseDown()
    {
        _health--;
        gameObject.transform.DOPunchRotation(Vector3.one * 10f, 1f);

        if (_health <= 0)
        {
            gameObject.transform.DOKill();
            Destroy(gameObject);
            AstarPath.active.Scan();
        }
    }
}
