using System.Collections;
using UnityEngine;

public class MasterTrafficController : MonoBehaviour
{
    [Header("Lane Parents (assign in Inspector)")]
    public GameObject northRightLane;    // “Heading North Right Lane”
    public GameObject southRightLane;    // “Heading South Right Lane”
    public GameObject northLeftLane;     // “Heading North Left Lane”
    public GameObject southLeftLane;     // “Heading South Left Lane”
    public GameObject east120Lane;       // “Heading East 120 Lane”
    public GameObject west120Lane;       // “Heading West 120 Lane”

    [Header("Phase Durations (seconds)")]
    public float phase1Duration = 15f;   // N–R & S–R
    public float phase2Duration = 10f;   // N–L & S–L
    public float phase3Duration = 15f;   // E–120 & W–120

    void Start()
    {
        // Disable all lanes, then begin the phase sequence
        SetAllLanesActive(false);
        StartCoroutine(RunTrafficPhases());
    }

    private IEnumerator RunTrafficPhases()
    {
        // Phase 1: North & South Right lanes
        SetAllLanesActive(false);
        northRightLane.SetActive(true);
        southRightLane.SetActive(true);
        yield return new WaitForSeconds(phase1Duration);

        // Phase 2: North & South Left lanes
        SetAllLanesActive(false);
        northLeftLane.SetActive(true);
        southLeftLane.SetActive(true);
        yield return new WaitForSeconds(phase2Duration);

        // Phase 3: East & West 120 lanes
        SetAllLanesActive(false);
        east120Lane.SetActive(true);
        west120Lane.SetActive(true);
        yield return new WaitForSeconds(phase3Duration);

        // If you want to loop the sequence, uncomment the following lines:
        // yield return new WaitForSeconds(0.5f);
        // StartCoroutine(RunTrafficPhases());
    }

    private void SetAllLanesActive(bool isActive)
    {
        northRightLane.SetActive(isActive);
        southRightLane.SetActive(isActive);
        northLeftLane.SetActive(isActive);
        southLeftLane.SetActive(isActive);
        east120Lane.SetActive(isActive);
        west120Lane.SetActive(isActive);
    }
}
