using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public NPCWaypoint[] waypoints;
    public Animator animator;

    public float detectionRadius = 5f; // Der Radius, in dem der NPC den Spieler erkennt.
    public string playerTag = "Player";

    private int currentWaypointIndex = 0;
    public bool isLooping = true;
    private int direction = 1;
    private float currentWaitTime = 0f;
    private float speed = 0f;
    private bool isRotating = false;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            SetDestinationToWaypoint(waypoints[currentWaypointIndex]);
        }
    }

    void Update()
    {
        transform.position = agent.transform.position;

        GameObject player = GameObject.FindWithTag(playerTag);
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;

               
                if (currentWaitTime > 0f || isRotating)
                {
                    currentWaitTime -= Time.deltaTime;
                    if (currentWaitTime <= 0f)
                    {
                        currentWaitTime = 0f;
                        isRotating = false;
                        GoToNextWaypoint();
                    }
                }
                else
                {
                    if (agent.remainingDistance < 0.1f)
                    {
                        if (waypoints[currentWaypointIndex].delayTime > 0)
                        {
                            currentWaitTime = waypoints[currentWaypointIndex].delayTime;
                        }
                        else
                        {
                            GoToNextWaypoint();

                            if (!isLooping && (currentWaypointIndex == 0 || currentWaypointIndex == waypoints.Length - 1))
                            {
                                direction *= -1; // Richtung umkehren
                            }
                        }
                    }
                }
            }
        }

        // Setze den Speed-Parameter im Animator auf die Geschwindigkeit des NPCs.
        speed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", speed);
    }

    void GoToNextWaypoint()
    {
        currentWaypointIndex += direction;

        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
        else if (currentWaypointIndex < 0)
        {
            currentWaypointIndex = waypoints.Length - 1;
        }

        SetDestinationToWaypoint(waypoints[currentWaypointIndex]);
    }

    void SetDestinationToWaypoint(NPCWaypoint waypoint)
    {
        agent.SetDestination(waypoint.transform.position);
    }

    void RotateTowardsNextWaypoint()
    {
        isRotating = true;
        Vector3 directionToNextWaypoint = waypoints[currentWaypointIndex].transform.position - transform.position;
        directionToNextWaypoint.y = 0;
        Quaternion rotation = Quaternion.LookRotation(directionToNextWaypoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * agent.angularSpeed);
    }
}
