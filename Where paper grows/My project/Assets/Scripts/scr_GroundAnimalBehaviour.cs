using UnityEngine;
using UnityEngine.AI;
public class scr_GroundAnimalBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public scr_AnimalEnvironment AnimalEnvironmentData;
    public Animator animator;
    private GameObject[] environmentObjects;
    private GameObject currentTargetObject;
    private Vector3 currentTargetPosition;
    private bool hasCurrentTarget;
    private bool isRandomTarget;
    private int currentTargetIndex = 0;
    private float timeSinceTargetReached = 0f;
    private float targetWaitTime = 0f;
    private const float targetReachedDistance = 1.5f;
    public Transform plane;
    public float LocationRandomizeChance = 0.5f; // Chance to random walk instead of object

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
    

        if (hasCurrentTarget && !isRandomTarget && currentTargetObject == null)
        {
            hasCurrentTarget = false;
        }

        if (!hasCurrentTarget && environmentObjects.Length > 0)
        {
            SelectNewTarget();
        }
        Movement();
        UpdateTargetWait();
        UpdateFacingDirection();
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

        timeSinceTargetReached = 0f;
        targetWaitTime = 0f;
        hasCurrentTarget = true;

        if (Random.value < LocationRandomizeChance)
        {
            isRandomTarget = true;
            currentTargetObject = null;
            currentTargetPosition = GetRandomPositionOnNavMesh();
            return;
        }

        int newIndex = Random.Range(0, environmentObjects.Length);
        if (environmentObjects.Length > 1)
        {
            while (newIndex == currentTargetIndex)
            {
                newIndex = Random.Range(0, environmentObjects.Length);

            }
        }
        currentTargetIndex = newIndex;
        isRandomTarget = false;
        currentTargetObject = environmentObjects[currentTargetIndex];
        Interact(); //this is where the animation triggers
        targetWaitTime = 5f; //Change for length of animation or whatever
    }

    private void UpdateTargetWait()
    {
        Vector3 targetPosition = currentTargetObject != null ? currentTargetObject.transform.position : currentTargetPosition;
        if (hasCurrentTarget && Vector3.Distance(transform.position, targetPosition) < targetReachedDistance)
        {
            timeSinceTargetReached += Time.deltaTime;
            if (timeSinceTargetReached >= targetWaitTime)
            {
                FindEnvironmentObjects();
                hasCurrentTarget = false;
                SelectNewTarget();
            }
        }
    }

    private void UpdateFacingDirection()
    {
        if (agent.velocity.z < -0.1f) //look left
        {
            Vector3 scale = plane.localScale;
            scale.x = Mathf.Abs(scale.x);
            plane.localScale = scale;

        }
        else if (agent.velocity.z > 0.1f) //look right 
        {
            Vector3 scale = plane.localScale;
            scale.x = -Mathf.Abs(scale.x);
            plane.localScale = scale;

        }
    }
    public void Movement()
    {
        if (!IsOnNavMesh())
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            transform.position = randomPosition;
        }

        if (hasCurrentTarget)
        {
            agent.destination = currentTargetObject != null ? currentTargetObject.transform.position : currentTargetPosition;
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
