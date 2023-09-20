using NPC;
using Quests.Goals;
using UI;
using UnityEngine;

namespace Quests.UI
{
    public class QuestMarker : MonoBehaviour
    {
        public Quest quest;
        public Waypoint waypoint;
        public Waypoint2D waypoint2D;
        public QuestMarkerType type;

        private WaypointManager _waypointManager;
        
        public void EvaluateWaypoint(WaypointManager waypointManager)
        {
            _waypointManager = waypointManager;
            var boxCollider = GetComponent<BoxCollider>();
            Vector3 pos;
            if (boxCollider != null)
            {
                type = QuestMarkerType.trigger;
                pos = boxCollider.bounds.center;
            }
            else
            {
                type = QuestMarkerType.npc;
                pos = transform.position;
            }
            var icon = QuestManager.Current.newQuestWaypoint;
            
            waypoint = _waypointManager.AddWaypoint(icon, pos);
            
            waypoint2D = Instantiate(_waypointManager.waypoint2DPrefab, transform).GetComponent<Waypoint2D>();
            waypoint2D.waypointImage.sprite = icon;
        }

        public enum QuestMarkerType
        {
            trigger,
            npc
        }
    }
}