using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Version 2: Improved order starter. Attach to the parent (e.g., "order").
// Orders cars spatially along the path, assigns precedingCar & startIndex, and staggers activation to preserve spacing.
public class CarOrderStarter : MonoBehaviour
{
    [Tooltip("Parent containing car GameObjects with CarNavigator attached.")]
    public string carsParentName = "order";
    [Tooltip("Waypoints defining the path direction (drag & drop, at least 2 elements)")]
    public Transform[] waypoints;
    [Tooltip("Delay in seconds between starting each car's navigation.")]
    public float delayBetweenCars = 0.5f;

    void Start()
    {
        GameObject parent = GameObject.Find(carsParentName);
        if (parent == null)
        {
           // Debug.LogError($"[CarOrderStarter v2] Parent '{carsParentName}' not found.");
            return;
        }

        var navs = parent.GetComponentsInChildren<CarNavigator>(includeInactive: true);
        if (navs.Length == 0)
        {
            Debug.LogWarning($"[CarOrderStarter v2] No CarNavigator components found under '{carsParentName}'.");
            return;
        }

        // Compute path direction from first two waypoints (fallback to forward)
        Vector3 pathDir = Vector3.forward;
        if (waypoints != null && waypoints.Length >= 2)
            pathDir = (waypoints[1].position - waypoints[0].position).normalized;

        // Sort navigators by their projection along pathDir
        List<CarNavigator> sorted = new List<CarNavigator>(navs);
        sorted.Sort((a, b) =>
        {
            float da = Vector3.Dot(a.transform.position, pathDir);
            float db = Vector3.Dot(b.transform.position, pathDir);
            return da.CompareTo(db);
        });

        // Assign precedingCar and startIndex before enabling
        for (int i = 0; i < sorted.Count; i++)
        {
            var nav = sorted[i];
            nav.enabled = false; // ensure disabled initially
            if (i > 0)
                nav.precedingCar = sorted[i - 1].transform;
            nav.startIndex = Mathf.Clamp(i, 0, (waypoints != null ? waypoints.Length - 1 : 0));
        }

        // Begin sequential enabling preserving order
        StartCoroutine(StartNavigatorsSequentially(sorted));
    }

    private IEnumerator StartNavigatorsSequentially(List<CarNavigator> sorted)
    {
        for (int i = 0; i < sorted.Count; i++)
        {
            yield return new WaitForSeconds(delayBetweenCars);
            var nav = sorted[i];
            nav.enabled = true;
            Debug.Log($"[CarOrderStarter v2] Navigation started for '{nav.gameObject.name}' at order {i}");
        }
    }
}
