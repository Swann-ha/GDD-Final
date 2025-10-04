using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 5, -10);

    void LateUpdate()
    {
        // Calculate the desired position relative to the player's rotation
        Vector3 desiredPosition = player.transform.position + player.transform.TransformDirection(offset);

        // Move the camera to the desired position
        transform.position = desiredPosition;

        // Make the camera look at the player
        transform.LookAt(player.transform);
    }
}
