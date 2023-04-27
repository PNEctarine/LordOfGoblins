using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class DailyQuestsSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private DailyQuestsConfig _dailyQuestsConfig;

    private List<Quest> _bonusQuest = new List<Quest>();
    private List<Quest> _easyQuest = new List<Quest>();
    private List<Quest> _averageQuest = new List<Quest>();
    private List<Quest> _difficultQuest = new List<Quest>();

    public override void OnInit()
    {
        _bonusQuest.Add(_dailyQuestsConfig.Quests[0]);

        for (int i = 1; i < _dailyQuestsConfig.Quests.Length - 1; i++)
        {
            if (_dailyQuestsConfig.Quests[i].QuestTask[0] != 0)
            {
                _easyQuest.Add(_dailyQuestsConfig.Quests[i]);
            }

            if (_dailyQuestsConfig.Quests[i].QuestTask.Length - 1 >= 1 && _dailyQuestsConfig.Quests[i].QuestTask[1] != 0)
            {
                _averageQuest.Add(_dailyQuestsConfig.Quests[i]);
            }

            if (_dailyQuestsConfig.Quests[i].QuestTask.Length - 1 >= 2 && _dailyQuestsConfig.Quests[i].QuestTask[2] != 0)
            {
                _difficultQuest.Add(_dailyQuestsConfig.Quests[i]);
            }
        }

        if (player.GeneratedQuest == null)
        {
            player.GeneratedQuest = new List<Quest>();
            player.QuestDifficulty = new List<int>();
            GenerateQuests();
        }

        else
            SetGeneratedQuest();
    }

    private void SetGeneratedQuest()
    {
        for (int i = 0; i < player.GeneratedQuest.Count; i++)
        {
            screen.DailyQuestsUIComponent.SetQuest(i, player.GeneratedQuest[i], player.QuestDifficulty[i], _dailyQuestsConfig.DifficultyMultiplier[player.QuestDifficulty[i]], player.GeneratedQuest[i].Tasks, player.GeneratedQuest[i].TaskType);
        }
    }

    private void GenerateQuests()
    {
        List<Quest>[] questLists = { _bonusQuest, _easyQuest, _averageQuest, _difficultQuest };

        player.GeneratedQuest.Add(_dailyQuestsConfig.Quests[0]);
        player.QuestDifficulty.Add(0);
        screen.DailyQuestsUIComponent.SetQuest(0, _dailyQuestsConfig.Quests[0], 0, _dailyQuestsConfig.DifficultyMultiplier[0], _dailyQuestsConfig.Quests[0].Tasks, _dailyQuestsConfig.Quests[0].TaskType);

        for (int i = 1; i < questLists.Length; i++)
        {
            int questDifficult;

            if (i == 1)
            {
                questDifficult = 0;
            }

            else if (i == 2)
            {
                int chance = Random.Range(0, 101);

                if (chance <= 50)
                {
                    questDifficult = 0;
                }

                else
                {
                    questDifficult = 1;
                }
            }

            else
            {
                int chance = Random.Range(0, 101);

                if (chance <= 40)
                {
                    questDifficult = 0;
                }

                else if (chance > 40 && chance <= 70)
                {
                    questDifficult = 1;
                }

                else
                {
                    questDifficult = 2;
                }
            }

            player.GeneratedQuest.Add(questLists[i][questDifficult]);
            player.QuestDifficulty.Add(questDifficult);
            screen.DailyQuestsUIComponent.SetQuest(i, questLists[i][questDifficult], questDifficult, _dailyQuestsConfig.DifficultyMultiplier[questDifficult], questLists[i][questDifficult].Tasks, questLists[i][questDifficult].TaskType);
        }
    }
}
