using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiComponent : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private SkeletonGraphic _scrollAnimation;
    [SerializeField] private GameUI _gameUI;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _closeButton.onClick.AddListener(() =>
        {
            _animator.Play("CloseWindowAnimation");
            _scrollAnimation.AnimationState.SetAnimation(0, "Close", false).Complete += OnSpineAnimationComplete;
        });

        GameEvents.CloseWindow_E += CloseWindow;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _scrollAnimation.AnimationState.SetAnimation(0, "Open", false);
    }

    private void CloseWindow()
    {
        _animator.Play("CloseWindowAnimation");
        _scrollAnimation.AnimationState.SetAnimation(0, "Close", false).Complete += OnSpineAnimationComplete;
    }

    public void OpenShop()
    {
        ShopComponent shopComponent = _gameUI.ShopComponent;
        shopComponent.OpenTab(shopComponent.HardCurrencyTab, shopComponent.HardCurrencyeButton);
        _gameUI.OpenWindow(shopComponent.gameObject, _gameUI.ShopButton.gameObject, true, Vector2.zero);
    }

    private void OnSpineAnimationComplete(TrackEntry trackEntry)
    {
        gameObject.SetActive(false);
        GameEvents.WindowClosed_E?.Invoke();
    }
}
