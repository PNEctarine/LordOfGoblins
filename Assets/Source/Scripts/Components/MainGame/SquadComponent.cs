using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using Code.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadComponent : MonoBehaviour
{
    [field: SerializeField] public SquadTypes SquadType { get; private set; }

    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public int Reward { get; private set; }
    [SerializeField] private int _chance;

    public bool IsRaid { get; private set; }
    public int CurrentChance { get; private set; }

    [field: SerializeField] public TextMeshProUGUI CostTMP { get; private set; }
    [SerializeField] private TextMeshProUGUI _chanceTMP;
    [SerializeField] private TextMeshProUGUI _noInternetTMP;

    [SerializeField] private Button _button;

    private void Awake()
    {
        GameEvents.StartRaid_E += StartRaid;
        GameEvents.RestoreRaidChance_E += RestoringChance;
        GameEvents.RewardFromRaids_E += RewardFromRaids;

        CurrentChance = PlayerPrefs.GetInt($"RaidChance{SquadType}", _chance);


        CostTMP.text = Cost.ToString();
        _chanceTMP.text = $"{CurrentChance}%";
        _noInternetTMP.gameObject.SetActive(false);

        _button.onClick.AddListener(() => GameEvents.BuySquad_E?.Invoke(this));
    }

    public bool RaidCheck()
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt($"IsRaid{SquadType}", 0));
    }

    private void StartRaid(bool isRaid, bool isWin, SquadTypes squadType)
    {
        if (SquadType == squadType)
        {
            _button.interactable = !isRaid;
            IsRaid = isRaid;

            PlayerPrefs.SetInt($"IsRaid{SquadType}", Convert.ToInt32(IsRaid));

            if (isWin)
            {
                ChanceReducing(10);
            }
        }
    }

    private void ChanceReducing(int chanceReduce)
    {
        CurrentChance -= chanceReduce;
        _chanceTMP.text = $"{CurrentChance}%";

        PlayerPrefs.SetInt($"RaidChance{SquadType}", CurrentChance);
    }

    public DateTime InternetDateTime()
    {
        var client = new TcpClient("time.nist.gov", 13);

        StreamReader streamReader = new StreamReader(client.GetStream());

        string response = streamReader.ReadToEnd();
        string utcDateTimeString = response.Substring(7, 17);

        DateTime localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", null);
        return localDateTime;
    }

    public void OpenSquad()
    {
        _button.interactable = true;
    }

    private void RewardFromRaids(int percent)
    {
        Reward += Reward * percent / 100;
        PlayerPrefs.SetInt($"RaidReward{SquadType}", Reward);
    }

    private void RestoringChance()
    {
        CurrentChance = _chance;
        _chanceTMP.text = $"{CurrentChance}%";

        PlayerPrefs.SetInt($"RaidChance{SquadType}", CurrentChance);
    }

    public void NoInternet()
    {
        _noInternetTMP.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(NoInternetHide());
    }

    private IEnumerator NoInternetHide()
    {
        yield return new WaitForSeconds(3f);
        _noInternetTMP.gameObject.SetActive(false);
    }
}