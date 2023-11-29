using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoftCurrencyBarUIComponent : MonoBehaviour
{
    [field: SerializeField] public Transform CoinsTarget { get; private set; }
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private Button _addGold;
    [SerializeField] private TextMeshProUGUI _softCurrency;
    [SerializeField] private TextMeshProUGUI _incomeTMP;

    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private AnimationCurve _alphaCurve;
    [SerializeField] private float _time;

    private long _income;
    private bool _isShow;

    private Color _startColor;
    private Color _targetColor;

    private float _currentTime;

    private void Awake()
    {
        _startColor = _incomeTMP.color;
        _targetColor = new Color(1, 0.8279877f, 0);

        _incomeTMP.gameObject.SetActive(false);
        _addGold.onClick.AddListener(() =>
        {
            ShopComponent shopComponent = _gameUI.ShopComponent;
            _gameUI.OpenWindow(shopComponent.gameObject, _gameUI.ShopButton.gameObject, true, Vector2.zero);
            shopComponent.OpenTab(shopComponent.HardCurrencyTab, shopComponent.HardCurrencyeButton);
        });
    }

    public void AddCoins(long count)
    {
        _softCurrency.text = CurrencyFormat.Format(count);
    }

    public void ShowIncome(long income)
    {
        _income += income;
        _incomeTMP.gameObject.SetActive(true);
        _incomeTMP.text = $"+{CurrencyFormat.Format(_income)}";

        if (_isShow == false)
        {
            _isShow = true;
            StartCoroutine(HideIncome());
        }
    }

    private void Update()
    {
        if (_isShow)
        {
            Color color = Color.Lerp(_startColor, _targetColor, Mathf.Abs(Mathf.Sin(_currentTime * 2)));

            _incomeTMP.transform.localScale = Vector3.one * _scaleCurve.Evaluate(_currentTime);
            _incomeTMP.color = new Color(color.r, color.g, color.b, _alphaCurve.Evaluate(_currentTime));

            _currentTime += Time.deltaTime / _time;
        }
    }

    private IEnumerator HideIncome()
    {
        yield return new WaitForSeconds(2 + _time);

        _incomeTMP.DOKill();
        _incomeTMP.color = _startColor;

        _isShow = false;
        _income = 0;
        _currentTime = 0;
        _incomeTMP.gameObject.SetActive(false);
    }
}
