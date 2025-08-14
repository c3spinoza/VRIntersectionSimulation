using UnityEngine;
using UnityEngine.UI;

public class VehicleProximitySimple : MonoBehaviour
{
    public float baseDangerDistance = 6f;
    public float reactionMultiplier = 0.5f;
    public LayerMask vehicleLayer;

    public Image leftSidebar;
    public Image rightSidebar;
    public Color safeColor = Color.green;
    public Color dangerColor = Color.red;

    public Rigidbody playerRb;

    // NEW: use this for orientation/position instead of this.transform
    public Transform referenceTransform;   // assign player (preferred) or camera
    public bool invertSides = false;       // if your UI is mirrored, tick this

    void Awake()
    {
        // Default to the player's transform if not set
        if (referenceTransform == null && playerRb != null)
            referenceTransform = playerRb.transform;
    }

    void Update()
    {
        if (leftSidebar == null || rightSidebar == null)
        {
            Debug.LogWarning("Assign both sidebar Images in the inspector.");
            return;
        }
        if (referenceTransform == null)
        {
            Debug.LogWarning("Assign referenceTransform (player/camera).");
            return;
        }

        // Speed-based danger radius
        float playerSpeed = (playerRb != null) ? playerRb.velocity.magnitude : 0f;
        float currentDangerDistance = baseDangerDistance + (playerSpeed * reactionMultiplier);

        // Use player/camera position as origin; prefer center of mass if we have a Rigidbody
        Vector3 origin = (playerRb != null) ? playerRb.worldCenterOfMass : referenceTransform.position;

        Collider[] hits = Physics.OverlapSphere(origin, currentDangerDistance, vehicleLayer);

        bool dangerLeft = false;
        bool dangerRight = false;

        foreach (Collider hit in hits)
        {
            if (hit == null) continue;
            // Prevent self-detection if layers overlap
            if (playerRb != null && hit.attachedRigidbody == playerRb) continue;

            // Compute position in reference's local space; x>0 is right, x<0 is left
            Vector3 localPos = referenceTransform.InverseTransformPoint(hit.transform.position);

            if (localPos.x > 0f) dangerRight = true;
            else dangerLeft = true;
        }

        // Optional flip if UI is mirrored
        if (invertSides)
        {
            bool tmp = dangerLeft;
            dangerLeft = dangerRight;
            dangerRight = tmp;
        }

        leftSidebar.color = dangerLeft ? dangerColor : safeColor;
        rightSidebar.color = dangerRight ? dangerColor : safeColor;
    }
}
