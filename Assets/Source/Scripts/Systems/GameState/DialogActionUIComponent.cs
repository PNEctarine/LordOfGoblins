using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using UnityEngine;
using UnityEngine.UI;

public class DialogActionUIComponent : MonoBehaviour
{
    [SerializeField] private Button _locationsButton;
    [SerializeField] private Button _locationButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _spellButton;

    [SerializeField] private RectTransform _tutorialFinger;
    [SerializeField] private GollumUIComponent _gollumUIComponent;

    [SerializeField] private Animator _fingerAnimator;

    private void Start()
    {
        _tutorialFinger.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _locationsButton.onClick.AddListener(ClickButton);
        _locationButton.onClick.AddListener(ClickButton);
        _inventoryButton.onClick.AddListener(ClickButton);
        _spellButton.onClick.AddListener(ClickButton);
    }

    private void OnDisable()
    {
        _locationsButton.onClick.RemoveListener(ClickButton);
        _locationButton.onClick.RemoveListener(ClickButton);
        _inventoryButton.onClick.RemoveListener(ClickButton);
        _spellButton.onClick.RemoveListener(ClickButton);
    }

    public bool Action(int dialogueCount, int stage)
    {
        switch (stage)
        {
            case 0:
                return FirstStage(dialogueCount);

            case 1:
                return SecondStage(dialogueCount);

            default:
                _tutorialFinger.gameObject.SetActive(false);
                return false;
        }
    }

    private bool FirstStage(int dialogueCount)
    {
        if (dialogueCount == 2)
        {
            _locationsButton.gameObject.SetActive(true);
            _tutorialFinger.parent = _locationsButton.transform;
            _tutorialFinger.anchoredPosition = _locationsButton.GetComponent<RectTransform>().anchoredPosition;

            _tutorialFinger.gameObject.SetActive(true);
            _fingerAnimator.SetInteger("State", 2);

            return true;
        }

        else
        {
            _tutorialFinger.parent = gameObject.transform.parent;
            _tutorialFinger.gameObject.SetActive(false);

            return false;
        }
    }

    private bool SecondStage(int dialogueCount)
    {
        if (dialogueCount == 1)
        {
            _tutorialFinger.parent = _locationsButton.transform;
            _tutorialFinger.anchoredPosition = _locationsButton.GetComponent<RectTransform>().anchoredPosition;

            _tutorialFinger.gameObject.SetActive(true);
            _fingerAnimator.SetInteger("State", 2);

            return true;
        }

        else if (dialogueCount == 3)
        {
            _tutorialFinger.parent = _locationButton.transform;
            _tutorialFinger.anchoredPosition = _locationButton.GetComponent<RectTransform>().anchoredPosition;

            _tutorialFinger.gameObject.SetActive(true);
            _fingerAnimator.SetInteger("State", 2);

            return true;
        }

        else if (dialogueCount == 5)
        {
            GameEvents.BuySpell_E?.Invoke(0, 20, SpellTypes.IncreaseIncome);

            _tutorialFinger.parent = _inventoryButton.transform;
            _tutorialFinger.anchoredPosition = _inventoryButton.GetComponent<RectTransform>().anchoredPosition;

            _tutorialFinger.gameObject.SetActive(true);
            _fingerAnimator.SetInteger("State", 2);

            return true;
        }

        else if (dialogueCount == 0 || dialogueCount == 4)
        {
            _tutorialFinger.parent = gameObject.transform.parent;
            _tutorialFinger.gameObject.SetActive(false);

            return false;
        }

        return false;
    }

    private void ClickButton()
    {
        _tutorialFinger.parent = gameObject.transform.parent;
        _tutorialFinger.gameObject.SetActive(false);

        _gollumUIComponent.ButtonClicked();
        _gollumUIComponent.NextDialogue();

    }
}
