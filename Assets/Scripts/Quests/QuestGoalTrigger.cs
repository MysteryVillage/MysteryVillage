using System;
using Mirror;
using Quests;
using Quests.Goals;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class QuestGoalTrigger : MonoBehaviour
    {
        public UnityEvent OnGoalTrigger;

        public GoToGoal goal;

        private void OnTriggerEnter(Collider other)
        {
            var player = NetworkClient.localPlayer;
            if (other.gameObject.GetComponent<NetworkIdentity>() == player)
            {
                player.GetComponent<PlayerQuestController>().OnGoalCollision(this);
            }
        }
    }
}

public static class QuestGoalTriggerReadWriteFunctions
{
    public static void WriteQuestGoalTrigger(this NetworkWriter writer, QuestGoalTrigger questGoalTrigger)
    {
        NetworkIdentity networkIdentity = questGoalTrigger.GetComponent<NetworkIdentity>();
        writer.WriteNetworkIdentity(networkIdentity);
    }

    public static QuestGoalTrigger ReadQuestGoalTrigger(this NetworkReader reader)
    {
        NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
        QuestGoalTrigger trigger = networkIdentity != null
            ? networkIdentity.GetComponent<QuestGoalTrigger>()
            : null;

        return trigger;
    }
}