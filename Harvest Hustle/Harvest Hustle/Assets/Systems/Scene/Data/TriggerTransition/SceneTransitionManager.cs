using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public CanvasGroup sceneTransitionModal;
    public List<InteractableTransition> interactablesTriggers = new List<InteractableTransition>();
    private InteractableTransition currentTransitionSettings;
    public List<GameScene> scenes = new List<GameScene>();

    public event Action SceneLoaded;

    private void Awake()
    {
        InteractableTransition[] allSceneTransitions = FindObjectsOfType<InteractableTransition>(true);

        foreach (InteractableTransition sceneTransitions in allSceneTransitions)
        {
            interactablesTriggers.Add(sceneTransitions);
        }
    }

    public void ChangeScene(InteractableTransition infos)
    {
        DOTween.To(() => sceneTransitionModal.alpha, x => sceneTransitionModal.alpha = x, 1f, 0.15f).OnComplete(() =>
        {
            GameplayManager.instance.interactableInstigator.ClearInteractables();
            currentTransitionSettings = infos;
            SceneLoaded += OnSceneLoaded;
            LoadGameSceneWithTransition();
        });
    }

    private void LoadGameSceneWithTransition()
    {
        scenes.ForEach(s => s.gameObject.SetActive(false));
        scenes.Find(s => s.SceneType == currentTransitionSettings.Info.gameSceneToGo).gameObject.SetActive(true);

        // Notify listeners that the sceneTransitions\ has loaded
        SceneLoaded?.Invoke();
        TimeManager.Instance.DoStrike(currentTransitionSettings.Info.strikeType);

        // Scene fully loaded, fade out transition modal
        DOTween.To(() => sceneTransitionModal.alpha, x => sceneTransitionModal.alpha = x, 0f, 0.15f);
    }

    private void OnSceneLoaded()
    {
        SceneLoaded -= OnSceneLoaded;

        GameScene scene = scenes.Find(s => s.SceneType == currentTransitionSettings.Info.gameSceneToGo);
        Transform targetSpot = scene.Transitions.Find(t => t.Info.spotNumber == currentTransitionSettings.Info.spotNumber).Info.spotTransform;

        //Teletransport the Player
        if (targetSpot != null)
        {
            GameplayManager.instance.player.Controller.enabled = false;
            GameplayManager.instance.player.transform.position = targetSpot.position;
            GameplayManager.instance.player.transform.forward = targetSpot.forward;
            GameplayManager.instance.player.Controller.enabled = true;
        }
        else
        {
            Debug.Log("Eita bixo");
        }
    }

    public Transform GetSpotTransform(int spot)
    {
        InteractableTransition interactable = interactablesTriggers.Find(s => s.Info.spotNumber == spot);

        if(interactable != null )
        {
            Debug.Log("GetSpotTransform: Found trigger for spot " + spot);
            return interactable.Info.spotTransform;
        }
        else
        {
            Debug.Log("GetSpotTransform: No trigger found for spot " + spot);
            return null;
        }
    }
}