using Mirror;
using NPC;
using Quests.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Quests.Goals
{
    public class GoToGoal : QuestGoal
    {
        public GameObject triggerGO;
        public new void Init(int questId)
        {
            base.Init(questId);
            
            var triggers = FindObjectsOfType<QuestGoalTrigger>();
            
            foreach (var trigger in triggers)
            {
                if (trigger.goal.Description == Description)
                {
                    trigger.OnGoalTrigger.AddListener(TargetReached);
                    triggerGO = trigger.gameObject;
                    trigger.AddComponent<QuestMarker>();
                    trigger.GetComponent<QuestMarker>().quest = GetQuest();
                }
            }
        }

        public void TargetReached()
        {
            CurrentAmount++;
            Evaluate();
            Destroy(triggerGO.GetComponent<QuestMarker>());
        }

        public GoToGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
        }

        public static void WriteGoToGoal(NetworkWriter writer, QuestGoal goal)
        {
            
        }

        public static GoToGoal ReadGoToGoal(NetworkReader reader)
        {
            var goal = CreateInstance<GoToGoal>();

            return goal;
        }
    }
}