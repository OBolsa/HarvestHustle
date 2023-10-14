using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public int spotNumber;
    public string goToScene;
    public Transform spot;

    private void OnTriggerEnter(Collider other)
    {
        GameplayManager.instance.sceneManager.SceneLoaded += OnSceneLoaded;

        //GameplayManager.instance.sceneManager.ChangeScene(SceneManager.GetActiveScene().name, goToSceneName);
    }

    private void OnSceneLoaded()
    {
        GameplayManager.instance.sceneManager.SceneLoaded -= OnSceneLoaded;

        Transform targetSpot = GameplayManager.instance.sceneManager.GetSpotTransform(spotNumber);
        if (targetSpot != null)
        {
            GameplayManager.instance.player.Controller.enabled = false;
            GameplayManager.instance.player.transform.position = targetSpot.position;
            GameplayManager.instance.player.transform.forward = targetSpot.forward;
            GameplayManager.instance.player.Controller.enabled = true;
        }
    }
}
