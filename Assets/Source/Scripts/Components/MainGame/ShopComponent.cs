using System.Collections;
using Code.Enums;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopComponent : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;

    [field: SerializeField] public Button CheapestUpgradesButton { get; private set; }
    [field: SerializeField] public Button GoblinUpgradesButton { get; private set; }
    [field: SerializeField] public Button IncreaseIncomeUpgradesButton { get; private set; }
    [field: SerializeField] public Button HardCurrencyeButton { get; private set; }
    [field: SerializeField] public Button BuyHardCurrencyeButton { get; private set; }
    [SerializeField] private Button _crossButton;

    [Space(10)]
    [SerializeField] private UpdatesTabUIComponent[] _updatesTabUIComponents;
    [SerializeField] private CheapestImprovementsUIComponent _ceapestImprovementsUIComponent;
    [field: SerializeField] public GameObject BoosterTab { get; private set; }
    [field: SerializeField] public GameObject GoblinUpgradeTab { get; private set; }
    [field: SerializeField] public GameObject IncreaseIncomeTab { get; private set; }
    [field: SerializeField] public GameObject HardCurrencyTab { get; private set; }
    [field: SerializeField] public GameObject BuyHardCurrencyTab { get; private set; }

    [Space(10)]
    [SerializeField] private Sprite _selectedButton;
    [SerializeField] private Sprite _unselectedButton;

    [Space(10)]
    [SerializeField] private SkeletonGraphic _scrollAnimation;
    [SerializeField] private GameObject _successfulUpgradePopUp;
    [SerializeField] private TextMeshProUGUI[] _textMeshProUGUIs;

    private Animator _animator;
    private const float _upMultiplier = 0.2f;
    private bool _isSetUp;

    private int _openingCount;

    public void SetUp()
    {
        if (_isSetUp)
            return;

        _isSetUp = true;

        GameEvents.CloseWindow_E += CloseWindow;

        IncreaseIncomeUpgradesButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            OpenTab(IncreaseIncomeTab, IncreaseIncomeUpgradesButton);
        });

        CheapestUpgradesButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            OpenTab(BoosterTab, CheapestUpgradesButton);
        });

        GoblinUpgradesButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            OpenTab(GoblinUpgradeTab, GoblinUpgradesButton);
        });

        BuyHardCurrencyeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            OpenTab(BuyHardCurrencyTab, BuyHardCurrencyeButton);
        });

        HardCurrencyeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            OpenTab(HardCurrencyTab, HardCurrencyeButton);
        });

        _crossButton.onClick.AddListener(() =>
        {
            GetComponent<Animator>().Play("CloseWindowAnimation");
            _scrollAnimation.AnimationState.SetAnimation(0, "Close", false).Complete += OnSpineAnimationComplete;
        });

        for (int i = 0; i < _updatesTabUIComponents.Length; i++)
        {
            for (int j = 0; j < _updatesTabUIComponents[i].UpdateCardUIComponents.Length; j++)
            {
                _updatesTabUIComponents[i].UpdateCardUIComponents[j].SetUp();
            }
        }

        _animator = GetComponent<Animator>();
        _ceapestImprovementsUIComponent.SetUp();
        _openingCount = 0;

        OpenTab(BoosterTab, CheapestUpgradesButton);
        GameEvents.WindowClosed_E?.Invoke();
        gameObject.SetActive(false);
    }

    public void SuccessfulUpgrage()
    {
        _successfulUpgradePopUp.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(HidePopUp());
    }

    public void OpenTab(GameObject tab, Button button)
    {
        DisableAll();

        tab.SetActive(true);
        button.transform.localScale += Vector3.one * _upMultiplier;
        button.image.sprite = _selectedButton;
        button.transform.SetAsLastSibling();

        _scrollRect.content = tab.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _scrollAnimation.AnimationState.SetAnimation(0, "Open", false);
        _successfulUpgradePopUp.SetActive(false);

        OpenTab(BoosterTab, CheapestUpgradesButton);
        StopAllCoroutines();

        _openingCount++;

        if (_openingCount >= 5)
        {
            ApplovinSDK.Instance.ShowInterstitial();
            _openingCount = 0;
        }
    }

    private void CloseWindow()
    {
        _animator.Play("CloseWindowAnimation");
        _scrollAnimation.AnimationState.SetAnimation(0, "Close", false).Complete += OnSpineAnimationComplete;
    }

    private void OnSpineAnimationComplete(TrackEntry trackEntry)
    {
        gameObject.SetActive(false);
        GameEvents.WindowClosed_E?.Invoke();
    }

    private void DisableAll()
    {
        IncreaseIncomeTab.SetActive(false);
        BoosterTab.SetActive(false);
        GoblinUpgradeTab.SetActive(false);
        BuyHardCurrencyTab.SetActive(false);
        HardCurrencyTab.SetActive(false);

        IncreaseIncomeUpgradesButton.transform.localScale = Vector3.one;
        CheapestUpgradesButton.transform.localScale = Vector3.one;
        GoblinUpgradesButton.transform.localScale = Vector3.one;
        BuyHardCurrencyeButton.transform.localScale = Vector3.one;
        HardCurrencyeButton.transform.localScale = Vector3.one;

        IncreaseIncomeUpgradesButton.image.sprite = _unselectedButton;
        CheapestUpgradesButton.image.sprite = _unselectedButton;
        GoblinUpgradesButton.image.sprite = _unselectedButton;
        BuyHardCurrencyeButton.image.sprite = _unselectedButton;
        HardCurrencyeButton.image.sprite = _unselectedButton;
    }

    private IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(2);
        _successfulUpgradePopUp.SetActive(false);
    }
}
