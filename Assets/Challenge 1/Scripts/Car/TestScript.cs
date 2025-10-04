using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        Debug.Log("TEST SCRIPT IS WORKING!");
        Debug.LogError("RED ERROR MESSAGE - SCRIPT IS ATTACHED!");
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("ANY KEY WAS PRESSED!");
        }
    }
}