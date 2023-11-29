using Code.Enums;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCardUIComponent : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI Name { get; private set; }

    [SerializeField] private UpdateTypes _updateType;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _costTMP;
    [SerializeField] private Animator _rayAnimator;

    [Space(10)]
    [SerializeField] private bool useEffect;
    [SerializeField][ShowIf("useEffect")] private TextMeshProUGUI _currentEffectivenessTMP;
    [SerializeField][ShowIf("useEffect")] private TextMeshProUGUI _nextEffectivenessTMP;
    [SerializeField][ShowIf("useEffect")] private string _measurementUnit;

    public UpdateTypes UpdateType => _updateType;
    public bool IsMax { get; private set; }

    private bool _isEnoughCoins;
    [SerializeField] private long _cost;

    public void SetUp()
    {
        GameEvents.CheckCost_E += CheckCost;
        _buyButton.onClick.AddListener(() =>
        {
            if (_isEnoughCoins)
                GameEvents.BuyUpgrade_E?.Invoke(_updateType, this, true);

            else
                GameEvents.NotEnoughGold_E?.Invoke();
        });

        _costTMP.text = _cost > 0 ? CurrencyFormat.Format(_cost) : "Free";
        GameEvents.BuyUpgrade_E?.Invoke(_updateType, this, false);
    }

    private void OnDestroy()
    {
        GameEvents.CheckCost_E -= CheckCost;
    }

    private void OnEnable()
    {
        GameEvents.BuyUpgrade_E?.Invoke(_updateType, this, false);

        if (_isEnoughCoins)
        {
            _rayAnimator.Play("ButtonRayAnimation", 0);
        }
    }

    public void SetInfo(long cost, float currentEffectiveness = 0, float nextEffectiveness = 0)
    {
        _cost = cost;
        _costTMP.text = _cost > 0 ? CurrencyFormat.Format(_cost) : "Free";

        if (useEffect)
        {
            _currentEffectivenessTMP.text = $"{currentEffectiveness}{_measurementUnit}";
            _nextEffectivenessTMP.text = $"{nextEffectiveness}{_measurementUnit}";
        }
    }

    public void CheckCost(long coins)
    {
        if (this)
        {
            if (coins >= _cost)
            {
                _buyButton.interactable = true;
                _isEnoughCoins = true;

                if (gameObject.activeInHierarchy)
                    _rayAnimator.Play("ButtonRayAnimation", 0);
            }

            else
            {
                _isEnoughCoins = false;
                _rayAnimator.StopPlayback();
            }
        }
    }

    public void MaxLevel()
    {
        IsMax = true;

        _cost = long.MaxValue;
        _buyButton.interactable = false;
        _costTMP.text = "MAX";
    }

    public long Cost()
    {
        return _cost;
    }
}
