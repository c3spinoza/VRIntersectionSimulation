using UnityEngine;

public class PedestrianLight : MonoBehaviour
{
    public Light[] redLights;
    public Light[] yellowLights;
    public Light[] greenLights;

    private enum LightState { Red, Green, Yellow }
    private LightState currentState;

    public void SetRed()
    {
        SetLightState(LightState.Red);
    }

    public void SetYellow()
    {
        SetLightState(LightState.Yellow);
    }

    public void SetGreen()
    {
        SetLightState(LightState.Green);
    }

    private void SetLightState(LightState state)
    {
        currentState = state;

        SetLights(redLights, state == LightState.Red);
        SetLights(yellowLights, state == LightState.Yellow);
        SetLights(greenLights, state == LightState.Green);
    }

    private void SetLights(Light[] lights, bool active)
    {
        if (lights == null) return;

        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = active;
        }
    }
}
