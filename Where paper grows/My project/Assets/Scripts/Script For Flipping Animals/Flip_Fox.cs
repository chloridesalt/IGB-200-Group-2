using UnityEngine;
using UnityEngine.AI;

public class Flip_Fox : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform flipHolder;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        FlipFox();
    }

    private void FlipFox()
    {
        if (agent.velocity.x > 0.1f)
        {
            // Face RIGHT
            flipHolder.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (agent.velocity.x < -0.1f)
        {
            // Face LEFT
            flipHolder.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
