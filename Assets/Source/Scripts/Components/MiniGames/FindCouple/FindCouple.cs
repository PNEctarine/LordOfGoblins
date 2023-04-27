using UnityEngine;

public class FindCouple : MonoBehaviour
{
    public int Objects;

    private bool _isGrab;
    private Couple _firstObject;

    public void Couple(Couple couple)
    {
        if (_isGrab)
        {
            GameEvents.CouplesPhysics_E?.Invoke(true);

            if (couple.Index == _firstObject.Index)
            {
                _firstObject.gameObject.SetActive(false);
                couple.gameObject.SetActive(false);

                Objects -= 2;
            }

            else
            {
                _firstObject.gameObject.SetActive(true);
                _firstObject.IsGrab = false;
                _firstObject = null;
                couple.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
            }

            if (Objects <= 0)
            {
                Debug.Log("WIN");
            }

            _isGrab = false;
        }

        else
        {
            _isGrab = true;
            _firstObject = couple;
            _firstObject.IsGrab = true;
            _firstObject.gameObject.SetActive(false);

            GameEvents.CouplesPhysics_E?.Invoke(false);

            Debug.Log("GRAB");
        }
    }
}
