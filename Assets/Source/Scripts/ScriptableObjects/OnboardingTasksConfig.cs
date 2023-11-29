using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using UnityEngine;

[Serializable]
public struct Tasks
{
    public OnboardingTasks Task;
    public OnboardingTaskType[] TaskType;
    public string TaskDescription;
    public int Count;
    public OnboardingTaskRewardTypes OnboardingTaskRewardTypes;
    public int Reward;
    public int LocationToOpen;
    public bool IsPassive;
}

[CreateAssetMenu(menuName = "Config/OnboardingTasksConfig")]
public class OnboardingTasksConfig : ScriptableObject
{
    public Tasks[] Tasks;
}
