using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class src_ParticleEmitter : MonoBehaviour
{
    private ParticleSystem leafParticles;
    private ParticleSystem.EmissionModule emissionModule;

    void Awake()
    {
        leafParticles = GetComponent<ParticleSystem>();
        emissionModule = leafParticles.emission;
        emissionModule.enabled = false; // Start disabled
    }

    public void StartEmitting()
    {
        // Ensure the particle system is actually running when emission turns on
        if (!leafParticles.isPlaying)
        {
            leafParticles.Play();
        }

        emissionModule.enabled = true;
    }

    public void StopEmitting()
    {
        emissionModule.enabled = false;
    }

    private void OnDisable()
    {
        // Safety check when object is destroyed OR deactivated/pooled
        if (src_WindManager.Instance != null)
        {
            src_WindManager.Instance.UnregisterObject(this);
        }
    }
}