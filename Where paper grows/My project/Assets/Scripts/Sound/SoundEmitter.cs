using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;
    private System.Action<SoundEmitter> returnToPoolAction;
    private Coroutine stopCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
    }
    // im retarded  
    public void Play(
        AudioClip clip,
        Vector3 position,
        float volume = 1f,
        float pitch = 1f,
        float minDistance = 2f,
        float maxDistance = 35f,
        Transform attachTo = null,
        System.Action<SoundEmitter> onComplete = null)
    {
        transform.position = position;
        if (attachTo != null) transform.SetParent(attachTo);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        returnToPoolAction = onComplete;
        audioSource.Play();

        if (stopCoroutine != null) StopCoroutine(stopCoroutine);
        stopCoroutine = StartCoroutine(DisableAfterDuration(clip.length / Mathf.Max(0.1f, pitch)));
    }

    private IEnumerator DisableAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        transform.SetParent(null); // Detach if attached to a moving target
        returnToPoolAction?.Invoke(this);
    }
}