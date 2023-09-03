using System;
using System.Collections.Generic;
using Quests;
using Quests.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class WaypointManager : MonoBehaviour
    {
        public GameObject waypointPrefab;
        public GameObject waypoint2DPrefab;
        public bool useMapCam;
        
        public List<Waypoint> activeWaypoints;

        private void Start()
        {
            var questGivers = FindObjectsOfType<QuestGiver>();

            foreach (var giver in questGivers)
            {
                giver.EvaluateWaypoint(this);
            }
        }

        public Waypoint AddWaypoint(Sprite icon, Transform position)
        {
            Camera mapCam = useMapCam ? GameObject.Find("CameraMap").GetComponent<Camera>() : null ;
            
            var newWaypoint = Instantiate(waypointPrefab, transform).GetComponent<Waypoint>();
            newWaypoint.icon = icon;
            newWaypoint.target = position;
            newWaypoint.cam = useMapCam ? mapCam : Camera.main;
            
            activeWaypoints.Add(newWaypoint);

            return newWaypoint;
        }

        public void RemoveWaypoint(Waypoint waypoint)
        {
            activeWaypoints.Remove(waypoint);
            Destroy(waypoint.gameObject);
        }
    }
}