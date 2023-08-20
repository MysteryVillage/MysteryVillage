using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public NPCWaypoint[] waypoints; // Jetzt verwenden wir den Typ NPCWaypoint
    private int currentWaypointIndex = 0;

    public bool isLooping = true;

    private int direction = 1;
    private float currentWaitTime = 0f;
    private bool isWaiting = false;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            SetDestinationToWaypoint(waypoints[currentWaypointIndex]);
        }
    }

    void Update()
    {
        if (isWaiting)
        {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0f)
            {
                isWaiting = false;
                GoToNextWaypoint();
            }
            return;
        }

        if (agent.remainingDistance < 0.1f)
        {
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
        if (waypoint.delayTime > 0)
        {
            currentWaitTime = waypoint.delayTime;
            isWaiting = true;
        }
    }
}
