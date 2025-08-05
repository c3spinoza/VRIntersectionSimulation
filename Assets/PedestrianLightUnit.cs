using UnityEngine;

public class PedestrianLightUnit : MonoBehaviour
{
    public Light redLight;
    public Light greenLight;

    public void SetState(TrafficLightState state)
    {
        redLight.enabled = (state != TrafficLightState.Red); // Show red for all non-Red states
        greenLight.enabled = (state == TrafficLightState.Red); // Green only during vehicle red
    }
}
