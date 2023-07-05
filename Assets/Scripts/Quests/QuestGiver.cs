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
        public Waypoint2D waypoint2D;

        private WaypointManager _waypointManager;

        public void EvaluateWaypoint(WaypointManager waypointManager)
        {
            if (quest != null) {
                _waypointManager = waypointManager;
                var icon = QuestManager.Current.newQuestWaypoint;
                waypoint = _waypointManager.AddWaypoint(icon, transform);
                
                waypoint2D = Instantiate(_waypointManager.waypoint2DPrefab, transform).GetComponent<Waypoint2D>();
                waypoint2D.waypointImage.sprite = icon;
            }
        }

        [YarnCommand("give_quest")]
        public void Give()
        {
            NetworkClient.localPlayer.GetComponent<InteractionManager>().AddQuest(quest);
            _waypointManager.RemoveWaypoint(waypoint);
            Destroy(waypoint2D.gameObject);
        }
    }
}