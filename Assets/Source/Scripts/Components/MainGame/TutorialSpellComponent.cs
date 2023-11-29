using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using InternetCheck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSpellComponent : MonoBehaviour
{
    [field: SerializeField] public SpellTypes SpellTypes { get; private set; }

    [SerializeField] private int _cost;
    [SerializeField] private int _percent;
    [SerializeField] private TextMeshProUGUI _сostTMP;
    [SerializeField] private TextMeshProUGUI _noInternetTMP;

    private Button _button;
    private InternetAccessComponent _internetAccessComponent;

    private bool _isInternet;
    private float _timer;

    private void Awake()
    {
        _internetAccessComponent = FindObjectOfType<InternetAccessComponent>();
        _button = GetComponentInChildren<Button>();
        _noInternetTMP.gameObject.SetActive(false);

        if (_timer > 0)
        {
            _button.interactable = false;
        }

        if (_cost > 0)
        {
            _сostTMP.text = $"{_cost} HC";
        }

        else
        {
            _timer = PlayerPrefs.GetFloat("Timer", 0);
            _сostTMP.text = "FREE";
            _button.interactable = false;

            CheckInternet();
        }

        _button.onClick.AddListener(() =>
        {
            GameEvents.TutorialNextStage_E?.Invoke();
            GetSpell();
        });
    }

    private void OnEnable()
    {
        if (_cost <= 0)
        {
            CheckInternet();
        }
    }

    public void Set(float timer)
    {
        _timer = timer;
        TimeSpan timeSpan = TimeSpan.FromSeconds(_timer);
        _сostTMP.text = string.Format("{00}:{01}", timeSpan.Hours, timeSpan.Minutes);
        PlayerPrefs.SetFloat("Timer", _timer);
    }

    private void GetSpell()
    {
        if (_cost == 0 && _isInternet)
        {
            _timer = 24 * 3600;
            _button.interactable = false;

            GameEvents.FreeSpellCountDown_E?.Invoke();
            GameEvents.BuySpell_E?.Invoke(_cost, _percent, SpellTypes);
        }

        else if (_cost == 0 && _isInternet == false)
        {
            _noInternetTMP.gameObject.SetActive(true);
            StartCoroutine(NoInternet());
        }

        else if (_cost >= 0)
        {
            GameEvents.BuySpell_E?.Invoke(_cost, _percent, SpellTypes);
        }
    }

    public void CheckInternet()
    {
        if (_internetAccessComponent)
        {
            _internetAccessComponent.AsyncTestConnection(result =>
            {
                Debug.Log("Result: " + result);
                _isInternet = result;

                if (_timer <= 0 || result == false)
                {
                    _button.interactable = true;
                }
            });
        }
    }

    private IEnumerator NoInternet()
    {
        yield return new WaitForSeconds(2f);
        _noInternetTMP.gameObject.SetActive(false);
    }
}
