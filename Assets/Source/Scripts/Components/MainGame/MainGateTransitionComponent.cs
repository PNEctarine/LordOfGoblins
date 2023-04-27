using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;

public class MainGateTransitionComponent : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _gateAnimation;
    public GameUI GameUI;

    private const string _open = "open_gate";
    private const string _close = "close_gate";

    private void Start()
    {
        GameEvents.LocationMap_E += OpenGate;
    }

    public void OpenGate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -9);
        gameObject.transform.DOScale(1, 0.5f).OnComplete(() =>
            {
                _gateAnimation.AnimationState.SetAnimation(0, _close, false).Complete += OpenAnimationComplete;
                StartCoroutine(Delay());
            });
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.3f);
        GameUI.OpenWindow(GameUI.LocationsUIComponent.gameObject, GameUI.LocationsButton.gameObject, false, Vector3.zero);
    }

    private void OpenAnimationComplete(TrackEntry trackEntry)
    {
        _gateAnimation.AnimationState.SetAnimation(0, _open, false).Complete += CloseAnimationComplete;
    }

    private void CloseAnimationComplete(TrackEntry trackEntry)
    {
        gameObject.transform.DOScale(13, 0.5f).OnComplete(() =>
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -11);
        });
    }
}
