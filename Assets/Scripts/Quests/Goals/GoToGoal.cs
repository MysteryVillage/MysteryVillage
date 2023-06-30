using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Quests.Goals
{
    public class GoToGoal : QuestGoal
    {
        public new void Init()
        {
            base.Init();
            
            var triggers = FindObjectsOfType<QuestGoalTrigger>();
            
            foreach (var trigger in triggers)
            {
                if (trigger.goal.Description == Description)
                {
                    trigger.OnGoalTrigger.AddListener(TargetReached);
                }
            }
        }

        public void TargetReached()
        {
            CurrentAmount++;
            Evaluate();
        }

        public GoToGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
        }
    }
}