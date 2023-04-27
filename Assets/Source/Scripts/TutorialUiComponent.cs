using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUiComponent : MonoBehaviour
{
    [SerializeField] private GollumComponent _gollum;
    [SerializeField] private GameObject _fingerTutorial;
    [SerializeField] private ShopComponent _shopWindow;

    [SerializeField] private Transform _mask;
    [SerializeField] private Transform _spriteMask;
    [SerializeField] private Transform _resourceMaskTarget;

    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _freeSpellButton;

    [SerializeField] private TextMeshProUGUI _dialog;
    [SerializeField] private Queue<string> _sentences;
    [SerializeField] private SentencesManager _sentencesManager;

    private int _stageCount;
    private bool _isTutorialCompleted;

    private void Start()
    {
        GameEvents.TutorialNextStage_E += NextStage;

        _shopButton.interactable = false;
        _inventoryButton.interactable = false;

        _fingerTutorial.SetActive(false);
        _sentences = new Queue<string>();
        _sentencesManager.GetComponent<SentencesManager>();
        _gollum.SetAnimation(1);
        _isTutorialCompleted = Convert.ToBoolean(PlayerPrefs.GetInt("Tutorial", 0));

        if (_isTutorialCompleted)
        {
            gameObject.SetActive(false);
        }

        foreach (string sentence in _sentencesManager.Sentences)
        {
            _sentences.Enqueue(sentence);
        }

        StartDialogue();
    }

    public void OnMouseDown()
    {
        if (_gollum.gameObject.activeInHierarchy)
        {
            _gollum.SetAnimation(1);
            NextStage();
        }
    }

    private void NextStage()
    {
        _stageCount++;

        if (_sentences.Count > 0 || _stageCount < 13)
        {
            if (_stageCount < 3 || _stageCount == 4 || _stageCount == 7 || _stageCount == 11 || _stageCount == 12)
            {
                if (_gollum.gameObject.activeInHierarchy == false)
                {
                    _gollum.gameObject.SetActive(true);
                    _gollum.SetAnimation(1);

                    _fingerTutorial.GetComponentInChildren<Animator>().SetInteger("State", 0);
                    _fingerTutorial.SetActive(false);

                    _spriteMask.localScale = Vector3.one * 500;
                }

                StartDialogue();
            }

            else if (_stageCount == 3)
            {
                GameEvents.TutorialMerge_E?.Invoke();
                _gollum.gameObject.SetActive(false);

                _spriteMask.position = _resourceMaskTarget.position;
                _spriteMask.DOScale(30, 1f).OnComplete(() =>
                {
                    _fingerTutorial.SetActive(true);
                    _fingerTutorial.GetComponentInChildren<Animator>().SetInteger("State", 1);
                });
            }

            else if (_stageCount == 5)
            {
                _gollum.gameObject.SetActive(false);
                _mask.parent = _shopButton.transform.parent;
                _spriteMask.gameObject.SetActive(true);
                _spriteMask.localScale = Vector3.one * 500;

                _spriteMask.position = _shopButton.GetComponent<RectTransform>().position;
                _spriteMask.DOScale(20, 1f).OnComplete(() =>
                {
                    _shopButton.interactable = true;
                    _fingerTutorial.SetActive(true);
                    _fingerTutorial.transform.parent = _shopButton.transform.parent;
                    _fingerTutorial.GetComponentInChildren<Animator>().SetInteger("State", 2);
                    _fingerTutorial.GetComponent<RectTransform>().position = _shopButton.GetComponent<RectTransform>().position;
                });
            }

            else if (_stageCount == 6)
            {
                _shopWindow.OpenTab(_shopWindow.IncreaseIncomeTab, _shopWindow.IncreaseIncomeUpgradesButton);
                _mask.parent = _upgradeButton.transform.parent;
                _spriteMask.gameObject.SetActive(true);
                _spriteMask.localScale = Vector3.one * 500;

                _spriteMask.position = _upgradeButton.GetComponent<RectTransform>().position;
                _upgradeButton.interactable = true;

                _fingerTutorial.SetActive(true);
                _fingerTutorial.transform.parent = _upgradeButton.transform.parent;
                _fingerTutorial.GetComponentInChildren<Animator>().SetInteger("State", 2);
                _fingerTutorial.GetComponent<RectTransform>().anchoredPosition = _upgradeButton.GetComponent<RectTransform>().anchoredPosition + Vector2.down * 50;
            }

            else if (_stageCount == 8)
            {
                _gollum.gameObject.SetActive(false);
                _mask.parent = _inventoryButton.transform.parent;
                _spriteMask.gameObject.SetActive(true);
                _spriteMask.localScale = Vector3.one * 500;

                _spriteMask.position = _inventoryButton.GetComponent<RectTransform>().position;
                _spriteMask.DOScale(20, 1f).OnComplete(() =>
                {
                    _inventoryButton.interactable = true;
                    _fingerTutorial.SetActive(true);
                    _fingerTutorial.transform.parent = _inventoryButton.transform.parent;
                    _fingerTutorial.GetComponentInChildren<Animator>().SetInteger("State", 2);
                    _fingerTutorial.GetComponent<RectTransform>().position = _inventoryButton.GetComponent<RectTransform>().position;
                });
            }

            else if (_stageCount == 9)
            {
                _mask.parent = _freeSpellButton.transform.parent;
                _spriteMask.gameObject.SetActive(true);
                _spriteMask.localScale = Vector3.one * 500;

                _spriteMask.position = _freeSpellButton.GetComponent<RectTransform>().position;

                _freeSpellButton.interactable = true;
                _fingerTutorial.SetActive(true);
                _fingerTutorial.transform.parent = _freeSpellButton.transform.parent;
                _fingerTutorial.GetComponentInChildren<Animator>().SetInteger("State", 2);
                _fingerTutorial.GetComponent<RectTransform>().anchoredPosition = _freeSpellButton.GetComponent<RectTransform>().anchoredPosition + Vector2.down * 50;
            }

            else if (_stageCount == 10)
            {
                _mask.parent = _inventoryButton.transform.parent;
                _spriteMask.gameObject.SetActive(true);
                _spriteMask.localScale = Vector3.one * 500;

                _spriteMask.position = _inventoryButton.GetComponent<RectTransform>().position;

                _inventoryButton.interactable = true;
                _fingerTutorial.SetActive(true);
                _fingerTutorial.transform.parent = _inventoryButton.transform.parent;
                _fingerTutorial.GetComponentInChildren<Animator>().SetInteger("State", 2);
                _fingerTutorial.GetComponent<RectTransform>().position = _inventoryButton.GetComponent<RectTransform>().position;
            }
        }

        else
        {
            GameEvents.TutorialCompleate_E?.Invoke();
        }
    }

    private void StartDialogue()
    {
        _dialog.text = _sentences.Dequeue();
    }
}
