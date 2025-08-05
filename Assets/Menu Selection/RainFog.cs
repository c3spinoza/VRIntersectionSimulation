using UnityEngine;

public class RainFog : MonoBehaviour
{
    [Header("Rain Settings")]
    public GameObject rainPrefab;       // Your rain particle prefab
    public AudioClip rainSound;         // Rain audio clip

    private GameObject rainInstance;    // Instantiated rain clone
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;       // Loop the rain sound
        audioSource.playOnAwake = false;
    }

    public void ToggleRain()
    {
        // Instantiate rain clone if none exists
        if (rainInstance == null && rainPrefab != null)
        {
            rainInstance = Instantiate(rainPrefab, transform);
            rainInstance.SetActive(true);
        }

        // Play rain audio if not already playing
        if (audioSource != null && rainSound != null && !audioSource.isPlaying)
        {
            audioSource.clip = rainSound;
            audioSource.Play();
        }

        // Enable fog
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.4f, 0.4f, 0.4f);
        RenderSettings.fogDensity = 0.008f;
    }

    public void DestroyRain()
    {
        // Stop rain audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Destroy rain clone if it exists
        if (rainInstance != null)
        {
            Destroy(rainInstance);
            rainInstance = null;
        }

        // Disable fog
        RenderSettings.fog = false;
    }
}

    // Optional: toggle rain on/off
