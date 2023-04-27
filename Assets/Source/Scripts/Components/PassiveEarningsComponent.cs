using System;
using System.Net.Http;
using Code.Enums;
using InternetCheck;
using InternetTime;
using Kuhpik;
using UnityEngine;

public class PassiveEarningsComponent : MonoBehaviour
{
    [SerializeField] private ResourcesConfig _resourcesConfig;
    [SerializeField] private PassiveEarningsUIComponent _passiveEarningsUIComponent;
    [SerializeField] private InternetAccessComponent _internetAccessComponent;

    private int _passiveEarning;

    public void Init(PassiveEarningsUIComponent passiveEarningsUIComponent)
    {
        PassiveEarningsComponent[] obj = FindObjectsOfType<PassiveEarningsComponent>();

        if (obj.Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            _internetAccessComponent = FindObjectOfType<InternetAccessComponent>();
            _passiveEarningsUIComponent = passiveEarningsUIComponent;
            _passiveEarningsUIComponent.TakeButton.onClick.AddListener(() =>
            {
                if (_passiveEarning > 0)
                {
                    GameEvents.AddCoins_E(_passiveEarning);
                }

                AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
                _passiveEarningsUIComponent.gameObject.SetActive(false);
            });

            OnApplicationPause(false);
        }

        DontDestroyOnLoad(gameObject);

    }

    private async void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            InternetAccessComponent.Instance.AsyncTestConnection(async result =>
            {
                if (result && PlayerPrefs.HasKey("ExitTime"))
                {
                    PlayerData playerData = Bootstrap.Instance.PlayerData;

                    int resourceLevel = playerData.ResurcesSpawnLevel[playerData.LocationLevel];
                    float collectorSpeed = playerData.CollectorSpeed;
                    float timeOfAction = playerData.PassiveEarningTime;
                    float effectiveness = playerData.PassiveEarningsEffectiveness;

                    string time = PlayerPrefs.GetString("ExitTime", "00/00/00 00:00:00");

                    DateTime exitTime = Convert.ToDateTime(time);
                    DateTime currentTime = await InternetTimeManager.DateTimeCheck();

                    TimeSpan timeSpan = currentTime - exitTime;

                    if (timeSpan >= TimeSpan.FromMinutes(5))
                    {
                        int resourcesCost = 0;

                        for (int i = resourceLevel; i < resourceLevel + 4; i++)
                        {
                            resourcesCost += _resourcesConfig.Resource[i].Cost[0];
                        }

                        resourcesCost /= 4;
                        float totalEarning;
                        int averageCost = resourcesCost * 10;
                        int passiveEarning = 0;

                        if ((float)timeSpan.TotalSeconds > 0 && (float)timeSpan.TotalHours <= timeOfAction)
                        {
                            totalEarning = (float)timeSpan.TotalMinutes * averageCost / (collectorSpeed * 10);
                            passiveEarning = Mathf.RoundToInt(totalEarning * effectiveness / 100);
                        }

                        else if ((float)timeSpan.TotalSeconds > 0 && (float)timeSpan.TotalHours > timeOfAction)
                        {
                            totalEarning = timeOfAction * 60 * 60 * averageCost / (collectorSpeed * 10);
                            passiveEarning = Mathf.RoundToInt(totalEarning * effectiveness / 100);
                        }

                        _passiveEarning = passiveEarning;
                        _passiveEarningsUIComponent.gameObject.SetActive(true);
                        _passiveEarningsUIComponent.SetInfo("Overlord, while you were gone, the goblins collected gold for you", _passiveEarning, true);
                    }
                }

                else if (result == false)
                {
                    _passiveEarningsUIComponent.gameObject.SetActive(true);
                    _passiveEarningsUIComponent.SetInfo("Overlord, connect to the network to get all the gold that the goblins have collected for you!", 0, false);
                }
            });
        }

        else
        {
            PlayerPrefs.SetString("ExitTime", $"{await InternetTimeManager.DateTimeCheck()}");
        }
    }
}
