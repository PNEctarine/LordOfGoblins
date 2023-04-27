using System.Collections;
using System.Reflection;
using Code.Enums;
using Kuhpik;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationsUIComponent : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private SkeletonGraphic[] _locks;
    [SerializeField] private GameObject[] _info;

    private const string _open = "Open";

    private int _index;
    private bool _isOpen;

    private void Awake()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            int closureIndex = i;
            _buttons[i].interactable = i == 0;
            _buttons[i].onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
                ChangeLocation(closureIndex);
            });
        }

        gameObject.SetActive(false);
    }

    public void OpenLocation(int index, bool isAnimate)
    {
        _index = index;
        _buttons[index].enabled = true;
        _buttons[index].interactable = true;

        if (isAnimate)
            _isOpen = true;

        else
            _info[_index - 1].SetActive(false);
    }

    public void CloseCurrentLocation(int index)
    {
        _buttons[index].enabled = false;
        //_info[index].gameObject.SetActive(true);
        //_info[index].text = "Current";
    }

    private void ChangeLocation(int index)
    {
        Bootstrap bootstrap = Bootstrap.Instance;
        GameEvents.ClearEvents();

        bootstrap.PlayerData.LocationLevel = index;
        bootstrap.GameRestart(0);
    }

    private void OnEnable()
    {
        if (_isOpen)
        {
            _isOpen = false;
            StartCoroutine(Delay());
        }
    }


    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        _locks[_index - 1].AnimationState.SetAnimation(0, _open, false).Complete += LocksAnimationComplete;
    }

    private void LocksAnimationComplete(TrackEntry trackEntry)
    {
        _info[_index - 1].SetActive(false);
    }
}
