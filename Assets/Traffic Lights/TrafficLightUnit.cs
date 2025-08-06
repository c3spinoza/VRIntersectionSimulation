using UnityEngine;

public class TrafficLightUnit : MonoBehaviour
{
    public Light redLight;
    public Light yellowLight;
    public Light greenLight;

    public void SetState(TrafficLightState state)
    {
        redLight.enabled = (state == TrafficLightState.Red);
        yellowLight.enabled = (state == TrafficLightState.Yellow);
        greenLight.enabled = (state == TrafficLightState.Green);
    }
}
