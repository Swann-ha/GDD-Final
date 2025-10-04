using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    public GameObject plane;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(30f, 0f, 10f);
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    void LateUpdate()
    {
        if (plane != null)
        {
            transform.position = plane.transform.position + offset;
        }
    }
}
