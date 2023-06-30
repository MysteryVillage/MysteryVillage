using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Quests.Goals;
using UnityEngine;
using UnityEngine.Events;

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

        public virtual void Init()
        {
            Debug.Log("Init Quest: " + Information.name + ". Number of Goals: " + goals.Count);
            Completed = false;
            QuestCompleted = new QuestCompletedEvent();

            foreach (var goal in goals)
            {
                if (goal is GoToGoal goToGoal)
                {
                    goToGoal.Init();
                } else if (goal is TalkToGoal talkToGoal)
                {
                    talkToGoal.Init();
                } else if (goal is CollectGoal collectGoal)
                {
                    collectGoal.Init();
                }
                else
                {
                    goal.Init();
                }
                goal.QuestGoalCompleted.AddListener(CheckGoals);
            }
        }

        protected void CheckGoals()
        {
            Debug.Log(name + ": Check Goals");
            Completed = goals.All(g => g.Completed);
            if (Completed)
            {
                Debug.Log(name + ": All goals completed");
                QuestCompleted.Invoke(this);
            }
        }

        public override string ToString()
        {
            return Information.name;
        }

        public bool Equals(Quest other)
        {
            if (other == null) return false;
            return Information.name == other.Information.name;
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