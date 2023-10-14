using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMov : MonoBehaviour
{
    public float amplitude = 1;
    public float period = 1;
    public float verticalShift;
    private float time;
    Vector3 newPos;

    void Update()
    {
        newPos = transform.position;
        time += Time.deltaTime;

        newPos.y = amplitude * Mathf.Sin(period * time) + verticalShift;

        transform.position = newPos;
    }
}