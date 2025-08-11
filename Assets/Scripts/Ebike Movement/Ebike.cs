using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json; // Requires Newtonsoft JSON (Json.NET for Unity)
using System.Linq;




public class Ebike : MonoBehaviour
{
    private string filePath;
    public Dictionary<string, List<Vector3>> EbikePaths3D;


    //constants
    private const float xConversion = 77.421f/665.88f;
    private const float zConversion = 62.623f / 759.76f;
    private const float xOffset = 390 * xConversion;
    private const float zOffset = 400 * zConversion;


    // Store ebike paths: key = bike name/id, value = list of Vector2 points
    public Dictionary<string, List<Vector2>> ebikePaths;

    private void Awake()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "filtered_trajs.json");

        var data = LoadJson();
        EbikePaths3D = ConvertToVector3(data);
        Debug.Log("Loaded paths: " + EbikePaths3D.Count);
        var ABC = ConvertToVector3(data);
        var firstPair = ABC.First();
        Debug.Log("First key: " + firstPair.Key);
        Debug.Log("First vector: " + firstPair.Value[0]);
    }

    // Helper to get the first path
    public List<Vector3> GetFirstPath()   // <-- ADD THIS
    {
        var enumerator = EbikePaths3D.GetEnumerator();
        Debug.Log($"Bike Paths 3D: {EbikePaths3D}");
        if (enumerator.MoveNext()){
            return enumerator.Current.Value; // return the first path’s vector list
        }
        return null;
    }


    Dictionary<string, List<Vector2>> LoadJson()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            // First, parse into dictionary of lists of lists (raw numbers)
            var rawData = JsonConvert.DeserializeObject<Dictionary<string, List<List<float>>>>(json);

            // Convert into dictionary of Vector2
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

            // Example: log first point
            if (ebikePaths.ContainsKey("186_ebike"))
            {
                Debug.Log("Loaded path for 186_ebike with " + ebikePaths["186_ebike"].Count + " points.");
                Debug.Log("First point: " + ebikePaths["186_ebike"][0]);
            }
            return ebikePaths;
        }
        else
        {
           
            Debug.LogError("JSON file not found at " + filePath);
            return null;
        }
    }

    Dictionary<string, List<Vector3>> ConvertToVector3(Dictionary<string, List<Vector2>> Data)
    {
        var Paths = new Dictionary<string, List<Vector3>>(Data.Count);

        foreach(var obj in Data)
        {
            var key = obj.Key;
            var path = obj.Value;
            var newPath = new List<Vector3>();
           for(var i = 0; i < path.Count; i++)
            {
                var vector = path[i];
                var x = vector.x * xConversion - xOffset;
                var z = vector.y * zConversion - zOffset;
                newPath.Add(new Vector3(x, 1, z));


            }
            Paths.Add(key, newPath);

        }
        return Paths;


    }
}
