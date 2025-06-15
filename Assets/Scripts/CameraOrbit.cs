using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;       // Le joueur
    public float distance = 7f;
    public float zoomSpeed = 2f;
    public float rotationSpeed = 3f;
    public Vector2 pitchLimits = new Vector2(20f, 60f);

    private float yaw = 0f;
    private float pitch = 20f;

    void LateUpdate()
    {
        // Rotation avec la souris
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        // Zoom avec la molette
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, 7f, 15f);

        // Positionnement autour du joueur
        Vector3 offset = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }
}
