using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SellPointLockComponent : MonoBehaviour
{
    //[SerializeField] private int _skin;
    [SerializeField] private SkeletonAnimation _lock;
    [SerializeField] private ParticleSystem _VFX;

    private const string _animationName = "Open";

    public void SetLockSkin(int skin)
    {
        _lock.initialSkinName = skin.ToString();
        _lock.Initialize(true);
    }

    public void OpenLock()
    {
        _lock.state.AddAnimation(0, _animationName, false, 0f).Complete += OnSpineAnimationComplete;
        _VFX.transform.parent = gameObject.transform.parent;
        _VFX.Play();
    }

    public void OnSpineAnimationComplete(TrackEntry trackEntry)
    {
        gameObject.SetActive(false);
        _lock.state.Complete -= OnSpineAnimationComplete;
    }
}
