using System;
using System.Collections;
using Code.Enums;
using InternetCheck;
using Kuhpik;
using UnityEngine;

public class SquadSystem : GameSystemWithScreen<GameUI>
{
    private float _rollback;
    private float _timer;

    private bool _hasInternet;
    private bool _isRestored;

    private InternetAccessComponent _internetAccessComponent;

    public override void OnInit()
    {
        GameEvents.BuySquad_E += BuySquad;
        GameEvents.RollbackOfRaids_E += RollbackOfRaids;

        _internetAccessComponent = FindObjectOfType<InternetAccessComponent>();
        _isRestored = Convert.ToBoolean(PlayerPrefs.GetInt("RaidChanceRestore", 1));
        _rollback = PlayerPrefs.GetFloat("RaidRollback", 60 * 60 * 2);

        _internetAccessComponent.AsyncTestConnection(result =>
        {
            _hasInternet = result;
            for (int i = 0; i < player.SquadComponents.Count; i++)
            {
                if (result && PlayerPrefs.HasKey($"RaidDate{player.SquadComponents[i].SquadType}"))
                {
                    SetDate();
                }
            }
        });
    }

    private void BuySquad(SquadComponent squadComponent)
    {
        if (player.Gold[player.LocationLevel] >= squadComponent.Cost && _hasInternet)
        {
            player.Gold[player.LocationLevel] -= squadComponent.Cost;
            player.SquadComponents.Add(squadComponent);
            screen.SoftCurrencyBar.AddCoins(player.Gold[player.LocationLevel]);

            for (int i = 0; i < player.SquadComponents.Count; i++)
            {
                _timer = _rollback;

                DateTime currentTime = player.SquadComponents[i].InternetDateTime();
                Debug.Log("START DATE: " + currentTime.ToString());
                DateTime dueDate = currentTime.AddHours(2);
                PlayerPrefs.SetString($"RaidDate{player.SquadComponents[i].SquadType}", dueDate.ToString());

                if (_isRestored == true)
                {
                    _isRestored = false;
                    DateTime restoreDate = currentTime.AddDays(1);

                    PlayerPrefs.SetInt("RaidChanceRestore", Convert.ToInt32(_isRestored));
                    PlayerPrefs.SetString($"RaidRestoreDate", restoreDate.ToString());
                }

                GameEvents.StartRaid_E?.Invoke(true, false, squadComponent.SquadType);
                StartCoroutine(StartRaid(squadComponent));
            }
        }

        else if (_hasInternet == false)
        {
            squadComponent.NoInternet();

            _internetAccessComponent.AsyncTestConnection(result =>
            {
                _hasInternet = result;
            });
        }
    }

    private IEnumerator StartRaid(SquadComponent squadComponent)
    {
        float timer = _timer;

        while (timer > 0)
        {
            yield return new WaitForFixedUpdate();
            timer -= Time.deltaTime;
        }

        player.SquadComponents.Remove(squadComponent);
        FinishRaid(squadComponent.SquadType, squadComponent.CurrentChance, squadComponent.Reward);
    }

    private void FinishRaid(SquadTypes squadType, int chance, int reward)
    {
        int chanceRandom = UnityEngine.Random.Range(0, 100);

        if (chance >= chanceRandom)
        {
            player.Gold[player.LocationLevel] += reward;
            screen.SoftCurrencyBar.AddCoins(player.Gold[player.LocationLevel]);

            GameEvents.StartRaid_E?.Invoke(false, true, squadType);
            GameEvents.RaidResult_E?.Invoke($"You win! Reward: {reward}");
        }

        else
        {
            GameEvents.StartRaid_E?.Invoke(false, false, squadType);
            GameEvents.RaidResult_E?.Invoke($"You lose");
        }
    }

    private void RollbackOfRaids(int percent)
    {
        _rollback -= _rollback * percent / 100;
        PlayerPrefs.SetFloat("RaidRollback", _rollback);
    }

    private void SetDate()
    {
        for (int i = 0; i < player.SquadComponents.Count; i++)
        {
            DateTime currentTime = player.SquadComponents[i].InternetDateTime();

            string dueDate = PlayerPrefs.GetString($"RaidDate{player.SquadComponents[i].SquadType}", "00/00/00 00:00:00");
            string restoreDate = PlayerPrefs.GetString($"RaidRestoreDate", "00/00/00 00:00:00");

            DateTime dueDateTime = Convert.ToDateTime(dueDate);
            TimeSpan dueDatetimeSpan = dueDateTime - currentTime;

            DateTime restoreDateTime = Convert.ToDateTime(restoreDate);
            TimeSpan restoreDatetimeSpan = restoreDateTime - currentTime;

            _timer = (float)dueDatetimeSpan.TotalSeconds;
            float restoreTime = (float)restoreDatetimeSpan.TotalSeconds;

            if (_timer > 0)
            {
                GameEvents.StartRaid_E?.Invoke(true, false, player.SquadComponents[i].SquadType);
                StartCoroutine(StartRaid(player.SquadComponents[i]));
            }

            else if (player.SquadComponents[i].RaidCheck())
            {
                FinishRaid(player.SquadComponents[i].SquadType, player.SquadComponents[i].CurrentChance, player.SquadComponents[i].Reward);
            }

            if (restoreTime <= 0)
            {
                _isRestored = false;
                GameEvents.RestoreRaidChance_E?.Invoke();
            }
        }
    }
}
