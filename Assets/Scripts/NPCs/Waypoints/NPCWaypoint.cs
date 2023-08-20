using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoint : MonoBehaviour
{
    public float delayTime = 0f; // Verzögerungszeit an diesem Waypoint in Sekunden

    // Wenn Sie zusätzliche Eigenschaften für Ihre Wegpunkte benötigen, können Sie sie hier hinzufügen

    // Optional können Sie gizmos verwenden, um die Wegpunkte im Unity-Editor anzuzeigen.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
