using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    private GameObject[] greenLights;
    private GameObject[] yellowLights;
    private GameObject[] redLights;

    void Awake()
    {
        greenLights = GetChildLightsContaining("green");
        yellowLights = GetChildLightsContaining("yellow");
        redLights = GetChildLightsContaining("red");
    }

    private GameObject[] GetChildLightsContaining(string keyword)
    {
        keyword = keyword.ToLower();
        Transform[] children = GetComponentsInChildren<Transform>(true);
        var result = new System.Collections.Generic.List<GameObject>();

        foreach (Transform child in children)
        {
            if (child.name.ToLower().Contains(keyword))
                result.Add(child.gameObject);
        }

        return result.ToArray();
    }

    public void SetGreen()
    {
        SetLights(greenLights, true);
        SetLights(yellowLights, false);
        SetLights(redLights, false);
    }

    public void SetYellow()
    {
        SetLights(greenLights, false);
        SetLights(yellowLights, true);
        SetLights(redLights, false);
    }

    public void SetRed()
    {
        SetLights(greenLights, false);
        SetLights(yellowLights, false);
        SetLights(redLights, true);
    }

    private void SetLights(GameObject[] lights, bool active)
    {
        foreach (var light in lights)
            light.SetActive(active);
    }
}
