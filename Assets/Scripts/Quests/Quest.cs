using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Quests.Goals;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Quests
{
    [Serializable]
    public class Quest : ScriptableObject, IEquatable<Quest>
    {
        [Serializable]
        public struct Info
        {
            public string name;
            public string description;
        }
        
        [Header("Info")]
        public Info Information;
        
        public bool Completed { get; private set; }
        public QuestCompletedEvent QuestCompleted;

        public List<QuestGoal> goals = new List<QuestGoal>();

        public Quest followUpQuest;

        public virtual void Init()
        {
            Debug.Log("Init Quest: " + Information.name + ". Number of Goals: " + goals.Count);
            Completed = false;
            QuestCompleted = new QuestCompletedEvent();

            foreach (var goal in goals)
            {
                Debug.Log(goal.GetType());
                if (goal is GoToGoal goToGoal)
                {
                    Debug.Log("Try to init go to goal");
                    goToGoal.Init();
                } else if (goal is TalkToGoal talkToGoal)
                {
                    Debug.Log("Try to init talk to goal");
                    talkToGoal.Init();
                } else if (goal is CollectGoal collectGoal)
                {
                    Debug.Log("Try to init collect goal");
                    collectGoal.Init();
                } else if (goal is BringToGoal bringToGoal)
                {
                    Debug.Log("Try to init bring to goal");
                    bringToGoal.Init();
                } else if (goal is EventGoal eventGoal)
                {
                    Debug.Log("Try to init event goal");
                    eventGoal.Init();
                }
                else
                {
                    Debug.Log("Try to init default goal");
                    goal.Init();
                }
                goal.QuestGoalCompleted.AddListener(CheckGoals);

            }

            var eventsystem = EventSystem.current as Events.EventSystem;
            if (eventsystem != null) eventsystem.onQuestStart.Invoke(this);
        }

        protected void CheckGoals()
        {
            Debug.Log(name + ": Check Goals");
            Completed = goals.All(g => g.Completed);
            if (Completed)
            {
                Debug.Log(name + ": All goals completed");
                QuestCompleted.Invoke(this);
                if (followUpQuest != null) QuestManager.Current.SetNewQuest(followUpQuest);
                
                Events.EventSystem eventSystem = EventSystem.current as Events.EventSystem;
                if (eventSystem)
                {
                    eventSystem.onQuestFinish.Invoke(this);
                }
            }
        }

        public override string ToString()
        {
            return Information.name;
        }

        public bool Equals(Quest other)
        {
            if (other == null) return false;
            return Information.name + Information.description == other.Information.name + other.Information.description;
        }
        
        public int GetId()
        {
            return FindId(this);
        }
        
        public static Quest FindById(int questId)
        {
            return QuestManager.Current.availableQuests[questId];
        }

        public static int FindId(Quest quest)
        {
            return QuestManager.Current.availableQuests.FindIndex(x => x.Equals(quest));
        }
    }

    public class QuestCompletedEvent : UnityEvent<Quest>
    {
    }
}

// public static class CustomReadWriteFunctions
// {
//     public static void WriteList(this NetworkWriter writer, List<QuestGoal> list)
//     {
//         NetworkIdentity networkIdentity = list.GetComponent<NetworkIdentity>();
//         writer.WriteNetworkIdentity(networkIdentity);
//     }
//
//     public static Rigidbody ReadRigidbody(this NetworkReader reader)
//     {
//         NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
//         Rigidbody rigidBody = networkIdentity != null
//             ? networkIdentity.GetComponent<Rigidbody>()
//             : null;
//
//         return rigidBody;
//     }
// }