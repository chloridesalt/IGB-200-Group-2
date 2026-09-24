using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class src_WindManager : MonoBehaviour
{
    public static src_WindManager Instance { get; private set; }
    [Header("particle settings")]
    [Range(0f, 1f)]
    public float emissionChance = 0.3f; // 30% chance for any individual tree to emit
    public int maxActiveEmitters = 5;    // Hard cap on total simultaneous emitters
   
    private readonly List<src_ParticleEmitter> registeredObjects = new List<src_ParticleEmitter>();
    
    [Header("Influence Timings")]
    
    // How much external wind is pushed onto foliage
    [SerializeField] private float targetInfluence = 8.0f;
    [SerializeField] private float targetInfluence2 = -8.0f;

    // settings for ease in and ease out
    [SerializeField] private float easeInDuration = 1.5f;
    
    [SerializeField] private float holdDuration = 0.5f;
    
    [SerializeField] private float easeOutDuration = 1.5f;

    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // DEFAULT VALUES FOR WIND 

        // Controls the intensity of the wind (The wobble/noise)
        Shader.SetGlobalFloat("_Windintensity", 0.5f);

        // Eases the intensity of windspeed and Windintensity
        Shader.SetGlobalFloat("_WindScale", 0.5f);

        // Dictates the speed of wind 
        Shader.SetGlobalFloat("_Windspeeed", 3f);

        // Starts at 0 so it eases in smoothly from a quiet state
        Shader.SetGlobalFloat("_ExternalInfluence", 0f);
    }
    //Register Object in manager
    public void RegisterObject(src_ParticleEmitter item)
    {
        if (!registeredObjects.Contains(item))
            registeredObjects.Add(item);
    }

    public void UnregisterObject(src_ParticleEmitter tree)
    {
        registeredObjects.Remove(tree);
    }
    private void Start()
    {
        StartCoroutine(RepeatTimerRoutine());
    }
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void TriggerParticles()
    {
        // Reset all currently registered trees
        foreach (var item in registeredObjects)
        {
            item.StopEmitting();
        }

        //  Shuffle a copy of the list for random selection
        List<src_ParticleEmitter> candidates = new List<src_ParticleEmitter>(registeredObjects);
        Shuffle(candidates);

        //  chance up to the max emitter limit
        int activeCount = 0;
        foreach (var item in candidates)
        {
            if (activeCount >= maxActiveEmitters) break;

            if (Random.value <= emissionChance)
            {
                item.StartEmitting();
                activeCount++;
            }
        }

    }
    public void StopParticle()
    {
        foreach (var item in registeredObjects)
        {
            item.StopEmitting();
        }
    }
    public IEnumerator TriggerExternalInfluence()
    {

        float currentTargetInfluence = (Random.value < 0.5f) ? targetInfluence : targetInfluence2;
        Debug.Log($"Wind has started with target influence: {currentTargetInfluence}");
        TriggerParticles();
        Debug.Log(string.Join(", ", registeredObjects));
        float elapsed = 0f;

        // --- PHASE 1: EASE IN (0.0 -> 5.0) ---
        while (elapsed < easeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / easeInDuration);

            // Quadratic Ease-In: t^2 (starts slow, accelerates)
            float easedT = t * t;

            float influence = Mathf.Lerp(0f, currentTargetInfluence, easedT);
            Shader.SetGlobalFloat("_ExternalInfluence", influence);

            yield return null;
        }

        Shader.SetGlobalFloat("_ExternalInfluence", currentTargetInfluence);

        
        if (holdDuration > 0f)
        {
            yield return new WaitForSeconds(holdDuration);
        }

        
        elapsed = 0f;
        while (elapsed < easeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / easeOutDuration);

            
            float easedT = 1f - (1f - t) * (1f - t);

            float influence = Mathf.Lerp(currentTargetInfluence, 0f, easedT);
            Shader.SetGlobalFloat("_ExternalInfluence", influence);

            yield return null;
        }
        StopParticle();
        Shader.SetGlobalFloat("_ExternalInfluence", 0f);
        Debug.Log("Wind has finished.");
    }

    private IEnumerator RepeatTimerRoutine()
    {
        while (true)
        {
            Debug.Log("Wind timer started!");
            yield return new WaitForSeconds(10f);

            // Pauses routine until TriggerExternalInfluence completes
            yield return StartCoroutine(TriggerExternalInfluence());
        }
    }
}