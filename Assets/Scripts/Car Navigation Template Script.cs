using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarNavigator : MonoBehaviour
{
    [Header("Path")]
    public Transform[] waypoints;
    [Tooltip("Agent max speed.")]
    public float speed = 10f;
    [Tooltip("Threshold to consider a waypoint reached.")]
    public float waypointThreshold = 1f;

    [Header("Spacing")]
    public Transform precedingCar; // set by the order starter
    [Tooltip("Hard minimum gap to maintain behind preceding car.")]
    public float minGap = 2f;
    [Tooltip("Additional buffer over which speed ramps up smoothly.")]
    public float spacingBuffer = 1f;

    [Header("Start offset")]
    [Tooltip("Which waypoint to start from (0-based). Assigned by the order starter.")]
    public int startIndex = 0;

    private NavMeshAgent agent;
    private int currentIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; // no built-in slowdowns
        agent.acceleration = 50f;  // responsive
        agent.angularSpeed = 720f; // quick turning
    }

    void OnEnable()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError($"[{nameof(CarNavigator)}] No waypoints assigned.");
            enabled = false;
            return;
        }

        currentIndex = Mathf.Clamp(startIndex, 0, waypoints.Length - 1);
        agent.isStopped = false;
        agent.speed = speed;
        agent.SetDestination(waypoints[currentIndex].position);
    }

    void Update()
    {
        if (agent.pathPending) return;

        // Enforce spacing from preceding car with smooth ramp
        if (precedingCar != null)
        {
            float distToPrev = Vector3.Distance(transform.position, precedingCar.position);
            if (distToPrev < minGap)
            {
                agent.isStopped = true;
            }
            else
            {
                // within buffer zone: scale speed linearly from 0 to full
                float effectiveSpeed = speed;
                if (distToPrev < minGap + spacingBuffer)
                {
                    float t = (distToPrev - minGap) / spacingBuffer;
                    effectiveSpeed = Mathf.Lerp(0f, speed, t);
                }

                if (agent.isStopped)
                    agent.isStopped = false;
                agent.speed = effectiveSpeed;
                // reissue destination if needed to keep moving
                if (agent.remainingDistance <= waypointThreshold && currentIndex < waypoints.Length)
                    agent.SetDestination(waypoints[currentIndex].position);
            }
        }
        else
        {
            // no preceding car: ensure full speed
            agent.isStopped = false;
            agent.speed = speed;
        }

        // Advance through waypoints sequentially
        if (!agent.isStopped && agent.remainingDistance <= waypointThreshold)
        {
            if (currentIndex < waypoints.Length - 1)
            {
                currentIndex++;
                agent.SetDestination(waypoints[currentIndex].position);
            }
            else
            {
                // final waypoint reached
                agent.isStopped = true;
            }
        }
    }
}
