using Code.Enums;
using Kuhpik;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : UIScreen
{
    [field: SerializeField] public ShopComponent ShopWindow { get; private set; }

    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _buyUpdate;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _setSpell;

    [SerializeField] private GameObject _inventoryWindow;

    private void Start()
    {
        _shopButton.onClick.AddListener(() =>
        {
            ShopWindow.gameObject.SetActive(true);
            _shopButton.interactable = false;

            GameEvents.TutorialNextStage_E?.Invoke();
            AudioManager.Instance.PlayAudio(AudioTypes.OpenWindow);
        });

        _buyUpdate.onClick.AddListener(() =>
        {
            ShopWindow.gameObject.SetActive(false);

            AudioManager.Instance.PlayAudio(AudioTypes.Upgrade);
            GameEvents.TutorialNextStage_E?.Invoke();
        });

        _inventoryButton.onClick.AddListener(() =>
        {
            _inventoryWindow.SetActive(!_inventoryWindow.activeInHierarchy);
            _inventoryButton.interactable = false;

            AudioManager.Instance.PlayAudio(AudioTypes.OpenWindow);
            GameEvents.TutorialNextStage_E?.Invoke();
        });

        _setSpell.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            GameEvents.TutorialNextStage_E?.Invoke();
        });
    }

    private void OnDisable()
    {
        _shopButton.onClick.RemoveListener(() =>
        {
            ShopWindow.gameObject.SetActive(true);
            _shopButton.interactable = false;

            GameEvents.TutorialNextStage_E?.Invoke();
            AudioManager.Instance.PlayAudio(AudioTypes.OpenWindow);
        });

        _buyUpdate.onClick.RemoveListener(() =>
        {
            ShopWindow.gameObject.SetActive(false);

            AudioManager.Instance.PlayAudio(AudioTypes.Upgrade);
            GameEvents.TutorialNextStage_E?.Invoke();
        });

        _inventoryButton.onClick.RemoveListener(() =>
        {
            _inventoryWindow.SetActive(!_inventoryWindow.activeInHierarchy);
            _inventoryButton.interactable = false;

            AudioManager.Instance.PlayAudio(AudioTypes.OpenWindow);
            GameEvents.TutorialNextStage_E?.Invoke();
        });

        _setSpell.onClick.RemoveListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            GameEvents.TutorialNextStage_E?.Invoke();
        });
    }
}
