using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BikeMover : MonoBehaviour
{
    public Ebike ebikeScript;   // Drag Ebike GameObject here in Inspector
    public float stoppingDistance = 0.5f;

    private List<Vector3> path;
    private int currentIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        LoadFirstPath();
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            currentIndex++;
            if (currentIndex < path.Count)
            {
                agent.SetDestination(path[currentIndex]);
            }
            else
            {
                LoadNextPath();
            }
        }
    }

    private void LoadFirstPath()
    {
        path = ebikeScript.GetFirstPath();
        if (path == null || path.Count == 0)
        {
            Debug.LogError("No path data found!");
            agent.isStopped = true;
            return;
        }

        transform.position = path[0];
        currentIndex = 1;

        if (currentIndex < path.Count)
            agent.SetDestination(path[currentIndex]);
    }

    private void LoadNextPath()
    {
        path = ebikeScript.GetNextPath();
        if (path == null || path.Count == 0)
        {
            Debug.Log("All paths completed.");
            agent.isStopped = true;
            enabled = false;
            return;
        }

        transform.position = path[0];
        currentIndex = 1;

        if (currentIndex < path.Count)
            agent.SetDestination(path[currentIndex]);
    }
}
