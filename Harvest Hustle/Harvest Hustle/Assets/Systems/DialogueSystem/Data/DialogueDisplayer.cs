using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueDisplayer : MonoBehaviour
{
    [Header("Elements")]
    public CanvasGroup elements;
    public GameObject nextTextIndication;

    [Header("Properties")]
    public Image speakerPortrait;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI speakerSpeech;
    private TextFormatter formatter;

    [Header("Properties")]
    public float fadeInOutTime = 0.2f;
    public int charactersPerLine = 32;
    private float textSpeed;

    public Dialogue dialogueToDisplay;

    private Dialogue.Speech currentSpeech;

    private void Start()
    {
        formatter = new TextFormatter(charactersPerLine);
    }

    public void DisplaySpeech(Dialogue.Speech speech)
    {
        if (speech == null)
        {
            return;
        }

        if (elements.alpha == 0)
        {
            DisplayDialogue();
        }

        currentSpeech = speech;

        if(currentSpeech.Speaker == null)
        {
            speakerPortrait.enabled = false;
            speakerName.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            speakerPortrait.enabled = true;
            speakerPortrait.sprite = currentSpeech.Speaker.GetPortrait();

            speakerName.transform.parent.gameObject.SetActive(true);
            speakerName.text = currentSpeech.Speaker.GetName();
        }

        textSpeed = GameplayManager.instance.globalConfigs.GetTextSpeed(currentSpeech.TextSpeed);

        if (currentSpeech.TextSpeed == TextSpeedType.Fast)
        {
            // Do Shake
            float shakeDuration = 0.5f; // Adjust the duration of the shake
            float shakeStrength = 5f; // Adjust the strength of the shake
            speakerSpeech.rectTransform.parent.DOShakePosition(shakeDuration, shakeStrength);
        }

        string textToSpeech = formatter.FormatText(currentSpeech.Text);
        speakerSpeech.fontSize = formatter.FontSize;
        speakerSpeech.text = textToSpeech;
        StartSpeech();

        StartCoroutine(DisplayTextLetterByLetter(speakerSpeech));
    }


    public void StartSpeech()
    {
        StopAllCoroutines();
        currentSpeech.IsLetteringDone = false;
        speakerSpeech.maxVisibleCharacters = 0;
        nextTextIndication.SetActive(false);
    }

    public void EndSpeech()
    {
        if (currentSpeech == null)
            return;

        StopAllCoroutines();
        currentSpeech.IsLetteringDone = true;
        speakerSpeech.maxVisibleCharacters = speakerSpeech.text.Length;
        nextTextIndication.SetActive(true);
    }

    public void DisplayDialogue()
    {
        DOTween.To(() => elements.alpha, x => elements.alpha = x, 1f, fadeInOutTime);
        StartCoroutine(GameplayManager.instance.CurrentState.CallForCutscene());
    }
    public void CloseDialogue()
    {
        DOTween.To(() => elements.alpha, x => elements.alpha = x, 0f, fadeInOutTime);
        StartCoroutine(GameplayManager.instance.CurrentState.CallForCutscene());
    }

    private IEnumerator DisplayTextLetterByLetter(TextMeshProUGUI textMesh)
    {
        int visibleCharacters = 0;
        int totalCharacters = textMesh.text.Length;
        float timePerCharacter = 1.0f / textSpeed;

        while (visibleCharacters < totalCharacters)
        {
            visibleCharacters++;
            textMesh.maxVisibleCharacters = visibleCharacters;

            yield return new WaitForSeconds(timePerCharacter);
        }

        nextTextIndication.SetActive(true);
        currentSpeech.IsLetteringDone = true;
    }
}