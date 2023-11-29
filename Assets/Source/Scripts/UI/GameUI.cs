using System;
using System.Collections;
using Code.Enums;
using DG.Tweening;
using Kuhpik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIScreen
{
    [SerializeField] private Button _inventoryButton;
    [field: SerializeField] public Button ShopButton { get; private set; }
    [SerializeField] private Button _raidButton;
    [SerializeField] private Button _settingsButton;
    [field: SerializeField] public Button LocationsButton { get; private set; }
    [field: SerializeField] public Button SuperMergeButton { get; private set; }
    [field: SerializeField] public Button OfferButton { get; private set; }
    [SerializeField] private Button _dailyQuestsButton;

    [Space]
    [SerializeField] private Button _cross;
    [field: SerializeField] public Image ExclamationMark { get; private set; }

    [Space]
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private TextMeshProUGUI _hardCurrency;
    [field: SerializeField] public SpellCDUIComponent SpellCountDownComponent { get; private set; }

    [Space]
    [SerializeField] private GameObject _pastLevels;
    [field: SerializeField] public InventoryUiComponent InventoryUiComponent { get; private set; }
    [field: SerializeField] public ShopComponent ShopComponent { get; private set; }
    [SerializeField] private GameObject _subscriptionWindow;
    [SerializeField] private GameObject _raidWindow;
    [field: SerializeField] public SettingsUIComponent SettingsUIComponent { get; private set; }
    [field: SerializeField] public PassiveEarningsUIComponent PassiveEarningsUIComponent { get; private set; }
    [field: SerializeField] public LocationsUIComponent LocationsUIComponent { get; private set; }
    [field: SerializeField] public DailyQuestsUIComponent DailyQuestsUIComponent { get; private set; }
    [field: SerializeField] public BoosterUIComponent BoosterWindow { get; private set; }
    [field: SerializeField] public SoftCurrencyBarUIComponent SoftCurrencyBar { get; private set; }
    [field: SerializeField] public OnboardingTasksUIComponent OnboardingTasksUIComponent { get; private set; }
    [field: SerializeField] public SuperMergeUIComponent SuperMergeUIComponent { get; private set; }
    [field: SerializeField] public OfferUIComponent OfferUIComponent { get; private set; }

    private GameObject _openWindow;
    private GameObject _buttonImage;
    private GameObject _activeButton;
    private const string _spellCountDownKey = "SpellCountDown";

    private float[] _onboardingTasksX = new float[2];
    private float[] _dailyQuestsX = new float[2];

    private float _countDown;
    private float _duration;

    private bool _isAnimation;

    private void Start()
    {
        _onboardingTasksX[0] = OnboardingTasksUIComponent.transform.position.x;
        _onboardingTasksX[1] = OnboardingTasksUIComponent.transform.position.x + 3;

        _dailyQuestsX[0] = _dailyQuestsButton.transform.position.x;
        _dailyQuestsX[1] = _dailyQuestsButton.transform.position.x + 3;

        _cross.gameObject.SetActive(false);
        _pastLevels.SetActive(false);
        ExclamationMark.gameObject.SetActive(false);

        _countDown = PlayerPrefs.GetFloat(_spellCountDownKey, 0);
        bool isTutorialCompleted = Convert.ToBoolean(PlayerPrefs.GetInt("Tutorial", 0));

        if (PurchaseManager.CheckBuyState("subscription_weekly"))
        {
            GameEvents.Subscripe_E?.Invoke(true);
            _subscriptionWindow.SetActive(false);
        }

        else
        {
            GameEvents.Subscripe_E?.Invoke(false);
            _subscriptionWindow.SetActive(true);
        }

        if (isTutorialCompleted == false)
        {
            _subscriptionWindow.SetActive(false);
        }

        _inventoryButton.onClick.AddListener(() =>
        {
            OpenWindow(InventoryUiComponent.gameObject, _inventoryButton.gameObject, true, Vector2.zero);
        });

        ShopButton.onClick.AddListener(() =>
        {
            OpenWindow(ShopComponent.gameObject, ShopButton.gameObject, true, Vector2.zero);
        });

        _raidButton.onClick.AddListener(() =>
        {
            OpenWindow(_raidWindow, _raidButton.gameObject, false, Vector2.zero);
        });

        _settingsButton.onClick.AddListener(() =>
        {
            OpenWindow(SettingsUIComponent.gameObject, _settingsButton.gameObject, true, SettingsUIComponent.CrossPosition);
        });

        LocationsButton.onClick.AddListener(() =>
        {
            GameEvents.LocationMap_E?.Invoke();
        });

        SuperMergeButton.onClick.AddListener(() =>
        {
            OpenWindow(SuperMergeUIComponent.gameObject, SuperMergeButton.gameObject, false, SuperMergeUIComponent.CrossPosition);
        });

        OfferButton.onClick.AddListener(() =>
        {
            OpenWindow(OfferUIComponent.gameObject, OfferButton.gameObject, false, OfferUIComponent.CrossPosition);
        });

        _dailyQuestsButton.onClick.AddListener(() =>
        {
            OpenWindow(DailyQuestsUIComponent.gameObject, _dailyQuestsButton.gameObject, false, DailyQuestsUIComponent.CrossPosition);
        });

        _cross.onClick.AddListener(() =>
        {
            OpenWindow(_openWindow, _activeButton, _isAnimation, Vector2.zero);
        });
    }

    public void OpenWindow(GameObject window, GameObject button, bool isAnimation, Vector3 crossPosition)
    {
        AudioManager.Instance.PlayAudio(AudioTypes.OpenWindow);
        _cross.gameObject.SetActive(true);

        if (_openWindow)
        {
            if (isAnimation && _openWindow == window)
                GameEvents.CloseWindow_E?.Invoke();

            else
                _openWindow.SetActive(false);

            _activeButton = null;
            _buttonImage.SetActive(true);
        }

        if (_openWindow != window)
        {
            _openWindow = window;
            _openWindow.SetActive(true);

            _activeButton = button;
            _isAnimation = isAnimation;

            if (crossPosition == Vector3.zero)
            {
                _buttonImage = button.GetComponentsInChildren<Image>()[1].gameObject;
                _cross.image.raycastTarget = false;
                _cross.interactable = false;
                _cross.GetComponent<RectTransform>().position = button.GetComponent<RectTransform>().position;
            }

            else
            {
                _buttonImage = button.GetComponentsInChildren<Image>()[0].gameObject;
                _cross.image.raycastTarget = true;
                _cross.interactable = true;
                _cross.GetComponent<RectTransform>().position = crossPosition;
            }

            OnboardingTasksUIComponent.transform.DOMoveX(_onboardingTasksX[1], 0.5f);
            _dailyQuestsButton.transform.DOMoveX(_dailyQuestsX[1], 0.5f);
            _buttonImage.SetActive(false);
        }

        else
        {
            GameEvents.WindowClosed_E();

            _cross.gameObject.SetActive(false);
            _activeButton = null;
            _openWindow = null;

            OnboardingTasksUIComponent.transform.DOMoveX(_onboardingTasksX[0], 0.5f);
            _dailyQuestsButton.transform.DOMoveX(_dailyQuestsX[0], 0.5f);
        }
    }

    public void CloseAll()
    {
        if (_openWindow)
        {
            _buttonImage.SetActive(true);
            _openWindow.SetActive(false);
            _cross.gameObject.SetActive(false);
            _activeButton = null;
            _openWindow = null;

            OnboardingTasksUIComponent.transform.DOMoveX(_onboardingTasksX[0], 0.5f);
            _dailyQuestsButton.transform.DOMoveX(_dailyQuestsX[0], 0.5f);

            GameEvents.WindowClosed_E();
        }
    }

    public void AddHardCurrency(int quantity)
    {
        _hardCurrency.text = $"{quantity}";
    }

    public void PastLevelWarning()
    {
        _pastLevels.SetActive(true);
        StartCoroutine(PastLevelCD());
    }

    private IEnumerator PastLevelCD()
    {
        yield return new WaitForSeconds(2f);
        _pastLevels.SetActive(false);
    }
}