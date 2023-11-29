using System;
using System.Collections;
using Code.Enums;
using Kuhpik;
using UnityEngine;

public class SpellsSystem : GameSystemWithScreen<GameUI>
{
    private int _incomeBoostPercent;
    private int _collectorBoostPercent;

    private float _countdown;
    private bool _isLoaded;

    public SpawnResourcesSystem SpawnResourcesSystem;
    private bool _isRandomCoupons;

    private const string _incomeBoostKey = "IncomeBoostPercent";
    private const string _collectorBoostKey = "CollectorBoostPercent";
    private const string _spellCountDownKey = "SpellCountDown";

    private void Start()
    {
        GameEvents.SpellActivate_E += SpellActivate;
    }

    public override void OnInit()
    {
        GameEvents.WindowClosed_E += ActivateCoupons;

        int incomePercent = PlayerPrefs.GetInt(_incomeBoostKey, 0);
        int collectorPercent = PlayerPrefs.GetInt(_collectorBoostKey, 0);

        _countdown = PlayerPrefs.GetFloat(_spellCountDownKey, 0);
        _isLoaded = true;

        if (_countdown <= 0)
        {
            GameEvents.SpellButtonsInteractable_E?.Invoke(true);
            screen.SpellCountDownComponent.gameObject.SetActive(false);
        }

        else if (_countdown > 0)
        {
            screen.SpellCountDownComponent.gameObject.SetActive(true);
        }

        if (incomePercent != 0)
        {
            IncreaseIncome(incomePercent);
        }

        if (collectorPercent != 0)
        {
            CollectorSpeedup(collectorPercent);
        }
    }

    private void SpellActivate(SpellTypes spellType, int percent, Sprite timerSprite)
    {
        if (timerSprite != null)
            player.SpellTimerSprite = timerSprite;

        switch (spellType)
        {
            case SpellTypes.RandomCoupons:
                RandomCoupons();
                break;

            case SpellTypes.IncreaseIncome:
                _countdown = 2 * 60 * 60;
                PlayerPrefs.SetFloat(_spellCountDownKey, _countdown);
                IncreaseIncome(percent); ;
                break;

            case SpellTypes.CollectorSpeedup:
                _countdown = 2 * 60 * 60;
                PlayerPrefs.SetFloat(_spellCountDownKey, _countdown);
                CollectorSpeedup(percent);
                break;

            case SpellTypes.IncreasesTreasureChance:
                _countdown = 2 * 60 * 60;
                PlayerPrefs.SetFloat(_spellCountDownKey, _countdown);
                CollectorSpeedup(percent);
                break;
        }
    }

    private void RandomCoupons()
    {
        _isRandomCoupons = true;
    }

    private void ActivateCoupons()
    {
        if (_isRandomCoupons)
        {
            SpawnResourcesSystem.SpawnCoupons();
            _isRandomCoupons = false;
        }
    }

    private void IncreaseIncome(int percent)
    {
        _incomeBoostPercent = percent;

        PlayerPrefs.SetInt(_incomeBoostKey, _incomeBoostPercent);
        GameEvents.CoinsBoost_E(_incomeBoostPercent, false);
        GameEvents.SpellButtonsInteractable_E?.Invoke(false);

        StartCoroutine(SpellCountdown(isEnd =>
        {
            if (isEnd)
            {
                GameEvents.CoinsBoost_E(-_incomeBoostPercent, false);
                PlayerPrefs.SetInt(_incomeBoostKey, 0);
            }
        }));
    }

    private void CollectorSpeedup(int percent)
    {
        _collectorBoostPercent += percent;

        PlayerPrefs.SetInt(_collectorBoostKey, _collectorBoostPercent);
        GameEvents.CollectorBoost_E?.Invoke(_collectorBoostPercent);
        GameEvents.SpellButtonsInteractable_E?.Invoke(false);

        StartCoroutine(SpellCountdown(isEnd =>
        {
            if (isEnd)
            {
                GameEvents.CollectorBoost_E?.Invoke(-_collectorBoostPercent);
                PlayerPrefs.SetInt(_collectorBoostKey, 0);
            }
        }));
    }

    private void IncreasesTreasureChance()
    {
        player.ChanceOfTreasure += 0.2f;
        StartCoroutine(SpellCountdown(isEnd =>
        {
            if (isEnd)
            {
                player.ChanceOfTreasure -= 0.2f;
            }
        }));
    }

    private IEnumerator SpellCountdown(Action<bool> callback)
    {
        while (_countdown > 0)
        {
            yield return new WaitForFixedUpdate();
            _countdown -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(_countdown);
            screen.SpellCountDownComponent.Timer.text = $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
        }

        callback(true);
        screen.SpellCountDownComponent.gameObject.SetActive(false);
        GameEvents.SpellButtonsInteractable_E?.Invoke(true);
    }

    private void OnApplicationPause(bool pause)
    {
        if (_isLoaded)
        {
            PlayerPrefs.SetFloat(_spellCountDownKey, _countdown);
            Bootstrap.Instance.SaveGame();
        }
    }
}
