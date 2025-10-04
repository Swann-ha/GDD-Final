using UnityEngine;

// Clean, simple car controller for testing
public class TestCarController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float rotateSpeed = 100f;

    void Update()
    {
        // Simple forward/backward movement
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * vertical * moveSpeed * Time.deltaTime);

        // Simple left/right rotation
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontal * rotateSpeed * Time.deltaTime);
    }
}