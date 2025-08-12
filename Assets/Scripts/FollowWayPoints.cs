using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class FollowWayPoints : MonoBehaviour
{
    [Header("Waypoint Settings")]
    public Transform waypointParent; // Parent of all waypoint children
    public float stoppingDistance = 0.5f;

    private NavMeshAgent agent;
    private List<Transform> waypoints = new List<Transform>();
    private int currentIndex = 0;
    private int direction = 1; // 1 = forward, -1 = backward

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypointParent == null)
        {
            Debug.LogError("Waypoint parent not assigned.");
            enabled = false;
            return;
        }

        // Add all active children of the parent as waypoints
        foreach (Transform child in waypointParent)
        {
            waypoints.Add(child);
        }

        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints found under the assigned parent.");
            enabled = false;
            return;
        }

        MoveToWaypoint(currentIndex);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            // Move index forward or backward
            currentIndex += direction;

            // Reverse direction at ends
            if (currentIndex >= waypoints.Count)
            {
                currentIndex = waypoints.Count - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }

            MoveToWaypoint(currentIndex);
        }
    }

    void MoveToWaypoint(int index)
    {
        if (index >= 0 && index < waypoints.Count)
        {
            agent.SetDestination(waypoints[index].position);
        }
    }
}
