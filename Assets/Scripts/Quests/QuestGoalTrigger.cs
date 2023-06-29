using System;
using Mirror;
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
            Debug.Log("Trigger entered: " + other.gameObject.name);
            var player = NetworkClient.localPlayer;
            if (other.gameObject.GetComponent<NetworkIdentity>() == player)
            {
                OnGoalTrigger.Invoke();
            }
        }
    }
}