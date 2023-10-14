using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform target;
    private Vector3 newPos;

    private void Start()
    {
        newPos = new Vector3(target.position.x, transform.position.y, target.position.z);
    }

    private void Update()
    {
        newPos.x = target.position.x;
        newPos.z = target.position.z;
        transform.position = newPos;
    }
}