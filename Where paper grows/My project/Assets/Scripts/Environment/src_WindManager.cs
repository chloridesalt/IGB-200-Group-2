using UnityEngine;
using System.Collections;

public class src_WindManager : MonoBehaviour
{
    [Header("Influence Timings")]
    
    // How much external wind is pushed onto foliage
    [SerializeField] private float targetInfluence = 8.0f;
    
    // settings for ease in and ease out
    [SerializeField] private float easeInDuration = 1.5f;
    
    [SerializeField] private float holdDuration = 0.5f;
    
    [SerializeField] private float easeOutDuration = 1.5f;

    private void Awake()
    {
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

    private void Start()
    {
        StartCoroutine(RepeatTimerRoutine());
    }

    public IEnumerator TriggerExternalInfluence()
    {
        Debug.Log("Wind has started.");

        float elapsed = 0f;

        // --- PHASE 1: EASE IN (0.0 -> 5.0) ---
        while (elapsed < easeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / easeInDuration);

            // Quadratic Ease-In: t^2 (starts slow, accelerates)
            float easedT = t * t;

            float influence = Mathf.Lerp(0f, targetInfluence, easedT);
            Shader.SetGlobalFloat("_ExternalInfluence", influence);

            yield return null;
        }

        Shader.SetGlobalFloat("_ExternalInfluence", targetInfluence);

        
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

            float influence = Mathf.Lerp(targetInfluence, 0f, easedT);
            Shader.SetGlobalFloat("_ExternalInfluence", influence);

            yield return null;
        }

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