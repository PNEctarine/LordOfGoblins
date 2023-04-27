using System.Collections;
using Code.Enums;
using NaughtyAttributes;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class LocationComponent : MonoBehaviour
{
    [SerializeField] private bool _useLight;
    [SerializeField][ShowIf("_useLight")] private SkeletonAnimation _gatesLightAnimation;

    [SerializeField] private bool _useDust;
    [SerializeField][ShowIf("_useDust")] private SkeletonAnimation _gatesDustAnimation;

    [SerializeField] private SkeletonAnimation _gatesAnimation;

    private bool _isOpen;

    [SerializeField][ShowIf("useEffect")] private TextMeshProUGUI _currentEffectivenessTMP;
    [SerializeField][ShowIf("useEffect")] private TextMeshProUGUI _nextEffectivenessTMP;
    [SerializeField][ShowIf("useEffect")] private string _measurementUnit;

    private const string _open = "Open";
    private const string _close = "Close";
    private const string _idle = "Idle";

    private void Start()
    {
        GameEvents.GatesAnimation_E += GatesAction;

        LightAnimation(_open);

        _gatesAnimation.state.AddAnimation(0, _open, false, 0f);
        _isOpen = true;

        if (_useDust)
        {
            _gatesDustAnimation.gameObject.SetActive(false);
        }
    }

    private void GatesAction()
    {
        if (_isOpen)
        {
            StartCoroutine(CloseGate());
        }

        else
        {
            GameEvents.CartGoblinLayer_E?.Invoke();
            AudioManager.Instance.PlayAudio(AudioTypes.Gates);

            LightAnimation(_open);
            _gatesAnimation.state.AddAnimation(0, _open, false, 0f);
        }

        _isOpen = !_isOpen;
    }

    private void LightAnimation(string animation)
    {
        if (_useLight)
        {
            _gatesLightAnimation.AnimationName = animation;
        }
    }

    private IEnumerator CloseGate()
    {
        yield return new WaitForSeconds(5f);

        GameEvents.CartGoblinLayer_E?.Invoke();
        AudioManager.Instance.PlayAudio(AudioTypes.Gates);

        LightAnimation(_close);
        _gatesAnimation.state.AddAnimation(0, _close, false, 0f);

        if (_useDust)
        {
            _gatesDustAnimation.gameObject.SetActive(true);
            _gatesDustAnimation.state.AddAnimation(0, _close, false, 0);
        }
    }
}
