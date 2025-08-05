using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class PedestrianWaypointNavigator : MonoBehaviour
{
    [Tooltip("Drag the parent object containing all waypoint children here.")]
    public Transform waypointParent;

    private NavMeshAgent agent;
    private List<Transform> waypoints = new List<Transform>();
    private int currentIndex = 0;
    private int direction = 1;  // 1 = forward, -1 = backward

    public float arrivalThreshold = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypointParent == null)
        {
            Debug.LogError("Waypoint parent is not assigned!");
            return;
        }

        foreach (Transform child in waypointParent)
        {
            waypoints.Add(child);
        }

        if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints found under the parent object!");
            return;
        }

        MoveToWaypoint();
    }

    void Update()
    {
        if (waypoints.Count == 0 || agent.pathPending)
            return;

        if (agent.remainingDistance <= arrivalThreshold && !agent.pathPending)
        {
            currentIndex += direction;

            // If reached the end, flip direction
            if (currentIndex >= waypoints.Count || currentIndex < 0)
            {
                direction *= -1;  // Reverse direction
                currentIndex += direction * 2; // Step back inside bounds
            }

            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (currentIndex >= 0 && currentIndex < waypoints.Count)
        {
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }
}
