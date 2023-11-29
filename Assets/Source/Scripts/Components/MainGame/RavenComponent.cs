using Code.Enums;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class RavenComponent : MonoBehaviour
{
    [SerializeField] private GameObject _flyingRaven;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private SkeletonAnimation _skeletonAnimation;

    private Vector3 _flyingRavenStartPosition;
    private BoxCollider2D _boxCollider2D;

    private const string _idleState = "Idle";
    private const string _landingState = "Landing";
    private const string _takeOfState = "Takeoff";

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _flyingRavenStartPosition = _flyingRaven.transform.position;
        _skeletonAnimation.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        GameEvents.EnableBooster_E?.Invoke();
    }

    public void Enable()
    {
        AudioManager.Instance.PlayAudio(AudioTypes.RavenFlight);

        _skeletonAnimation.gameObject.SetActive(false);

        _flyingRaven.transform.position = _flyingRavenStartPosition;
        _flyingRaven.SetActive(true);
        _flyingRaven.transform.DOMove(_targetPoint.position, 2f).OnComplete(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.RavenLanding);

            _boxCollider2D.enabled = true;

            _flyingRaven.SetActive(false);
            _flyingRaven.transform.position = Vector3.zero;

            _skeletonAnimation.gameObject.SetActive(true);
            _skeletonAnimation.state.ClearTracks();

            _skeletonAnimation.state.AddAnimation(0, _landingState, false, 0f);
            _skeletonAnimation.state.AddAnimation(0, _idleState, true, 0f);
        });
    }

    public void Disable()
    {
        AudioManager.Instance.PlayAudio(AudioTypes.RavenFlight);

        _skeletonAnimation.state.AddAnimation(0, _takeOfState, false, 0f);
        _boxCollider2D.enabled = false;
    }
}
