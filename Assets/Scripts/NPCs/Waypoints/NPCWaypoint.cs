using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoint : MonoBehaviour
{
    public float delayTime = 0f; // Delay für den Waypoint

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
