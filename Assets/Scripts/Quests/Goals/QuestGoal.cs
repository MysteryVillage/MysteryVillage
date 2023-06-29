using System;
using System.Collections.Generic;
using Mirror;
using Quests.Goals;
using UnityEngine;
using UnityEngine.Events;

namespace Quests.Goals
{
    [Serializable]
    public abstract class QuestGoal : ScriptableObject
    {
        public string Description;
        
        public int CurrentAmount { get; set; }
        public int RequiredAmount = 1;
        
        public bool Completed { get; set; }
        public UnityEvent QuestGoalCompleted;

        public QuestGoal(string description, int currentAmount, int requiredAmount, bool completed)
        {
            Description = description;
            CurrentAmount = currentAmount;
            RequiredAmount = requiredAmount;
            Completed = completed;
        }

        public void Init()
        {
            Completed = false;
            QuestGoalCompleted = new UnityEvent();
        }

        protected void Evaluate()
        {
            if (CurrentAmount >= RequiredAmount)
            {
                Complete();
            }
        }

        void Complete()
        {
            Debug.Log("Quest Goal completed: " + Description);
            Completed = true;
            QuestGoalCompleted.Invoke();
            QuestGoalCompleted.RemoveAllListeners();
        }
    }
}

public static class CustomReadWriteFunctions
{
    public static void WriteQuestGoal(this NetworkWriter writer, QuestGoal questGoal)
    {
        writer.WriteString(questGoal.Description);
        writer.WriteInt(questGoal.CurrentAmount);
        writer.WriteInt(questGoal.RequiredAmount);
        writer.WriteBool(questGoal.Completed);
        // writer.Write(questGoal.Completed);
    }

    public static QuestGoal ReadQuestGoal(this NetworkReader reader)
    {
        var goal = ScriptableObject.CreateInstance<GoToGoal>();
        goal.Description = reader.ReadString();
        goal.CurrentAmount = reader.ReadInt();
        goal.RequiredAmount = reader.ReadInt();
        goal.Completed = reader.ReadBool();

        return goal;
    }
}