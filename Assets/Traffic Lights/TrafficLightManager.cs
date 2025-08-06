using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TrafficLightManager : MonoBehaviour
{
    public float redDuration = 5f;
    public float greenDuration = 5f;
    public float yellowDuration = 2f;

    public List<TrafficLightUnit> vehicleLights;
    public List<PedestrianLightUnit> pedestrianLights;

    private void Start()
    {
        StartCoroutine(ControlTraffic());
    }

    IEnumerator ControlTraffic()
    {
        while (true)
        {
            // 🚗 Vehicle RED, 🚶 Pedestrian GREEN
            SetAllVehicleLights(TrafficLightState.Red);
            SetAllPedestrianLights(TrafficLightState.Red);
            yield return new WaitForSeconds(redDuration);

            // 🚗 Vehicle GREEN, 🚶 Pedestrian RED
            SetAllVehicleLights(TrafficLightState.Green);
            SetAllPedestrianLights(TrafficLightState.Green);
            yield return new WaitForSeconds(greenDuration);

            // 🚗 Vehicle YELLOW, 🚶 Still RED
            SetAllVehicleLights(TrafficLightState.Yellow);
            SetAllPedestrianLights(TrafficLightState.Yellow);
            yield return new WaitForSeconds(yellowDuration);
        }
    }

    void SetAllVehicleLights(TrafficLightState state)
    {
        foreach (var light in vehicleLights)
            light.SetState(state);
    }

    void SetAllPedestrianLights(TrafficLightState vehicleState)
    {
        foreach (var light in pedestrianLights)
            light.SetState(vehicleState);  // Interpreted as inverted inside PedestrianLightUnit
    }
}
