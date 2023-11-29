using Code.Enums;
using DG.Tweening;
using Kuhpik;
using UnityEngine;

public class CurrencySystem : GameSystemWithScreen<GameUI>
{
    private float _boost;
    private float _couponPercent;
    private long _cheapestUpdateCost;
    private bool _isCoupon;

    private Vector3 _coinTargetScale;

    private void Start()
    {
        GameEvents.CoinsBoost_E += CoinsBoost;
    }

    public override void OnInit()
    {
        GameEvents.AddCoins_E += AddCoins;
        GameEvents.AddHardCurrency_E += AddHardCurrency;
        GameEvents.CheapestUpdateCost_E += CheapestUpdate;

        _coinTargetScale = screen.SoftCurrencyBar.CoinsTarget.localScale;

        screen.SoftCurrencyBar.AddCoins(player.Gold[player.LocationLevel]);
        screen.AddHardCurrency(player.HardCurrency);
        FindObjectOfType<PassiveEarningsComponent>().Init(screen.PassiveEarningsUIComponent);
        //GameEvents.CheckCost_E?.Invoke(player.Gold[player.LocationLevel]);
    }

    private void AddCoins(long count)
    {
        screen.SoftCurrencyBar.DOKill();
        screen.SoftCurrencyBar.transform.localScale = _coinTargetScale;
        screen.SoftCurrencyBar.CoinsTarget.DOScale(screen.SoftCurrencyBar.transform.localScale * 1.5f, 0.1f).OnComplete(() =>
        {
            screen.SoftCurrencyBar.CoinsTarget.DOScale(_coinTargetScale, 0.1f);
        });

        if (count > 0)
        {
            long income = count + (Mathf.RoundToInt(count * _boost / 100)) + (count * player.LevelIncomeBoost[player.LocationLevel] / 100);

            player.Gold[player.LocationLevel] += income;
            screen.SoftCurrencyBar.ShowIncome(income);
        }

        else
        {
            player.Gold[player.LocationLevel] += count;
        }

        screen.SoftCurrencyBar.AddCoins(player.Gold[player.LocationLevel]);
        GameEvents.CheckCost_E?.Invoke(player.Gold[player.LocationLevel]);

        if (_isCoupon)
        {
            _isCoupon = false;
            _boost -= _couponPercent;
        }

        UpdateE();
        Bootstrap.Instance.SaveGame();
    }

    private void CheapestUpdate(long cost)
    {
        _cheapestUpdateCost = cost;
        UpdateE();
    }

    private void UpdateE()
    {
        if (player.Gold[player.LocationLevel] >= _cheapestUpdateCost)
        {
            screen.ExclamationMark.gameObject.SetActive(true);
        }

        else
        {
            screen.ExclamationMark.gameObject.SetActive(false);
        }
    }

    private void AddHardCurrency(int quantity)
    {
        player.HardCurrency += quantity;
        screen.AddHardCurrency(player.HardCurrency);
    }

    private void CoinsBoost(float percent, bool isCoupon)
    {
        _boost += percent;

        if (isCoupon)
        {
            _isCoupon = true;
            _couponPercent = percent;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Bootstrap.Instance.SaveGame();
    }
}
