using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ThimbleComponent : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private bool _hasBall;

    public void EnableCollider(bool enabled)
    {
        _boxCollider2D.enabled = enabled;
    }

    private void OnMouseDown()
    {
        gameObject.transform.DOMoveY(1.23f, 0.5f);

        if (_hasBall)
        {
            Debug.Log("WIN");
        }

        else
        {
            StartCoroutine(RestartGame());
        }
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponentInParent<ThimblesMiniGame>().RestartGame();
    }
}
