using System.Collections;
using Code.Enums;
using UnityEngine;

public class MergePointsComponent : MonoBehaviour
{
    [field: SerializeField] public SellPointLockComponent[] LockComponents { get; private set; }
    private int _places;
    private int _lastPlaces;
    private bool _isLaunch;

    private void Awake()
    {
        _isLaunch = true;
        GameEvents.WindowClosed_E += OpenLocks;
    }

    public void SetLockSkin(int skin)
    {
        for (int i = 0; i < LockComponents.Length; i++)
        {
            LockComponents[i].SetLockSkin(skin);
        }
    }

    public void AddPlace(int places)
    {
        _lastPlaces = _places;
        _places = places;
    }

    private void OpenLocks()
    {
        StartCoroutine(Open());
    }

    private IEnumerator Open()
    {
        yield return new WaitForSeconds(1f);

        if (_lastPlaces > _places || _isLaunch)
        {
            for (int i = LockComponents.Length - 1; i >= _places; i--)
            {
                LockComponents[i].OpenLock();
            }

            _lastPlaces = _places;
            _isLaunch = false;

            AudioManager.Instance.PlayAudio(AudioTypes.OpenLock);
            GameEvents.SpawnResource_E?.Invoke("");
        }
    }
}
