using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoint : MonoBehaviour
{
    public float delayTime = 0f; // Verz�gerungszeit an diesem Waypoint in Sekunden

    // Wenn Sie zus�tzliche Eigenschaften f�r Ihre Wegpunkte ben�tigen, k�nnen Sie sie hier hinzuf�gen

    // Optional k�nnen Sie gizmos verwenden, um die Wegpunkte im Unity-Editor anzuzeigen.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
