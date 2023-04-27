using System.Collections;
using Code.Enums;
using Kuhpik;
using UnityEngine;
using UnityEngine.Device;

public class BoosterSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private BoostersConfig _boostersConfig;

    private int _boosterIndex;
    private float _timer;
    private float _countdown;
    private bool _isCooldown;
    private bool _isRewardRequested;

    public override void OnInit()
    {
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        GameEvents.EnableBooster_E += EnableBooster;

        screen.BoosterWindow.Button.onClick.AddListener(() =>
        {
            if (player.IAPUpgrades[IAPUpgradeTypes.ADCoupon] > 0)
            {
                player.IAPUpgrades[IAPUpgradeTypes.ADCoupon] -= 1;
                StartBooster();
            }

            else
            {
                _isRewardRequested = true;
                AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
                ApplovinSDK.Instance.ShowRewardedAd();
            }
        });

        _countdown = 6;
        _isCooldown = false;
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        if (_isRewardRequested)
        {
            _isRewardRequested = false;
            StartBooster();
        }
    }

    public override void OnUpdate()
    {
        if (_timer < 10f * 60 && _isCooldown == false)
        {
            _timer += 1 * Time.deltaTime;
        }

        else if (_timer >= 10f * 60 && _isCooldown == false)
        {
            game.LevelComponent.RavenComponent.gameObject.SetActive(true);
            game.LevelComponent.RavenComponent.Enable();

            _isCooldown = true;
            _timer = 0f;

            StartCoroutine(CountdownToLost());
        }
    }

    private void EnableBooster()
    {
        _boosterIndex = Random.Range(0, _boostersConfig.Boosters.Length);
        Booster booster = _boostersConfig.Boosters[_boosterIndex];

        screen.BoosterWindow.gameObject.SetActive(true);
        screen.BoosterWindow.SetBoosterInfo(booster.Percent, booster.Duration, booster.BoosterSprite, booster.Term());

        if (player.IAPUpgrades[IAPUpgradeTypes.ADCoupon] > 0)
            screen.BoosterWindow.ClaimTMP.text = $"Free {player.IAPUpgrades[IAPUpgradeTypes.ADCoupon]}/1";

        else
            screen.BoosterWindow.ClaimTMP.text = "Watch ad";

        game.LevelComponent.RavenComponent.gameObject.SetActive(false);

        StopAllCoroutines();
    }

    private void StartBooster()
    {
        _isCooldown = true;

        BoosterTypes boosterTypes = _boostersConfig.Boosters[_boosterIndex].BoosterType;

        switch (boosterTypes)
        {
            case BoosterTypes.CollectorSpeedUp:
                GameEvents.CollectorBoost_E?.Invoke(_boostersConfig.Boosters[_boosterIndex].Percent);
                break;

            case BoosterTypes.IncreaseCost:
                GameEvents.CoinsBoost_E?.Invoke(_boostersConfig.Boosters[_boosterIndex].Percent, false);
                break;

        }

        StartCoroutine(BoosterCountdown(_boosterIndex));
    }

    private IEnumerator CountdownToLost()
    {
        while (_countdown > 0)
        {
            yield return new WaitForFixedUpdate();
            _countdown -= Time.deltaTime;
        }

        _isCooldown = false;
        _countdown = 6;
        _timer = 0f;
        game.LevelComponent.RavenComponent.Disable();
    }

    private IEnumerator BoosterCountdown(int i)
    {
        yield return new WaitForSeconds(_boostersConfig.Boosters[i].Duration * 60);

        GameEvents.CollectorBoost_E?.Invoke(-_boostersConfig.Boosters[i].Percent);
        GameEvents.CoinsBoost_E?.Invoke(-_boostersConfig.Boosters[i].Percent, false);

        _isCooldown = false;
        _countdown = 5;
        _timer = 0f;
    }
}
