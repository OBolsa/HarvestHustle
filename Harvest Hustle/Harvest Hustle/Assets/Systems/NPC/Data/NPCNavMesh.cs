using UnityEngine;
using UnityEngine.AI;

public class NPCNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Transform position)
    {
        agent.destination = position.position;
    }
}