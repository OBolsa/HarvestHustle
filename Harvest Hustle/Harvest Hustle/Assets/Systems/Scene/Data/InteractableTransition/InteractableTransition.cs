using UnityEngine;

[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(Rigidbody))]
public class InteractableTransition : Interactable
{
    public GameScene Scene;
    public SceneTransitionSettings Info;

    public override void DoInteraction()
    {
        base.DoInteraction();

        GameplayManager.instance.sceneManager.ChangeScene(this);
    }
}

[System.Serializable]
public struct SceneTransitionSettings
{
    public int spotNumber;
    public string sceneToGo;
    public GameScenesEnum gameSceneToGo;
    public Transform spotTransform;
    public StrikeType strikeType;
}