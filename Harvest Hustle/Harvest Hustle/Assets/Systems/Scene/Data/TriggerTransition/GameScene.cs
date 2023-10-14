using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public string SceneName;
    public GameScenesEnum SceneType;
    public List<InteractableTransition> Transitions { get; private set; }

    private void Awake()
    {
        Transitions = GetComponentsInChildren<InteractableTransition>(true).ToList();
    }
}