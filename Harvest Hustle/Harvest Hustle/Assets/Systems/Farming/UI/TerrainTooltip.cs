using UnityEngine;

public class TerrainTooltip : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public GameObject moistureFeedback;
    public GameObject fertilizationFeedback;
    public GameObject degradationFeedback;

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.position) + offset;
    }

    public void UpdateMoistureFeedback(bool active) => moistureFeedback.SetActive(active);
    public void UpdateFertilizationFeedback(bool active) => fertilizationFeedback.SetActive(active);
    public void UpdateDegradationFeedback(bool active) => degradationFeedback.SetActive(active);
}