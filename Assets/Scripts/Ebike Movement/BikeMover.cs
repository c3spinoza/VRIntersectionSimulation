using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // <-- Needed for NavMeshAgent

public class BikeMover : MonoBehaviour
{
    public Ebike ebikeScript;   // Drag the GameObject with Ebike.cs into this field
    public float stoppingDistance = 0.5f;

    private List<Vector3> path;
    private int currentIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Get the first path from Ebike
        path = ebikeScript.GetFirstPath();

        if (path == null || path.Count == 0)
        {
            Debug.LogError("No path data found!");
        }
        else
        {
            transform.position = path[0]; // snap to first waypoint
            currentIndex = 1; // start moving toward the second point
            if (currentIndex < path.Count)
            {
                agent.SetDestination(path[currentIndex]);
            }
        }
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;

        // Check if we've reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            currentIndex++;
            if (currentIndex < path.Count)
            {
                agent.SetDestination(path[currentIndex]);
            }
            else
            {
                Debug.Log("Finished path!");
                agent.isStopped = true;
                enabled = false;
            }
        }
    }
}
