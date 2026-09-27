using UnityEngine;
using UnityEngine.AI;

public class Flip_Snake : MonoBehaviour
{
    private NavMeshAgent agent;

    public Transform flipHolder;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        FlipSnake();
    }

    private void FlipSnake()
    {
        if (flipHolder == null || agent == null)
            return;

        Vector3 scale = flipHolder.localScale;

        if (agent.velocity.x > 0.1f)
        {
            // Moving right
            scale.x = -Mathf.Abs(scale.x);
        }
        else if (agent.velocity.x < -0.1f)
        {
            // Moving left
            scale.x = Mathf.Abs(scale.x);
        }

        flipHolder.localScale = scale;
    }
}