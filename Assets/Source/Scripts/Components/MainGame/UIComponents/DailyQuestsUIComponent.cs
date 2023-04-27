using Code.Enums;
using TMPro;
using UnityEngine;

public class DailyQuestsUIComponent : MonoBehaviour
{
    public Vector2 CrossPosition { get; private set; }
    [SerializeField] private QuestUIComponent[] _questUIComponents;
    [SerializeField] private RectTransform _crossPosition;
    [SerializeField] private TextMeshProUGUI[] _textMeshProUGUIs;

    private const string _closeWindowAnimation = "CloseWindowAnimation";
    private Animator _animator;

    private void Awake()
    {
        CrossPosition = _crossPosition.position;

        _animator = GetComponent<Animator>();

        for (int i = 0; i < _questUIComponents.Length; i++)
            _questUIComponents[i].Init(i);

        gameObject.SetActive(false);
        GameEvents.CloseWindow_E += CloseWindow;
    }

    private void CloseWindow()
    {
        _animator.Play(_closeWindowAnimation);
    }

    public void SetQuest(int questIndex, Quest quest, int questDifficult, float multiplier, OnboardingTasks onboardingTasks, OnboardingTaskType taskType)
    {
        _questUIComponents[questIndex].QuestInfoSet(quest.QuestName, quest.Description, quest.QuestTask[questDifficult], multiplier, onboardingTasks, taskType);
    }
}
