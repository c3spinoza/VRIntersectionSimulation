// NightTimeController (Night Time)
using UnityEngine;

public class NightTimeController : MonoBehaviour
{
    public Material nightSkybox;
    public Light directionalLight;
    public Color nightAmbientLight = Color.black;
    public float nightDirectionalLightIntensity = 0.1f;

    public RainFog rainController;  // Reference to your RainFog script

    void Start()
    {
        SetNightTime();
    }

    void SetNightTime()
    {
        RenderSettings.skybox = nightSkybox;
        RenderSettings.ambientLight = nightAmbientLight;

        if (directionalLight != null)
        {
            directionalLight.intensity = nightDirectionalLightIntensity;
            directionalLight.color = new Color(0.1f, 0.1f, 0.3f);
        }

        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.1f);
        RenderSettings.fogDensity = 0.02f;

        if (rainController != null)
        {
            rainController.DestroyRain();
        }
    }
}
