using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Infos")]
    public string _interactableName;
    public string Name { get => _interactableName; }
    public string ID
    {
        get
        {
            if (_id == string.Empty)
            {
                _id = System.Guid.NewGuid().ToString();
            }
            return _id;
        }
    }
    private string _id;
    [SerializeField] private Vector3 _tooltipPosition;
    [SerializeField] private float _tooltipDebugSize = 0.1f;
    public Vector3 TooltipPosition { get => transform.position + _tooltipPosition; }

    protected virtual void Start()
    {
        _id = System.Guid.NewGuid().ToString();
    }

    public virtual void DoInteraction()
    {
        if (!InteractableInstigator.IsInteractingWithMe(ID))
        {
            return;
        }

        EventManager.Instance.QueueEvent(new InteractionGameEvent(Name));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(TooltipPosition, _tooltipDebugSize);
    }
}