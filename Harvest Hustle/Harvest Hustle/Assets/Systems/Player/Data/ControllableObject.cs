using UnityEngine;

public class ControllableObject : MonoBehaviour
{
    [SerializeField] public MovementConfig movementConfig;
    [SerializeField] private float radius;
    [SerializeField] private float feetPoint;
    public Animator anim;

    public Vector2 objectSize
    {
        get
        {
            return new Vector2(Mathf.Abs(radius), Mathf.Abs(feetPoint * 2));
        }
        private set { }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, feetPoint, 0f), 0.1f);
        Gizmos.DrawSphere(transform.position + new Vector3(radius, 0f, 0f), 0.1f);
    }
}