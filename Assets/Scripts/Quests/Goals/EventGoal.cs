using Events;
using Items;
using Mirror;
using UnityEngine;

namespace Quests.Goals
{
    public class EventGoal : QuestGoal
    {
        public string listenFor;
        
        public EventGoal(string description, int currentAmount, int requiredAmount, bool completed) : base(description, currentAmount, requiredAmount, completed)
        {
        }

        public new void Init(int questId)
        {
            Debug.Log("Init EventGoal");
            base.Init(questId);
            
            EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
            if (eventSystem)
            {
                eventSystem.onQuestEvent.AddListener(OnQuestEvent);
                Debug.Log("Add Listener");
            }
        }

        public void OnQuestEvent(string questEvent)
        {
            if (questEvent != listenFor) return;

            CurrentAmount++;
            Evaluate();
            QuestManager.Current.OnGoalUpdated.Invoke(this);
        }
        
        public static void WriteEventGoal(NetworkWriter writer, QuestGoal goal)
        {
            EventGoal eventGoal = goal as EventGoal;
            if (eventGoal == null) return;
            
            writer.WriteString(eventGoal.listenFor);
        }

        public static EventGoal ReadEventGoal(NetworkReader reader)
        {
            var goal = CreateInstance<EventGoal>();

            goal.listenFor = reader.ReadString();

            return goal;
        }
    }
}