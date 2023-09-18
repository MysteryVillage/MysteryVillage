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
            CurrentAmount = 0;
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
        // Write type of quest goal
        writer.WriteString(questGoal.GetType().ToString());
        
        
        // determine type and execute type-specific write method
        if (questGoal.GetType() == typeof(CollectGoal))
        {
            CollectGoal.WriteCollectGoal(writer, questGoal);
        } else if (questGoal.GetType() == typeof(TalkToGoal))
        {
            TalkToGoal.WriteTalkToGoal(writer, questGoal);
        } else if (questGoal.GetType() == typeof(GoToGoal))
        {
            GoToGoal.WriteGoToGoal(writer, questGoal);
        } else if (questGoal.GetType() == typeof(BringToGoal))
        {
            BringToGoal.WriteBringToGoal(writer, questGoal);
        } else if (questGoal.GetType() == typeof(EventGoal))
        {
            EventGoal.WriteEventGoal(writer, questGoal);
        }
        
        // write general data
        writer.WriteString(questGoal.Description);
        writer.WriteInt(questGoal.CurrentAmount);
        writer.WriteInt(questGoal.RequiredAmount);
        writer.WriteBool(questGoal.Completed);
    }

    public static QuestGoal ReadQuestGoal(this NetworkReader reader)
    {
        // read quest goal type
        var type = reader.ReadString();
        var classType = Type.GetType(type);

        QuestGoal goal = null;
        
        // determine type and execute type-specific read method
        if (classType == typeof(CollectGoal))
        {
            goal = CollectGoal.ReadCollectGoal(reader);
        } else if (classType == typeof(TalkToGoal))
        {
            goal = TalkToGoal.ReadTalkToGoal(reader);
        } else if (classType == typeof(GoToGoal))
        {
            goal = GoToGoal.ReadGoToGoal(reader);
        } else if (classType == typeof(BringToGoal))
        {
            goal = BringToGoal.ReadBringToGoal(reader);
        } else if (classType == typeof(EventGoal))
        {
            goal = EventGoal.ReadEventGoal(reader);
        }

        // if goal was not initialized at this point, abort
        if (goal == null) return null;

        // read general data
        goal.Description = reader.ReadString();
        goal.CurrentAmount = reader.ReadInt();
        goal.RequiredAmount = reader.ReadInt();
        goal.Completed = reader.ReadBool();

        return goal;
    }
}