using TMPro;
using UnityEngine;

public class CompostBinTooltip_UI : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public GameObject displayer;
    public TMP_Text BrownSubstractAmountDisplayer;
    public TMP_Text GreenSubstractAmountDisplayer;

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.position) + offset;
    }

    public void DisplayFeedback(bool active)
    {
        displayer.SetActive(active);
    }

    public void UpdateValues(int brownAmount, int greenAmount)
    {
        BrownSubstractAmountDisplayer.text = $"{brownAmount}/{GameplayManager.instance.globalConfigs.BrownSubstractToMakeFertilization_Amount}";
        GreenSubstractAmountDisplayer.text = $"{greenAmount}/{GameplayManager.instance.globalConfigs.GreenSubstractToMakeFertilization_Amount}";
    }
}