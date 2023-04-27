using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class SettingsUIComponent : MonoBehaviour
{
    public Vector2 CrossPosition { get; private set; }
    [SerializeField] private RectTransform _crossPosition;

    [SerializeField] private SkeletonGraphic _scrollAnimation;
    [SerializeField] private MusicSliderUIComponent _musicSliderUIComponent;
    [SerializeField] private EffectsSliderUIcomponent _effectsSliderUIcomponent;
    [SerializeField] private TextMeshProUGUI[] _textMeshProUGUIs;

    private Animator _animator;

    private const string _closeWindowAnimation = "CloseWindowAnimation";
    private const string _close = "Close";
    private const string _open = "Open";

    private void Start()
    {
        CrossPosition = _crossPosition.position;

        _animator = GetComponent<Animator>();
        _musicSliderUIComponent.SetSetting();
        _effectsSliderUIcomponent.SetSetting();

        gameObject.SetActive(false);
        GameEvents.CloseWindow_E += CloseWindow;
    }

    private void OnEnable()
    {
        _scrollAnimation.AnimationState.SetAnimation(0, _open, false);
    }

    private void CloseWindow()
    {
        _animator.Play(_closeWindowAnimation);
        _scrollAnimation.AnimationState.SetAnimation(0, _close, false).Complete += OnSpineAnimationComplete;
    }

    private void OnSpineAnimationComplete(TrackEntry trackEntry)
    {
        gameObject.SetActive(false);
        GameEvents.WindowClosed_E?.Invoke();
    }
}
