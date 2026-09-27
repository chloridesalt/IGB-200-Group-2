using UnityEngine;
using UnityEngine.AI;
public class scr_FlyingAnimalBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public scr_AnimalEnvironment AnimalEnvironmentData;
    public Animator animator;
    public float BaseOffset = 5f;
    private GameObject[] environmentObjects;
    private GameObject currentTargetObject;
    private Vector3 currentTargetPosition;
    private int currentTargetIndex = 0;
    private bool hasCurrentTarget;
    private bool isRandomTarget;
    private float timeSinceTargetReached = 0f;
    private float targetWaitTime = 0f;
    private const float targetReachedDistance = 1.5f;
    public Transform BirdObject;
    public float LocationRandomizeChance = 0.5f; // Chance to random walk instead of object

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
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
    private void UpdateFacingDirection()
    {
        if (agent.velocity.x > 0.1f) //look right
        {
            Vector3 scale = BirdObject.localScale;
            scale.x = Mathf.Abs(scale.x);
            BirdObject.localScale = scale;
        }
        else if (agent.velocity.x < -0.1f) //look left i think idk my directions i forgot ill find out soon
        {
            Vector3 scale = BirdObject.localScale;
            scale.x = -Mathf.Abs(scale.x);
            BirdObject.localScale = scale;
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
        Vector3 navMeshPosition = transform.position - Vector3.up * agent.baseOffset;
        if (hasCurrentTarget && Vector3.Distance(navMeshPosition, targetPosition) < targetReachedDistance)
        {
            timeSinceTargetReached += Time.deltaTime;
            if (timeSinceTargetReached >= targetWaitTime)
            {
                animator.SetBool("Flying", true);

                FindEnvironmentObjects();
                hasCurrentTarget = false;
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

        if (hasCurrentTarget)
        {
            agent.destination = currentTargetObject != null ? currentTargetObject.transform.position : currentTargetPosition;
        }
    }

    private bool IsOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 navMeshPosition = transform.position - Vector3.up * agent.baseOffset;
        return NavMesh.SamplePosition(navMeshPosition, out hit, 1.0f, NavMesh.AllAreas);
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
        animator.SetBool("Flying", false);
    }
}
