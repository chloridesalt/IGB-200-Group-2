using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Pool Setup")]
    [SerializeField] private SoundEmitter emitterPrefab;
    [SerializeField] private int initialPoolSize = 20;

    private Queue<SoundEmitter> pool = new Queue<SoundEmitter>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //// 3D AUDIO IS NOT ENABLED CURRENTLY 
    
        for (int i = 0; i < initialPoolSize; i++)
        {
            SoundEmitter emitter = Instantiate(emitterPrefab, transform);
            emitter.gameObject.SetActive(false);
            pool.Enqueue(emitter);
        }
    }
    
    /// Plays a 3D spatial sound at a fixed world coordinate.

    public SoundEmitter PlayAtPosition(
        AudioClip clip,
        Vector3 position,
        float volume = 1f,
        float minDistance = 2f,
        float maxDistance = 35f,
        bool randomizePitch = true)
    {
        if (clip == null) return null;

        SoundEmitter emitter = GetEmitter();
        float pitch = randomizePitch ? Random.Range(0.92f, 1.08f) : 1.0f;

        emitter.Play(clip, position, volume, pitch, minDistance, maxDistance, null, ReturnToPool);
        return emitter;
    }


    /// Plays a 3D sound attached to a moving object 
    // im retarded  this work? 
    public SoundEmitter PlayAtPosition(AudioData data, Vector3 position, Transform parent = null)
    {
        AudioClip clip = data.GetClip();
        if (clip == null) return null;

        SoundEmitter emitter = GetEmitter();
        emitter.Play(
            clip: clip,
            position: position,
            volume: data.volume,
            pitch: data.GetPitch(),
            minDistance: data.minDistance,
            maxDistance: data.maxDistance,
            attachTo: parent,
            onComplete: ReturnToPool
        );

        return emitter;
    }

    private SoundEmitter GetEmitter()
    {
        SoundEmitter emitter = pool.Count > 0 ? pool.Dequeue() : Instantiate(emitterPrefab);
        emitter.gameObject.SetActive(true);
        return emitter;
    }

    private void ReturnToPool(SoundEmitter emitter)
    {
        emitter.gameObject.SetActive(false);
        emitter.transform.SetParent(transform);
        pool.Enqueue(emitter);
    }
}