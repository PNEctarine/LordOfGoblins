using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using TMPro;
using UnityEngine;

public class GollumUIComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueTMP;
    [SerializeField] private GameObject[] _gollum;

    private DialogActionUIComponent _dialogActionUIComponent;

    private int _dialogueCount;
    private int _actionsCount;
    private int _stage;
    private int _actions;

    private const string _dialogueCountKey = "DialogueCount";
    private const string _actionsCountKey = "ActionsCount";
    private string[] _dialogues;

    private void Awake()
    {
        _dialogActionUIComponent = GetComponent<DialogActionUIComponent>();

        _actionsCount = PlayerPrefs.GetInt(_actionsCountKey, 0);
        _dialogueCount = PlayerPrefs.GetInt(_dialogueCountKey, 0);
    }

    public void Dialogue(string[] dialogues, int stage, int actions)
    {
        _dialogues = dialogues;
        _stage = stage;
        _dialogueTMP.text = _dialogues[_dialogueCount];
        _actions = actions;
    }

    public void NextDialogue()
    {
        if (_dialogues.Length - 1 > _dialogueCount || _actions - 1 > _actionsCount)
        {
            if (_dialogActionUIComponent.Action(_dialogueCount + _actionsCount + 1, _stage))
            {
                _actionsCount++;

                for (int i = 0; i < _gollum.Length; i++)
                {
                    _gollum[i].SetActive(false);
                }
            }

            else
            {
                for (int i = 0; i < _gollum.Length; i++)
                {
                    _gollum[i].SetActive(true);
                }

                _dialogueCount++;
                _dialogueTMP.text = _dialogues[_dialogueCount];
            }
        }

        else
        {
            _dialogueCount = 0;
            _actionsCount = 0;

            Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        }

        PlayerPrefs.SetInt(_dialogueCountKey, _dialogueCount);
        PlayerPrefs.SetInt(_actionsCountKey, _actionsCount);
    }

    public void ButtonClicked()
    {
        //_dialogueCount = Mathf.Abs(_dialogueCount - _actionsCount + 1);
    }
}
