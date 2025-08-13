using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AgentWaypointNavigator : MonoBehaviour
{
    [Header("Waypoint Settings")]
    public Transform waypointParent; // Parent of all waypoint children
    public float stoppingDistance = 0.5f;

    private NavMeshAgent agent;
    private List<Transform> waypoints = new List<Transform>();
    private HashSet<Transform> visited = new HashSet<Transform>();
    private Transform currentTarget;

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

        MoveToNextClosestWaypoint();
    }

    void Update()
    {
        if (currentTarget != null && !agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            visited.Add(currentTarget);
            currentTarget = null;

            if (visited.Count < waypoints.Count)
                MoveToNextClosestWaypoint();
            else
                agent.isStopped = true; // All waypoints visited
        }
    }

    void MoveToNextClosestWaypoint()
    {
        float shortestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (Transform wp in waypoints)
        {
            if (visited.Contains(wp)) continue;

            float dist = Vector3.Distance(transform.position, wp.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closest = wp;
            }
        }

        if (closest != null)
        {
            currentTarget = closest;
            agent.SetDestination(currentTarget.position);
        }
    }
}
