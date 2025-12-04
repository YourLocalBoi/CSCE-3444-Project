using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Target to follow")]
    public Transform target;

    [Header("Camera movement")]
    public float smoothSpeed = 5f;

    [Header("Offset from the player")]
    public Vector3 offset = new Vector3(0, 0, -10);

    public bool canFollow = true; // Cutscene will toggle this
    void LateUpdate()
    {
        if (!canFollow || target == null)
            return;
        // Desired camera position
        Vector3 desiredPosition = target.position + offset;

        // Smooth follow the target
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}
