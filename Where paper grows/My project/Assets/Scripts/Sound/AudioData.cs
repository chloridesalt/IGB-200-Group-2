using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/Sound Effect Data")]
public class AudioData : ScriptableObject
{
    [Header("Clips")]
    public AudioClip[] clips;    // im retarded  

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [Header("3D Spatialization")]
    public float minDistance = 2f;
    public float maxDistance = 35f;

    public AudioClip GetClip()
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }

    public float GetPitch()
    {
        return Random.Range(pitchRange.x, pitchRange.y);
    }
}