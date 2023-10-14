using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDataRStateMachine : ScriptableObject
{
    [Header("States")]
    public List<PlantStateDataR> States = new List<PlantStateDataR>();

    [Header("Current State")]
    [SerializeField]
    private int currentStateIndex = 0;

    public PlantStateDataR CurrentState
    {
        get
        {
            if (currentStateIndex >= 0 && currentStateIndex < States.Count)
            {
                return States[currentStateIndex];
            }
            return null;
        }
    }

    public int CurrentStateIndex
    {
        get { return currentStateIndex; }
    }

    public void SetCurrentState(int newStateIndex)
    {
        if (newStateIndex >= 0 && newStateIndex < States.Count)
        {
            currentStateIndex = newStateIndex;
        }
    }

    // Add a method to transition to a state based on GrowthState
    public void TransitionToState(GrowthState growthState)
    {
        int newStateIndex = (int)growthState;
        SetCurrentState(newStateIndex);
    }
}