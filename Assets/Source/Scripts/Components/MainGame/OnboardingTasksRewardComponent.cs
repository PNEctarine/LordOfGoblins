using System.Collections;
using UnityEngine;

public class OnboardingTasksRewardComponent : MonoBehaviour
{
    [SerializeField] private OnboardingCurrencyComponent[] _softCurrencyComponents;
    [SerializeField] private OnboardingCurrencyComponent[] _hardCurrencyComponents;

    [SerializeField] private Transform _softCurrencyTarget;
    [SerializeField] private Transform _hardCurrencyTarget;

    private Vector3 _startPos;
    private Vector3[] _movingVector = new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(-1, 1, 0), new Vector3(-1, -1, 0) };

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

    private void Start()
    {
        _startPos = _softCurrencyComponents[0].transform.position;

        for (int i = 0; i < _softCurrencyComponents.Length; i++)
        {
            _softCurrencyComponents[i].gameObject.SetActive(false);
            _hardCurrencyComponents[i].gameObject.SetActive(false);
        }
    }

    public void SoftCurrencyAnimate()
    {
        StartCoroutine(Delay(_softCurrencyComponents, _softCurrencyTarget));
    }

    public void HardCurrencyAnimate()
    {
        StartCoroutine(Delay(_hardCurrencyComponents, _hardCurrencyTarget));
    }

    private IEnumerator Delay(OnboardingCurrencyComponent[] onboardingCurrencyComponent, Transform currencyTarget)
    {
        for (int i = 0; i < _softCurrencyComponents.Length; i++)
        {
            yield return _waitForSeconds;

            onboardingCurrencyComponent[i].transform.position = _startPos;
            onboardingCurrencyComponent[i].Play(_movingVector[i] * 0.5f, currencyTarget.position);
        }
    }
}
