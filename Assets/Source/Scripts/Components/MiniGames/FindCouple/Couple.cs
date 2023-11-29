using UnityEngine;

public class Couple : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; }
    public bool IsGrab;
    private FindCouple _findCouple;

    private void Start()
    {
        GameEvents.CouplesPhysics_E += PhysicsSimulated;

        _findCouple = gameObject.GetComponentInParent<FindCouple>();
        _findCouple.Objects++;
    }

    private void PhysicsSimulated(bool isStatic)
    {
        if (!isStatic)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        else
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void OnMouseDown()
    {
        if (IsGrab == false)
        {
            _findCouple.Couple(this);
        }
    }
}
