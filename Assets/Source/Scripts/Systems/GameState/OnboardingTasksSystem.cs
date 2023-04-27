using System.Collections.Generic;
using System.Linq;
using Code.Enums;
using Kuhpik;
using TMPro;
using UnityEngine;

public class OnboardingTasksSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private OnboardingTasksConfig _onboardingTasksConfig;
    [SerializeField] private ResourcesConfig _resourcesConfig;

    [SerializeField] private TMP_SpriteAsset _hardCurrencySpriteAsset;
    [SerializeField] private TMP_SpriteAsset _softCurrencySpriteAsset;

    private OnboardingTasksUIComponent _onboardingTasksUIComponent;
    private OnboardingTasksRewardComponent _onboardingTasksRewardComponent;
    private int _reward;
    private string _rewardText;
    private TMP_SpriteAsset _spriteAsset;

    public override void OnInit()
    {
        _onboardingTasksUIComponent = screen.OnboardingTasksUIComponent;
        _onboardingTasksRewardComponent = _onboardingTasksUIComponent.GetComponent<OnboardingTasksRewardComponent>();

        if (player.OnboardingTaskProgress == null)
        {
            player.OnboardingTaskProgress = new List<int>();

            for (int i = 0; i < _onboardingTasksConfig.Tasks.Length; i++)
            {
                player.OnboardingTaskProgress.Add(0);
            }
        }

        else if (_onboardingTasksConfig.Tasks.Length > player.OnboardingTaskProgress.Count)
        {
            int index = _onboardingTasksConfig.Tasks.Length - player.OnboardingTaskProgress.Count;

            for (int i = 0; i < index; i++)
            {
                player.OnboardingTaskProgress.Add(0);
            }
        }

        if (TasksCountCheck())
        {
            SetReward();

            _onboardingTasksUIComponent.TaskCompleted(false);
            _onboardingTasksUIComponent.NextTask.onClick.AddListener(() => CopleateTask());

            if (IsCurrentLocation())
            {
                _onboardingTasksUIComponent.SetQuest($"Quest {player.OnboardingTaskLevel + 1}",
                    $"{_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].TaskDescription} {player.OnboardingTaskProgress[player.OnboardingTaskLevel]}/{_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Count}",
                    $"{_rewardText}",
                    _spriteAsset);
            }

            else
            {
                _onboardingTasksUIComponent.NextLocationTask($"Quest {player.OnboardingTaskLevel + 1}");
            }

            if (player.OnboardingTaskProgress[player.OnboardingTaskLevel] >= _onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Count)
            {
                _onboardingTasksUIComponent.TaskCompleted(true);
            }

            GameEvents.CheckTask_E += Step;
        }

        else
        {
            _onboardingTasksUIComponent.TasksFinish();
        }
    }

    private void Step(OnboardingTasks onboardingTasks, OnboardingTaskType[] taskType)
    {
        bool isAll = taskType[0] == _onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].TaskType[0];

        if (_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Task == onboardingTasks && isAll)
        {
            player.OnboardingTaskProgress[player.OnboardingTaskLevel]++;

            if (player.OnboardingTaskProgress[player.OnboardingTaskLevel] >= _onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Count)
            {
                _onboardingTasksUIComponent.TaskCompleted(true);
            }

            screen.OnboardingTasksUIComponent.SetQuest($"Quest {player.OnboardingTaskLevel + 1}",
                $"{_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].TaskDescription} {player.OnboardingTaskProgress[player.OnboardingTaskLevel]}/{_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Count}",
                $"{_rewardText}",
                _spriteAsset);
        }

        for (int i = player.OnboardingTaskLevel; i < _onboardingTasksConfig.Tasks.Length; i++)
        {
            isAll = CheckingArrays(_onboardingTasksConfig.Tasks[i].TaskType, taskType);

            if (_onboardingTasksConfig.Tasks[i].Task == onboardingTasks && _onboardingTasksConfig.Tasks[i].IsPassive && isAll && _onboardingTasksConfig.Tasks[i].LocationToOpen <= player.LocationLevel)
            {
                player.OnboardingTaskProgress[i]++;
            }
        }
    }

    private bool CheckingArrays(OnboardingTaskType[] taskType, OnboardingTaskType[] incomingTask)
    {
        for (int i = 0; i < taskType.Length; i++)
        {
            OnboardingTaskType incoming = incomingTask[i];

            if (taskType.Any(type => type == incoming) == false || taskType.Length != incomingTask.Length)
            {
                return false;
            }
        }

        return true;
    }

    private void CheckTask()
    {
        if (player.OnboardingTaskProgress[player.OnboardingTaskLevel] >= _onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Count)
        {
            _onboardingTasksUIComponent.TaskCompleted(true);
        }
    }

    private void CopleateTask()
    {
        _onboardingTasksUIComponent.TaskCompleted(false);

        Reward(_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Reward);

        player.OnboardingTaskLevel++;

        if (TasksCountCheck())
        {
            SetReward();

            if (IsCurrentLocation())
            {
                screen.OnboardingTasksUIComponent.SetQuest($"Quest {player.OnboardingTaskLevel + 1}",
                $"{_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].TaskDescription} {player.OnboardingTaskProgress[player.OnboardingTaskLevel]}/{_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Count}",
                $"{_rewardText}",
                _spriteAsset);

                CheckTask();
            }

            else
            {
                _onboardingTasksUIComponent.NextLocationTask($"Quest {player.OnboardingTaskLevel + 1}");
            }
        }

        else
        {
            _onboardingTasksUIComponent.TasksFinish();
        }
    }

    private void Reward(int income)
    {
        if (_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].OnboardingTaskRewardTypes == OnboardingTaskRewardTypes.HardCurrency)
        {
            _onboardingTasksRewardComponent.HardCurrencyAnimate();
            GameEvents.AddHardCurrency_E(income);
        }

        else
        {
            _onboardingTasksRewardComponent.SoftCurrencyAnimate();
            GameEvents.AddCoins_E(_reward);
        }
    }


    private void SetReward()
    {
        if (_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].OnboardingTaskRewardTypes == OnboardingTaskRewardTypes.HardCurrency)
        {
            _reward = _onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Reward;
            _rewardText = $"<sprite=0> {CurrencyFormat.Format(_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].Reward)}";
            _spriteAsset = _hardCurrencySpriteAsset;
        }

        else
        {
            _reward = SoftCurrencyCount();
            _rewardText = $"<sprite=0> {CurrencyFormat.Format(SoftCurrencyCount())}";
            _spriteAsset = _softCurrencySpriteAsset;
        }
    }

    private int SoftCurrencyCount()
    {
        int resourcesCost = 0;

        for (int i = player.ResurcesSpawnLevel[player.LocationLevel]; i < player.ResurcesSpawnLevel[player.LocationLevel] + 4; i++)
        {
            resourcesCost += _resourcesConfig.Resource[i].Cost[0];
        }

        resourcesCost /= 4;
        float totalEarning;
        int averageCost = resourcesCost * 10;

        totalEarning = averageCost / (player.CollectorSpeed * 10) * 60;
        int reward = Mathf.RoundToInt(totalEarning * 10 / 100);

        return reward;
    }

    private bool IsCurrentLocation()
    {
        if (_onboardingTasksConfig.Tasks[player.OnboardingTaskLevel].LocationToOpen > player.LocationLevel)
            return false;

        return true;
    }

    private bool TasksCountCheck()
    {
        return player.OnboardingTaskLevel < _onboardingTasksConfig.Tasks.Length;
    }
}
