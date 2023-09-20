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

        private void Update()
        {
            var questMarkers = FindObjectsOfType<QuestMarker>();

            Debug.Log(questMarkers.Length);

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var marker in questMarkers)
            {
                Debug.Log("Manager: Try EvaluateWaypoint for " + marker.gameObject.name);
                marker.EvaluateWaypoint(this);
            }
        }

        public Waypoint AddWaypoint(Sprite icon, Vector3 position)
        {
            Camera mapCam = useMapCam ? GameObject.Find("CameraMap").GetComponent<Camera>() : null ;

            Debug.Log("Add new waypoint");
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