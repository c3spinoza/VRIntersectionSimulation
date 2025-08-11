using System.Collections;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public TrafficLight vehicleLight_EW;
    public TrafficLight vehicleLight_NS;
    public PedestrianLight pedestrianLight_EW;
    public PedestrianLight pedestrianLight_NS;

    public float vehicleGreenTime_EW = 10f;
    public float vehicleYellowTime_EW = 3f;
    public float vehicleGreenTime_NS = 10f;
    public float vehicleYellowTime_NS = 3f;

    public float pedestrianGreenTime = 7f;
    public float pedestrianYellowTime = 3f;
    public float allRedBufferTime = 1f;

    private void Start()
    {
        StartCoroutine(CycleLights());
    }

    private IEnumerator CycleLights()
    {
        while (true)
        {
            // EAST-WEST VEHICLES GREEN, NS RED
            vehicleLight_EW.SetGreen();
            vehicleLight_NS.SetRed();

            // Pedestrians EW RED (don't cross)
            pedestrianLight_EW.SetRed();

            // Pedestrians NS GREEN (safe to cross)
            pedestrianLight_NS.SetGreen();

            yield return new WaitForSeconds(vehicleGreenTime_EW);

            // EAST-WEST VEHICLES YELLOW
            vehicleLight_EW.SetYellow();

            // Pedestrians NS YELLOW (warning to finish crossing)
            pedestrianLight_NS.SetYellow();

            yield return new WaitForSeconds(vehicleYellowTime_EW);

            // ALL VEHICLES RED - Buffer
            vehicleLight_EW.SetRed();
            vehicleLight_NS.SetRed();

            // Pedestrians NS RED (stop crossing)
            pedestrianLight_NS.SetRed();

            yield return new WaitForSeconds(allRedBufferTime);

            // NORTH-SOUTH VEHICLES GREEN, EW RED
            vehicleLight_NS.SetGreen();
            vehicleLight_EW.SetRed();

            // Pedestrians NS RED (don't cross)
            pedestrianLight_NS.SetRed();

            // Pedestrians EW GREEN (safe to cross)
            pedestrianLight_EW.SetGreen();

            yield return new WaitForSeconds(vehicleGreenTime_NS);

            // NORTH-SOUTH VEHICLES YELLOW
            vehicleLight_NS.SetYellow();

            // Pedestrians EW YELLOW (warning to finish crossing)
            pedestrianLight_EW.SetYellow();

            yield return new WaitForSeconds(vehicleYellowTime_NS);

            // ALL VEHICLES RED - Buffer
            vehicleLight_NS.SetRed();
            vehicleLight_EW.SetRed();

            // Pedestrians EW RED (stop crossing)
            pedestrianLight_EW.SetRed();

            yield return new WaitForSeconds(allRedBufferTime);
        }
    }
}
