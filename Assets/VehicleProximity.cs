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

    void Update()
    {
        if (playerRb == null || leftSidebar == null || rightSidebar == null)
        {
            Debug.LogWarning("Assign all references in inspector!");
            return;
        }

        float playerSpeed = playerRb.velocity.magnitude;
        float currentDangerDistance = baseDangerDistance + (playerSpeed * reactionMultiplier);

        Collider[] hits = Physics.OverlapSphere(transform.position, currentDangerDistance, vehicleLayer);

        bool dangerLeft = false;
        bool dangerRight = false;

        foreach (Collider hit in hits)
        {
            Vector3 dirToVehicle = (hit.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.right, dirToVehicle);

            if (dot > 0) dangerRight = true;
            else dangerLeft = true;
        }

        leftSidebar.color = dangerLeft ? dangerColor : safeColor;
        rightSidebar.color = dangerRight ? dangerColor : safeColor;
    }
}
