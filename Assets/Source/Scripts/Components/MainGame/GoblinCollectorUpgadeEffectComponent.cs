using System.Collections;
using UnityEngine;

public class GoblinCollectorUpgadeEffectComponent : MonoBehaviour
{
    [SerializeField] private GameObject _upgrageVFX;
    private bool _isEffect;

    private void Start()
    {
        GameEvents.WindowClosed_E += PlayEffect;
        GameEvents.GoblinCollectorUpgrade += CheckUpgrade;

        _upgrageVFX.SetActive(false);
    }

    private void CheckUpgrade()
    {
        _isEffect = true;
    }

    private void PlayEffect()
    {
        if (_isEffect)
        {
            StartCoroutine(EffectDelay());
        }
    }

    private IEnumerator EffectDelay()
    {
        yield return new WaitForSeconds(1);

        _upgrageVFX.SetActive(true);
        _isEffect = false;
    }
}
