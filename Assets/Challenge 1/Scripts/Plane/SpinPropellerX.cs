using UnityEngine;

public class SpinPropellerX : MonoBehaviour
{
    [Tooltip("Revolutions per minute (RPM) for the propeller.")]
    public float rpm = 1200f;

    void Update()
    {
        // Convert RPM to degrees per second: RPM * 360 / 60 = RPM * 6
        float degreesPerSecond = rpm * 6f;
        transform.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}
