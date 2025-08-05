using UnityEngine;

public class WeatherResetController : MonoBehaviour
{
    public Light directionalLight;             // Your sun light
    public Material daySkybox;                 // Original daytime skybox material
    public Color dayAmbientLight = Color.white;
    public Color dayFogColor = new Color(0.75f, 0.75f, 0.75f);
    public float dayFogDensity = 0f;

    public RainFog rainController;             // Reference to your RainFog script

    void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = RenderSettings.sun;
        }
        if (daySkybox == null)
        {
            daySkybox = RenderSettings.skybox; // Capture the current skybox as default day skybox
        }
    }

    public void ResetToDayTime()
    {
        // Reset skybox to day
        if (daySkybox != null)
        {
            RenderSettings.skybox = daySkybox;
        }

        // Disable fog
        RenderSettings.fog = false;

        // Reset ambient lighting and fog settings
        RenderSettings.ambientLight = dayAmbientLight;
        RenderSettings.fogColor = dayFogColor;
        RenderSettings.fogDensity = dayFogDensity;

        // Enable sun light and adjust intensity/color
        if (directionalLight != null)
        {
            directionalLight.enabled = true;
            directionalLight.intensity = 1.2f;
            directionalLight.color = Color.white;
        }

        // Disable rain (destroy rain clone and stop audio)
        if (rainController != null)
        {
            rainController.DestroyRain();
        }
    }
}
