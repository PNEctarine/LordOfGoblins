using Code.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellInventoryComponent : MonoBehaviour
{
    [SerializeField] private Sprite _timerSprite;
    [SerializeField] private SpellTypes _spellType;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private Button _useButton;
    [SerializeField] private int _percent;

    private InventoryUiComponent _inventoryUiComponent;
    private int _spellsCount;

    private void Awake()
    {
        GameEvents.SpellPickUp_E += GetSpell;
        GameEvents.SpellButtonsInteractable_E += InteractableButton;

        _inventoryUiComponent = GetComponentInParent<InventoryUiComponent>();

        _useButton.onClick.AddListener(() => SetSpell());
        _spellsCount = PlayerPrefs.GetInt($"SpellCount{_spellType + _percent}", 0);

        _count.outlineWidth = 0.4f;
        _count.outlineColor = new Color32(38, 23, 76, 255);
        _count.text = _spellsCount.ToString();

    }

    private void OnEnable()
    {
        _spellsCount = PlayerPrefs.GetInt($"SpellCount{_spellType + _percent}", 0);
        _count.text = _spellsCount.ToString();
    }

    private void GetSpell(int percent, SpellTypes spellType)
    {
        if (percent == _percent && spellType == _spellType)
        {
            _spellsCount++;
            _count.text = _spellsCount.ToString();

            PlayerPrefs.SetInt($"SpellCount{_spellType + _percent}", _spellsCount);
        }
    }

    private void SetSpell()
    {
        if (_spellsCount > 0)
        {
            _spellsCount--;
            _count.text = _spellsCount.ToString();

            PlayerPrefs.SetInt($"SpellCount{_spellType + _percent}", _spellsCount);
            GameEvents.SpellActivate_E?.Invoke(_spellType, _percent, _timerSprite);
        }

        else
        {
            _inventoryUiComponent.OpenShop();
        }
    }

    private void InteractableButton(bool isInteractable)
    {
        _useButton.interactable = isInteractable;
    }
}
