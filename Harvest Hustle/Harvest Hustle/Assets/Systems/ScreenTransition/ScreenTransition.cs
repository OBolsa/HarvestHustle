using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public static ScreenTransition Instance;
    public CanvasGroup canvas;

    private void Awake()
    {
        Instance = this;
    }

    public void DoTransition(List<Action> doInTransition)
    {
        DOTween.To(() => canvas.alpha, x => canvas.alpha = x, 1f, 0.25f).OnComplete(() =>
        {
            StartCoroutine(DoMultipleTransitions(doInTransition));
        });
    }
    public void DoTransition(Action doInTransition)
    {
        DOTween.To(() => canvas.alpha, x => canvas.alpha = x, 1f, 0.15f)
            .OnComplete(() =>
            {
                // Delay before calling doInTransition
                DOTween.Sequence()
                    .AppendInterval(0.2f)
                    .OnKill(() =>
                    {
                        doInTransition();
                    })
                    .AppendInterval(0.2f)
                    .OnComplete(() =>
                    {
                        // Start fading out after the second delay
                        DOTween.To(() => canvas.alpha, x => canvas.alpha = x, 0f, 0.15f);
                    });
            });
    }

    public IEnumerator DoMultipleTransitions(List<Action> doInTransition)
    {
        int actionsCounter = 0;

        while (actionsCounter < doInTransition.Count)
        {
            doInTransition[actionsCounter].Invoke();
            actionsCounter++;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        DOTween.To(() => canvas.alpha, x => canvas.alpha = x, 0f, 0.25f);
    }    
}