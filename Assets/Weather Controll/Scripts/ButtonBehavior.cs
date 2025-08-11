using UnityEngine;

public class UIButtonHandler : MonoBehaviour
{
    public WeatherResetController weatherResetController; // Button1
    public NightTimeController nightTimeController;        // Button2
    public RainFog rainWeatherController;    // Button3
    public MenuToggle menuToggle;                          // Minimize button

    // Called by Button1
    public void OnButton1Click()
    {
        if (weatherResetController != null)
            weatherResetController.ResetToDayTime();
    }

    // Called by Button2
    public void OnButton2Click()
    {
        if (nightTimeController != null)
        {
            // Re-enable night logic since original SetNightTime() is private and only runs at Start
            nightTimeController.Invoke("SetNightTime", 0f);
        }
    }

    // Called by Button3
    public void OnButton3Click()
    {
        if (rainWeatherController != null)
            rainWeatherController.ToggleRain();
    }

    // Called by Minimize Button
    public void OnMinimizeClick()
    {
        if (menuToggle != null)
            menuToggle.MinimizeMenu();
    }
    public void OnMaximizeClick()
    {
        if (menuToggle != null)
            menuToggle.MaximizeMenu();
    }
}

