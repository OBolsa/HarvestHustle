using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBar_UI : MonoBehaviour
{
    public CanvasGroup ProgressBarCanvas;
    public Image SliderBar;
    private bool IsInProgress { get; set; }

    private void Update()
    {
        if(IsInProgress && Input.anyKeyDown)
        {
            EndFillBar();
        }
    }

    public void StartProgress(StrikeType strikeType, Action onProgressEnd)
    {
        float timeToEnd = GameplayManager.instance.globalConfigs.GetBarFillTimeInSeconds(strikeType);
        SliderBar.fillAmount = 0;
        IsInProgress = true;
        // Começa a aparecer a barrinha
        DOTween.To(() => ProgressBarCanvas.alpha, x => ProgressBarCanvas.alpha = x, 1f, 0.2f).OnComplete(() =>
        {

            // qd ela aparece, ela começa a encher
            DOTween.To(() => SliderBar.fillAmount, x => SliderBar.fillAmount = x, 1f, timeToEnd).OnComplete(() =>
            {
                //qd ela termina de encher, ela executa o que deve ser executado
                onProgressEnd();
                EndFillBar();
            });
            //StartCoroutine(FillBar(timeToEnd, onProgressEnd));
        });
    }

    private void EndFillBar()
    {
        IsInProgress = false;
        // e volta a sumir
        DOTween.To(() => ProgressBarCanvas.alpha, x => ProgressBarCanvas.alpha = x, 0f, 0.2f);
    }
}