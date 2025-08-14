using System.Collections;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    // Vehicle signal heads
    public TrafficLight vehicleLight_EW;
    public TrafficLight vehicleLight_NS;

    // Pedestrian signal heads
    public PedestrianLight pedestrianLight_EW;
    public PedestrianLight pedestrianLight_NS;

    [Header("Phase totals (green + yellow)")]
    public float phaseTotal_NS = 40f;   // NS goes first
    public float phaseTotal_EW = 20f;   // then EW

    [Header("Vehicle yellow within each phase")]
    public float vehicleYellowTime_NS = 3f;
    public float vehicleYellowTime_EW = 3f;

    [Header("All-red buffer between phases")]
    public float allRedBufferTime = 1f;

    [Tooltip("If true, the cycle starts with NS, then EW.")]
    public bool startWithNS = true;

    private void Start()
    {
        StartCoroutine(CycleLights());
    }

    private IEnumerator CycleLights()
    {
        while (true)
        {
            if (startWithNS)
            {
                yield return StartCoroutine(Phase_NS());
                yield return StartCoroutine(Phase_EW());
            }
            else
            {
                yield return StartCoroutine(Phase_EW());
                yield return StartCoroutine(Phase_NS());
            }
        }
    }

    // ---------------- PHASES ----------------

    private IEnumerator Phase_NS()
    {
        // NS vehicles run for phaseTotal_NS including yellow
        float greenNS = Mathf.Max(0f, phaseTotal_NS - vehicleYellowTime_NS);

        // Vehicles: NS go, EW stop
        vehicleLight_NS.SetGreen();
        vehicleLight_EW.SetRed();

        // Pedestrians per spec:
        // EW ped RED while NS vehicles are running
        pedestrianLight_EW.SetRed();
        // NS ped GREEN while NS vehicles are running
        pedestrianLight_NS.SetGreen();

        // NS green interval
        yield return new WaitForSeconds(greenNS);

        // NS yellow (keep ped states as above during yellow)
        vehicleLight_NS.SetYellow();
        yield return new WaitForSeconds(vehicleYellowTime_NS);

        // All red for changeover; peds stop during interlock
        vehicleLight_NS.SetRed();
        vehicleLight_EW.SetRed();
        pedestrianLight_NS.SetRed();
        pedestrianLight_EW.SetRed();

        yield return new WaitForSeconds(allRedBufferTime);
    }

    private IEnumerator Phase_EW()
    {
        // EW vehicles run for phaseTotal_EW including yellow
        float greenEW = Mathf.Max(0f, phaseTotal_EW - vehicleYellowTime_EW);

        // Vehicles: EW go, NS stop
        vehicleLight_EW.SetGreen();
        vehicleLight_NS.SetRed();

        // Pedestrians per spec:
        // EW ped GREEN while EW vehicles are running
        pedestrianLight_EW.SetGreen();
        // NS ped RED while EW vehicles are running
        pedestrianLight_NS.SetRed();

        // EW green interval
        yield return new WaitForSeconds(greenEW);

        // EW yellow (keep ped states as above during yellow)
        vehicleLight_EW.SetYellow();
        yield return new WaitForSeconds(vehicleYellowTime_EW);

        // All red for changeover; peds stop during interlock
        vehicleLight_EW.SetRed();
        vehicleLight_NS.SetRed();
        pedestrianLight_NS.SetRed();
        pedestrianLight_EW.SetRed();

        yield return new WaitForSeconds(allRedBufferTime);
    }
}
