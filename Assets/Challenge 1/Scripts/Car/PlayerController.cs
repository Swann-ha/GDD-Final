using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Vehicle movement settings (adjustable in inspector) - INCREASED FOR TESTING
    public float speed = 100f;  // Much higher for obvious movement
    public float turnSpeed = 180f;  // Much higher for obvious rotation

    // Input variables (private - only used internally)
    private float horizontalInput;
    private float forwardInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("PlayerController Start() called - Script is attached and running!");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug: Check if Update is running every 60 frames (about once per second)
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log("PlayerController Update() running - Frame: " + Time.frameCount);
        }

        // Try direct key detection first
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Forward key detected!");
            forwardInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Backward key detected!");
            forwardInput = -1f;
        }
        else
        {
            forwardInput = 0f;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Left key detected!");
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Right key detected!");
            horizontalInput = 1f;
        }
        else
        {
            horizontalInput = 0f;
        }

        // Get player input from arrow keys or WASD (backup method)
        // horizontalInput = Input.GetAxis("Horizontal");
        // forwardInput = Input.GetAxis("Vertical");

        // Debug movement values
        if (forwardInput != 0)
        {
            Debug.Log("Moving with forwardInput: " + forwardInput + ", speed: " + speed + ", deltaTime: " + Time.deltaTime);
            Debug.Log("Current position: " + transform.position);
        }
        if (horizontalInput != 0)
        {
            Debug.Log("Rotating with horizontalInput: " + horizontalInput + ", turnSpeed: " + turnSpeed);
            Debug.Log("Current rotation: " + transform.rotation.eulerAngles);
        }

        // Move the vehicle forward and backward
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);

        // Rotate the vehicle left and right like a real car
        transform.Rotate(Vector3.up * horizontalInput * turnSpeed * Time.deltaTime);
    }
}
