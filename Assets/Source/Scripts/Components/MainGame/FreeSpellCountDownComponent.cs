using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using InternetCheck;
using InternetTime;
using UnityEngine;

public class FreeSpellCountDownComponent : MonoBehaviour
{
    [SerializeField] private SpellComponent _spellComponent;
    private InternetAccessComponent _internetAccessComponent;

    private float _timer;
    private TimeSpan _currentTime;

    private void Start()
    {
        GameEvents.FreeSpellCountDown_E += SpellCountDown;
        _internetAccessComponent = FindObjectOfType<InternetAccessComponent>();

        StartCoroutine(_internetAccessComponent.CoroutineTestConnection(result =>
        {
            if (result && PlayerPrefs.HasKey("FreeSpellDate"))
            {
                SetDate();
            }
        }));
    }

    private async void SetDate()
    {
        DateTime currentTime = await InternetTimeManager.DateTimeCheck();

        string nextDay = PlayerPrefs.GetString("FreeSpellDate", "00/00/00 00:00:00");

        DateTime dateTime = Convert.ToDateTime(nextDay);
        TimeSpan timeSpan = dateTime - currentTime;

        _timer = PlayerPrefs.GetFloat("Timer", 0);
        _timer = (float)timeSpan.TotalSeconds;

        if (_timer > 0)
        {
            StartCoroutine(CountDown());
        }
    }

    private async void SpellCountDown()
    {
        DateTime currentTime = await InternetTimeManager.DateTimeCheck();
        Debug.Log("START DATE: " + currentTime.ToString());

        DateTime nextDay = currentTime.AddDays(1);
        PlayerPrefs.SetString("FreeSpellDate", nextDay.ToString());

        _currentTime = nextDay - currentTime;
        _timer = (float)_currentTime.TotalSeconds;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (_timer > 0)
        {
            yield return new WaitForFixedUpdate();
            _timer -= Time.deltaTime;
            _spellComponent.Set(_timer);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.SetFloat("Timer", _timer);
    }
}
