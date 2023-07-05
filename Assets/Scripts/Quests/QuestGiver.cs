using System;
using Mirror;
using Player;
using Quests.UI;
using UI;
using UnityEngine;
using Yarn.Unity;

namespace Quests
{
    public class QuestGiver : NetworkBehaviour
    {
        public Quest quest;
        public Waypoint waypoint;

        private WaypointManager _waypointManager;

        public void EvaluateWaypoint(WaypointManager waypointManager)
        {
            _waypointManager = waypointManager;
            waypoint = _waypointManager.AddWaypoint(QuestManager.Current.newQuestWaypoint, transform);
        }

        [YarnCommand("give_quest")]
        public void Give()
        {
            NetworkClient.localPlayer.GetComponent<InteractionManager>().AddQuest(quest);
            _waypointManager.RemoveWaypoint(waypoint);
        }
    }
}