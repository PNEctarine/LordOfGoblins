using UnityEngine;

public class TargetCupComponent : MonoBehaviour
{
    [SerializeField] private int _minCount;
    private int _count;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LiquidEffectLayer"))
        {
            _count++;
            Debug.Log(_count);
            if (_count >= _minCount)
            {
                Debug.Log("Win");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LiquidEffectLayer"))
        {
            _count--;
            Debug.Log(_count);
        }
    }
}
