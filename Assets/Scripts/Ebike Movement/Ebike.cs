using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class Ebike : MonoBehaviour
{
    private string filePath;
    public Dictionary<string, List<Vector3>> EbikePaths3D;

    // Conversion constants
    private const float xConversion = 77.421f / 665.88f;
    private const float zConversion = 62.623f / 759.76f;
    private const float xOffset = 390 * xConversion;
    private const float zOffset = 400 * zConversion;

    // Store original 2D paths
    public Dictionary<string, List<Vector2>> ebikePaths;

    // Keep paths in a list for easy ordered access
    private List<List<Vector3>> allPathsList;
    private int currentPathIndex = -1;

    private void Awake()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "filtered_trajs.json");

        var data = LoadJson();
        EbikePaths3D = ConvertToVector3(data);
        Debug.Log("Loaded paths: " + EbikePaths3D.Count);

        // Convert dictionary values to list for sequential access
        allPathsList = EbikePaths3D.Values.ToList();
    }

    // Get the first path in the list
    public List<Vector3> GetFirstPath()
    {
        currentPathIndex = 0;
        if (allPathsList.Count > 0)
        {
            return allPathsList[0];
        }
        return null;
    }

    // Get the next path in the list
    public List<Vector3> GetNextPath()
    {
        currentPathIndex++;
        if (currentPathIndex >= 0 && currentPathIndex < allPathsList.Count)
        {
            return allPathsList[currentPathIndex];
        }

        // No more paths left
        return null;
    }

    // Load JSON and convert to Vector2 paths
    Dictionary<string, List<Vector2>> LoadJson()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var rawData = JsonConvert.DeserializeObject<Dictionary<string, List<List<float>>>>(json);

            ebikePaths = new Dictionary<string, List<Vector2>>();
            foreach (var entry in rawData)
            {
                List<Vector2> path = new List<Vector2>();
                foreach (var coords in entry.Value)
                {
                    if (coords.Count >= 2)
                        path.Add(new Vector2(coords[0], coords[1]));
                }
                ebikePaths[entry.Key] = path;
            }

            if (ebikePaths.ContainsKey("186_ebike"))
            {
                Debug.Log("Loaded path for 186_ebike with " + ebikePaths["186_ebike"].Count + " points.");
            }

            return ebikePaths;
        }
        else
        {
            Debug.LogError("JSON file not found at " + filePath);
            return null;
        }
    }

    // Convert Vector2 paths into Vector3 for Unity's NavMesh
    Dictionary<string, List<Vector3>> ConvertToVector3(Dictionary<string, List<Vector2>> data)
    {
        var paths = new Dictionary<string, List<Vector3>>(data.Count);

        foreach (var obj in data)
        {
            var key = obj.Key;
            var path = obj.Value;
            var newPath = new List<Vector3>();

            foreach (var vector in path)
            {
                var x = vector.x * xConversion - xOffset;
                var z = vector.y * zConversion - zOffset;
                newPath.Add(new Vector3(x, 1, z));
            }

            paths.Add(key, newPath);
        }

        return paths;
    }
}
