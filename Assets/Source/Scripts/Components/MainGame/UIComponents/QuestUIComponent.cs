using Code.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIComponent : MonoBehaviour
{
    [SerializeField] private bool _isAd;
    [SerializeField] private TextMeshProUGUI _questName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _rewardTMP;
    [SerializeField] private Image _rewardImage;
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _currencyTarget;
    [SerializeField] private CurrencyAnimationComponent[] _diamonds;

    private Vector3 _startPosition;
    private Vector3[] _movingVector = new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(-1, 1, 0), new Vector3(-1, -1, 0) };

    private string _descriptionText;
    private float _questTaskText;
    private float _reward;

    private int _index;
    private int _count;

    private OnboardingTasks _onboardingTasks;
    private OnboardingTaskType _taskType;

    public void Init(int index)
    {
        _index = index;
        _startPosition = _button.GetComponent<RectTransform>().anchoredPosition;
        _button.interactable = false;
        _count = PlayerPrefs.GetInt($"DailyQuest{_index}Count", 0);

        _button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ButtonClick);
            GetReward();
        });


        GameEvents.CheckTask_E += Step;

        for (int i = 0; i < _diamonds.Length; i++)
        {
            _diamonds[i].gameObject.SetActive(false);
        }
    }

    public void QuestInfoSet(string questName, string description, float questTask, float multiplier, OnboardingTasks onboardingTasks, OnboardingTaskType taskType)
    {
        _descriptionText = description;
        _questTaskText = questTask;

        _questName.text = questName;
        _description.text = $"{_descriptionText} {_count}/{_questTaskText}";
        _reward = 100 * multiplier;
        _rewardTMP.text = $"{_reward}";

        _onboardingTasks = onboardingTasks;
        _taskType = taskType;

        if (_count >= _questTaskText)
        {
            _button.interactable = true;
        }
    }

    private void Step(OnboardingTasks onboardingTasks, OnboardingTaskType[] taskType)
    {
        if (_onboardingTasks == onboardingTasks && taskType[0] == _taskType)
        {
            _count++;
            _description.text = $"{_descriptionText} {_count}/{_questTaskText}";

            if (_count >= _questTaskText)
            {
                _button.interactable = true;
            }

            PlayerPrefs.SetInt($"DailyQuest{_index}Count", _count);
        }
    }

    private void GetReward()
    {
        GameEvents.AddHardCurrency_E((int)_reward);
        PlayerPrefs.SetInt($"DailyQuest{_index}Count", 0);
        _button.interactable = false;

        for (int i = 0; i < _diamonds.Length; i++)
        {
            _diamonds[i].GetComponent<RectTransform>().anchoredPosition = _startPosition;
            _diamonds[i].Play(_movingVector[i] * 0.5f, _currencyTarget.position);
        }
    }
}
