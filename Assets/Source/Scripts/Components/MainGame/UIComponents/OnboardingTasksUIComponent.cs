using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnboardingTasksUIComponent : MonoBehaviour
{
    [field: SerializeField] public Button NextTask { get; private set; }
    [field: SerializeField] public GameObject Dot { get; private set; }

    [SerializeField] private TextMeshProUGUI _taskNumber;
    [SerializeField] private TextMeshProUGUI _task;
    [SerializeField] private TextMeshProUGUI _reward;

    private void Start()
    {
        Dot.SetActive(false);
    }

    public void SetQuest(string questNumber, string quest, string reward, TMP_SpriteAsset spriteAsset)
    {
        _taskNumber.text = questNumber;
        _task.text = quest;

        _reward.spriteAsset = spriteAsset;
        _reward.text = reward;
    }

    public void NextLocationTask(string questNumber)
    {
        _taskNumber.text = questNumber;
        _task.text = "Move to next location";
        _reward.gameObject.SetActive(false);
    }

    public void TaskCompleted(bool isCompleted)
    {
        Dot.SetActive(isCompleted);
        NextTask.interactable = isCompleted;
    }

    public void TasksFinish()
    {
        _taskNumber.gameObject.SetActive(false);
        _reward.gameObject.SetActive(false);
        _task.text = "You've done all quests!\n There will be more.";
    }

    public void SetText(string text)
    {
        _taskNumber.text = text;
    }
}
