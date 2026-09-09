using UnityEngine;
using UnityEngine.AI;
public class scr_FlyingAnimalBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public scr_AnimalEnvironment AnimalEnvironmentData;
    public Animator animator;
    public float BaseOffset = 5f;
    private GameObject[] environmentObjects;
    private GameObject currentTarget;
    private int currentTargetIndex = 0;
    private float timeSinceTargetReached = 0f;
    private float targetWaitTime = 0f;
    private const float targetReachedDistance = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = BaseOffset;
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
        UpdateTargetWait();
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
            agent.Warp(randomPosition);
        }

        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
    }

    private bool IsOnNavMesh()
    {
        return agent != null && agent.isOnNavMesh;
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 navMeshPosition = transform.position - Vector3.up * agent.baseOffset;
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += navMeshPosition;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return navMeshPosition;
    }

    public void Interact()
    {
        //Animator trigger for interaction
    }
}
