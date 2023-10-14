using UnityEngine;

public class WiggleUI : MonoBehaviour
{
    [SerializeField] private float wiggleYAmount = 10f;
    [SerializeField] private float wiggleXAmount = 10f;
    [SerializeField] private float wiggleYSpeed = 2f;
    [SerializeField] private float wiggleXSpeed = 2f;

    private float originalY;
    private float originalX;
    private float time = 0f;

    private void Start()
    {
        originalY = transform.localPosition.y;
    }

    private void Update()
    {
        time += Time.deltaTime * wiggleYSpeed;
        time += Time.deltaTime * wiggleXSpeed;
        float y = originalY + Mathf.Sin(time) * wiggleYAmount * GetScreenProportion();
        float x = originalX + Mathf.Sin(time) * wiggleXAmount * GetScreenProportion();

        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
    }

    private float GetScreenProportion()
    {
        float screenHeight = Screen.height;
        float screenProportion = screenHeight / 1080f; // 1080 is a reference height
        return screenProportion;
    }
}