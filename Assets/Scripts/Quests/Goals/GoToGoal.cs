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
            Debug.Log("Go To Goal: Init()");
            var triggers = FindObjectsOfType<QuestGoalTrigger>();
            foreach (var trigger in triggers)
            {
                if (trigger.goal == this)
                {
                    trigger.OnGoalTrigger.AddListener(TargetReached);
                }
            }
        }

        public void TargetReached()
        {
            Debug.Log("Target Reached");
            CurrentAmount++;
            Evaluate();
        }

        public GoToGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
        }
    }
}