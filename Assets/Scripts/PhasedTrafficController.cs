using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhasedTrafficController : MonoBehaviour
{
    [Header("Lane Parents (assign these)")]
    public GameObject northRightLane;    // Heading North Right Lane
    public GameObject southRightLane;    // Heading South Right Lane
    public GameObject northLeftLane;     // Heading North Left Lane
    public GameObject southLeftLane;     // Heading South Left Lane
    public GameObject east120Lane;       // Heading East 120 Lane
    public GameObject west120Lane;       // Heading West 120 Lane

    [Header("Timing (seconds)")]
    public float phase1Duration = 15f;   // N–R & S–R
    public float phase2Duration = 15f;   // N–L & S–L
    public float phase3Duration = 15f;   // E–120 & W–120

    [Header("Lane Start Behavior")]
    [Tooltip("Delay between starting cars within the SAME lane (0 = all at once).")]
    public float staggerBetweenCars = 0.35f;
    [Tooltip("If true, previous phases are frozen when a new phase begins.")]
    public bool exclusivePhases = false;

    // Cached ordered cars per lane (front -> back)
    private List<CarNavigator> nR, sR, nL, sL, e120, w120;

    // ---------------------------------------------------------------------

    void Awake()
    {
        // Hard-freeze everything so nothing creeps before phase 1
        FreezeLane(northRightLane);
        FreezeLane(southRightLane);
        FreezeLane(northLeftLane);
        FreezeLane(southLeftLane);
        FreezeLane(east120Lane);
        FreezeLane(west120Lane);
    }

    void Start()
    {
        // Build leader/follower chains once (front -> back). We do NOT edit waypoints here.
        nR   = OrderLaneAndLink(northRightLane);
        sR   = OrderLaneAndLink(southRightLane);
        nL   = OrderLaneAndLink(northLeftLane);
        sL   = OrderLaneAndLink(southLeftLane);
        e120 = OrderLaneAndLink(east120Lane);
        w120 = OrderLaneAndLink(west120Lane);

        StartCoroutine(RunPhases());
    }

    // ---------------------------------------------------------------------

    private IEnumerator RunPhases()
    {
        // PHASE 1: North & South RIGHT
        if (exclusivePhases) FreezeAll();
        yield return StartCoroutine(StartBoth(nR, sR, phase1Duration));

        // PHASE 2: North & South LEFT
        if (exclusivePhases) { FreezeList(nR); FreezeList(sR); }
        yield return StartCoroutine(StartBoth(nL, sL, phase2Duration));

        // PHASE 3: East & West 120th
        if (exclusivePhases) { FreezeList(nL); FreezeList(sL); }
        yield return StartCoroutine(StartBoth(e120, w120, phase3Duration));

        // Optional loop:
        // StartCoroutine(RunPhases());
    }

    private IEnumerator StartBoth(List<CarNavigator> a, List<CarNavigator> b, float holdSeconds)
    {
        StartCoroutine(StartLaneStaggered(a));  // starts FRONT car first
        StartCoroutine(StartLaneStaggered(b));
        yield return new WaitForSeconds(holdSeconds);
    }

    // ---------------------------------------------------------------------
    // Lane ordering & starting
    // ---------------------------------------------------------------------

    private List<CarNavigator> OrderLaneAndLink(GameObject laneParent)
    {
        var ordered = new List<CarNavigator>();
        if (!laneParent) return ordered;

        var cars = laneParent.GetComponentsInChildren<CarNavigator>(true);
        if (cars.Length == 0) return ordered;

        ordered.AddRange(cars);

        // Determine travel direction from any car's waypoints (preferred),
        // otherwise fallback to the lane parent's forward.
        Vector3 dir = Vector3.zero;
        foreach (var c in ordered)
        {
            var wps = c.waypoints;
            if (wps != null && wps.Length >= 2)
            {
                int si = Mathf.Clamp(c.startIndex, 0, wps.Length - 2);
                dir = (wps[si + 1].position - wps[si].position);
                break;
            }
        }
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        if (dir.sqrMagnitude < 1e-4f)
            dir = Vector3.ProjectOnPlane(laneParent.transform.forward, Vector3.up);
        if (dir.sqrMagnitude < 1e-4f) dir = Vector3.forward;
        dir.Normalize();

        // Sort FRONT -> BACK along that direction (descending dot)
        ordered.Sort((a, b) =>
            Vector3.Dot(b.transform.position, dir).CompareTo(
            Vector3.Dot(a.transform.position, dir)));

        // Link followers to their leader; freeze everyone for now
        for (int i = 0; i < ordered.Count; i++)
        {
            var nav = ordered[i];
            nav.precedingCar = (i > 0) ? ordered[i - 1].transform : null;

            var agent = nav.GetComponent<NavMeshAgent>();
            if (agent)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                agent.ResetPath();
            }
            nav.enabled = false; // visible & stationary
        }

        return ordered;
    }

    private IEnumerator StartLaneStaggered(List<CarNavigator> lane)
    {
        if (lane == null || lane.Count == 0) yield break;

        // Lane list is FRONT -> BACK, so start in that order
        for (int i = 0; i < lane.Count; i++)
        {
            var nav = lane[i];
            if (!nav) continue;

            var agent = nav.GetComponent<NavMeshAgent>();
            if (agent)
            {
                agent.isStopped = false;
                agent.velocity = Vector3.zero;
            }
            if (!nav.enabled) nav.enabled = true;   // OnEnable() sets destination & speed

            if (staggerBetweenCars > 0f)
                yield return new WaitForSeconds(staggerBetweenCars);
        }
    }

    // ---------------------------------------------------------------------
    // Freezing helpers
    // ---------------------------------------------------------------------

    private void FreezeLane(GameObject laneParent)
    {
        if (!laneParent) return;
        foreach (var nav in laneParent.GetComponentsInChildren<CarNavigator>(true))
        {
            var agent = nav.GetComponent<NavMeshAgent>();
            if (agent)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                agent.ResetPath();
            }
            nav.enabled = false;
        }
    }

    private void FreezeList(List<CarNavigator> lane)
    {
        if (lane == null) return;
        foreach (var nav in lane)
        {
            if (!nav) continue;
            var agent = nav.GetComponent<NavMeshAgent>();
            if (agent)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                agent.ResetPath();
            }
            nav.enabled = false;
        }
    }

    private void FreezeAll()
    {
        FreezeList(nR); FreezeList(sR);
        FreezeList(nL); FreezeList(sL);
        FreezeList(e120); FreezeList(w120);
    }
}
