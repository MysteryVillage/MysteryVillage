using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public NPCWaypoint[] waypoints;
    private int currentWaypointIndex = 0;

    public bool isLooping = true;

    private int direction = 1;
    private float currentWaitTime = 0f;
    private bool isWaiting = false;

    private Animator animator; // Verweis auf den Animator des NPCs

    void Start()
    {
        if (waypoints.Length > 0)
        {
            SetDestinationToWaypoint(waypoints[currentWaypointIndex]);
        }

        animator = GetComponent<Animator>(); // Animator-Komponente holen
    }

    void Update()
    {
        // Vor der Aktualisierung der Animation, sicherstellen, dass die Position des Agents und des NPCs synchron sind
        transform.position = agent.transform.position;

        if (isWaiting)
        {
            animator.SetBool("IsWaiting", true); // Aktiviert die Idle-Animation
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0f)
            {
                isWaiting = false;
                animator.SetBool("IsWaiting", false); // Deaktiviert die Idle-Animation, wenn der NPC weitergeht
                GoToNextWaypoint();
            }
            return; // Warten, bis die Verzögerung abgelaufen ist
        }

        if (agent.remainingDistance < 0.1f)
        {
            if (waypoints[currentWaypointIndex].delayTime > 0)
            {
                // Der aktuelle Waypoint hat eine Verzögerung, aktiviere die Idle-Animation
                animator.SetBool("IsWaiting", true);
                currentWaitTime = waypoints[currentWaypointIndex].delayTime;
                isWaiting = true;
            }
            else
            {
                // Der aktuelle Waypoint hat keine Verzögerung, aktiviere die Laufanimation
                animator.SetBool("IsWaiting", false);

                if (isLooping)
                {
                    GoToNextWaypoint();
                }
                else
                {
                    currentWaypointIndex += direction;

                    if (currentWaypointIndex >= waypoints.Length || currentWaypointIndex < 0)
                    {
                        direction *= -1;
                        currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, waypoints.Length - 1);
                    }

                    SetDestinationToWaypoint(waypoints[currentWaypointIndex]);
                }
            }
        }
    }

    void GoToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + direction) % waypoints.Length;

        if (currentWaypointIndex == 0 && direction == -1)
        {
            direction = 1;
        }

        SetDestinationToWaypoint(waypoints[currentWaypointIndex]);
    }

    void SetDestinationToWaypoint(NPCWaypoint waypoint)
    {
        agent.SetDestination(waypoint.transform.position);
    }
}
