using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Config", fileName = "MovementConfig_")]
public class MovementConfig : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float gravity;
    public float timeToRotateInSeconds;
}