using UnityEngine;
using UnityEngine.AI;
public class scr_GroundAnimalBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public scr_AnimalEnvironment AnimalEnvironmentData;
    public Animator animator;
    private GameObject[] environmentObjects;
    private GameObject currentTarget;
    private int currentTargetIndex = 0;
    private float timeSinceTargetReached = 0f;
    private float targetWaitTime = 0f;
    private const float targetReachedDistance = 1.5f;
    public Transform plane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindEnvironmentObjects();
        if (environmentObjects.Length > 0)
        {
            SelectNewTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
    

        if (currentTarget == null && environmentObjects.Length > 0)
        {
            SelectNewTarget();
        }
        Movement();
        UpdateFacingDirection();
        UpdateTargetWait();
    }
    private void UpdateFacingDirection()
    {
        if (agent.velocity.x > 0.1f)
        {
            Vector3 scale = plane.localScale;
            scale.x = -Mathf.Abs(scale.x);
            plane.localScale = scale;
        }
        else if (agent.velocity.x < -0.1f)
        {
            // same as bird :P flips the sprite left n right 
            Vector3 scale = plane.localScale;
            scale.x = Mathf.Abs(scale.x);
            plane.localScale = scale;
        }
    }
    private void FindEnvironmentObjects()
    {
        string environmentTag = AnimalEnvironmentData.EName.ToString();
        environmentObjects = GameObject.FindGameObjectsWithTag(environmentTag);
    }

    private void SelectNewTarget()
    {
        if (environmentObjects.Length == 0)
            return;

        int newIndex = currentTargetIndex;
        if (environmentObjects.Length > 1)
        {
            while (newIndex == currentTargetIndex)
            {
                newIndex = Random.Range(0, environmentObjects.Length);
            }
        }

        currentTargetIndex = newIndex;
        currentTarget = environmentObjects[currentTargetIndex];
        timeSinceTargetReached = 0f;
        Interact(); //this is where the animation triggers
        targetWaitTime = 5f; //Change for length of animation or whatever
    }

    private void UpdateTargetWait()
    {
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < targetReachedDistance)
        {
            timeSinceTargetReached += Time.deltaTime;
            if (timeSinceTargetReached >= targetWaitTime)
            {
                SelectNewTarget();
            }
        }
    }

    public void Movement()
    {
        if (!IsOnNavMesh())
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            transform.position = randomPosition;
        }

        if (currentTarget != null)
        {
            agent.destination = currentTarget.transform.position;
        }
    }

    private bool IsOnNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas);
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f; 
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }

    public void Interact()
    {
        //Animator trigger for interaction
    }
}
