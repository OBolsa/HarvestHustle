using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Follow:")]
    public Transform target;
    private Vector3 newPos;

    [Header("Follow in:")]
    public bool x = true;
    public bool y = false;
    public bool z = true;

    private void Update()
    {
        newPos.x = x ? target.position.x : transform.position.x;
        newPos.y = y ? target.position.y : transform.position.y;
        newPos.z = z ? target.position.z : transform.position.z;

        transform.position = newPos;
    }
}