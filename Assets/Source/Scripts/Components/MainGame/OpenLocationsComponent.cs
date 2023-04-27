using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;

public class OpenLocationsComponent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;
    public GameUI GameUI;

    private const string _open = "open_gate";
    private const string _close = "close_gate";

    private void Start()
    {
        _vfx.gameObject.SetActive(false);
        GameEvents.LocationMap_E += OpenGate;
    }

    public void OpenGate()
    {
        _vfx.gameObject.SetActive(true);
        _vfx.Play();

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        GameUI.OpenWindow(GameUI.LocationsUIComponent.gameObject, GameUI.LocationsButton.gameObject, false, Vector3.zero);
    }
}
