using System;
using Code.Enums;
using UnityEngine;

[Serializable]
public struct Quest
{
    public OnboardingTasks Tasks;
    public OnboardingTaskType TaskType;

    [Space(10)]
    public string QuestName;
    public string Description;
    public float[] QuestTask;
}

[CreateAssetMenu(menuName = "Config/DailyQuestsConfig")]
public class DailyQuestsConfig : ScriptableObject
{
    public float[] DifficultyMultiplier;
    public Quest[] Quests;
}
