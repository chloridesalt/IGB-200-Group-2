using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class src_ParticleEmitter : MonoBehaviour
{
    [Header("Random Spawning Settings")]
    [SerializeField] private float minInterval = 0.2f; // Min time between leaf pops
    [SerializeField] private float maxInterval = 0.8f; // Max time between leaf pops

    private ParticleSystem leafParticles;
    private Coroutine emitRoutine;

    void Awake()
    {
        leafParticles = GetComponent<ParticleSystem>();

        // Turn off continuous emission rate so particles ONLY spawn via code
        var emission = leafParticles.emission;
        emission.rateOverTime = 0;
    }

    public void StartEmitting()
    {
        StopEmitting();
        emitRoutine = StartCoroutine(EmitLeavesRoutine());
    }
    public void WindRight()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void WindLeft()
    {
        transform.rotation = Quaternion.Euler(-180, 0, 0);
    }
    public void StopEmitting()
    {
        if (emitRoutine != null)
        {
            StopCoroutine(emitRoutine);
            emitRoutine = null;
        }
    }

    private IEnumerator EmitLeavesRoutine()
    {
        if (!leafParticles.isPlaying)
        {
            leafParticles.Play();
        }

        while (true)
        {
            //  Wait a random amount of time before popping out leaves
            float randomWait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomWait);

            //  Randomly pick either 1 or 2 leaves (3 is exclusive in Random.Range for ints)
            int leafAmount = Random.Range(1, 3);

            // Emit exact count
            leafParticles.Emit(leafAmount);
        }
    }

    private void OnDisable()
    {
        StopEmitting();

        if (src_WindManager.Instance != null)
        {
            src_WindManager.Instance.UnregisterObject(this);
        }
    }
}