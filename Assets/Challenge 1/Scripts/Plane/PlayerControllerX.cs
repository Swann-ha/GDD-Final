using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public float speed = 15f;
    public float rotationSpeed = 50f;
    public float verticalInput;

    [Header("Pitch Limits (Degrees)")]
    public bool clampPitch = true;
    public float minPitch = -30f; // Nose down limit
    public float maxPitch = 45f;  // Nose up limit

    void FixedUpdate()
    {
        verticalInput = Input.GetAxis("Vertical");

        // Move forward relative to the plane's own facing
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (Mathf.Abs(verticalInput) > 0.0001f)
        {
            // Invert so pressing Up (positive) pitches the nose upward visually
            transform.Rotate(Vector3.right * -rotationSpeed * verticalInput * Time.deltaTime, Space.Self);
        }

        if (clampPitch)
        {
            Vector3 angles = transform.localEulerAngles;
            // Unity wraps angles 0..360; convert X to signed -180..180 for clamping
            float pitch = angles.x;
            if (pitch > 180f) pitch -= 360f;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            angles.x = pitch;
            transform.localEulerAngles = angles;
        }
    }
}
