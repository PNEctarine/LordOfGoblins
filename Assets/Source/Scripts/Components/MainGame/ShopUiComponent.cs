using UnityEngine;
using UnityEngine.UI;

public class ShopUiComponent : MonoBehaviour
{
    [SerializeField] private Button _closeShop;
    [SerializeField] private Button _spells;
    [SerializeField] private GameObject _spellsTab;

    [Space(10)]
    [SerializeField] private Button _subcription;
    [SerializeField] private GameObject _subscriptionTab;

    [Space(10)]
    [SerializeField] private Button _hardCurrency;
    [SerializeField] private GameObject _hardCurrencTab;

    private void Start()
    {
        _closeShop.onClick.AddListener(() => gameObject.SetActive(false));

        _spellsTab.SetActive(true);
        _subscriptionTab.SetActive(false);
        _hardCurrencTab.SetActive(false);

        _spells.onClick.AddListener(() =>
        {
            _spellsTab.SetActive(true);
            _subscriptionTab.SetActive(false);
            _hardCurrencTab.SetActive(false);
        });

        _subcription.onClick.AddListener(() =>
        {
            _spellsTab.SetActive(false);
            _subscriptionTab.SetActive(true);
            _hardCurrencTab.SetActive(false);
        });

        _hardCurrency.onClick.AddListener(() =>
        {
            _spellsTab.SetActive(false);
            _subscriptionTab.SetActive(false);
            _hardCurrencTab.SetActive(true);
        });

        gameObject.SetActive(false);
    }
}
