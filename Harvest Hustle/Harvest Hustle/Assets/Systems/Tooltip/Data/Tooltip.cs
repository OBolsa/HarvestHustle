using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public bool Active { get { return gameObject.activeSelf; } }

    public RectTransform backgroundRect;
    public float textPaddingSize = 4f;

    private Vector3 _target;
    private TMP_Text _tooltipText;
    private Camera mainCamera;
    private Vector3 _tooltipPosition = new Vector3();

    private void OnEnable()
    {
        TimeManagerStrike.Instance.StrikePassed += UpdateText;
        GameplayManager.instance.sceneManager.SceneLoaded += GetCamera;
    }

    private void OnDisable()
    {
        TimeManagerStrike.Instance.StrikePassed -= UpdateText;
        GameplayManager.instance.sceneManager.SceneLoaded -= GetCamera;
    }

    private void Awake()
    {
        _tooltipText = GetComponentInChildren<TMP_Text>();
        HideTooltip();
    }

    private void Start()
    {
        GetCamera();
    }

    private void Update()
    {
        if (_target != null)
        {
            _tooltipPosition = mainCamera.WorldToScreenPoint(_target);
            transform.position = _tooltipPosition;
        }
    }

    public void UpdateText()
    {
        if(InteractableInstigator.HaveClosestInteractable)
            ShowTooltip(InteractableInstigator.ClosestInteractable.TooltipPosition, InteractableInstigator.ClosestInteractable.Name);
    }

    public void ShowTooltip(Vector3 targetPosition, string tooltipString)
    {
        gameObject.SetActive(true);

        //Renderer objectRenderer = targetPosition.GetComponent<Renderer>();
        //_objectSize.y = objectRenderer ? objectRenderer.bounds.size.y : 0;

        _target = targetPosition;
        _tooltipText.text = tooltipString;
        Vector2 backgroundSize = new Vector2(_tooltipText.preferredWidth + textPaddingSize * 2f, _tooltipText.preferredHeight + 4 * 2f);
        backgroundRect.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public void GetCamera()
    {
        HideTooltip();
        mainCamera = Camera.main;
    }
}