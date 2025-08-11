using UnityEngine;
using UnityEngine.UI;

public class VehicleProximity : MonoBehaviour
{
    [Header("Detection Settings")]
    public float baseDangerDistance = 6f;
    public float reactionMultiplier = 0.5f;
    public LayerMask vehicleLayer;

    [Header("UI")]
    public Image leftSidebar;
    public Image rightSidebar;
    public Color safeColor = new Color(0, 1, 0, 0.5f); // semi-transparent green
    public Color dangerColor = new Color(1, 0, 0, 0.5f); // semi-transparent red
    public float fadeSpeed = 5f; // how fast colors fade

    [Header("Player Movement")]
    public Rigidbody playerRb; // assign the player's rigidbody

    private float currentDangerDistance;

    void Update()
    {
        // Calculate dynamic danger distance based on speed
        float playerSpeed = playerRb.velocity.magnitude;
        currentDangerDistance = baseDangerDistance + (playerSpeed * reactionMultiplier);

        // Find vehicles in range
        Collider[] hits = Physics.OverlapSphere(transform.position, currentDangerDistance, vehicleLayer);

        bool dangerLeft = false;
        bool dangerRight = false;

        foreach (Collider hit in hits)
        {
            Vector3 dirToVehicle = (hit.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.right, dirToVehicle);

            if (dot > 0) dangerRight = true; // right side
            else dangerLeft = true; // left side
        }

        // Fade colors for smooth transition
        Color targetLeft = dangerLeft ? dangerColor : safeColor;
        Color targetRight = dangerRight ? dangerColor : safeColor;

        leftSidebar.color = Color.Lerp(leftSidebar.color, targetLeft, Time.deltaTime * fadeSpeed);
        rightSidebar.color = Color.Lerp(rightSidebar.color, targetRight, Time.deltaTime * fadeSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentDangerDistance);
    }
}
