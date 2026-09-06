using UnityEngine;

public enum ClipSelectionMode
{
    Random,
    RandomNoRepeat,
    Sequential
}

[CreateAssetMenu(fileName = "NewAudioData", menuName = "Audio/Audio Data")]
public class AudioData : ScriptableObject
{
    [Header("Clips")]
    public AudioClip[] clips;
    public ClipSelectionMode selectionMode = ClipSelectionMode.Random;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [Header("3D Spatialization")]
    public float minDistance = 2f;
    public float maxDistance = 35f;

    [System.NonSerialized] private int currentIndex = -1;
    [System.NonSerialized] private int lastPlayedIndex = -1;

    public AudioClip GetClip()
    {
        if (clips == null || clips.Length == 0) return null;
        if (clips.Length == 1) return clips[0];

        switch (selectionMode)
        {
            case ClipSelectionMode.Sequential:
                currentIndex = (currentIndex + 1) % clips.Length;
                return clips[currentIndex];

            case ClipSelectionMode.RandomNoRepeat:
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, clips.Length);
                }
                while (randomIndex == lastPlayedIndex);

                lastPlayedIndex = randomIndex;
                return clips[randomIndex];

            case ClipSelectionMode.Random:
            default:
                return clips[Random.Range(0, clips.Length)];
        }
    }

    public float GetPitch()
    {
        return Random.Range(pitchRange.x, pitchRange.y);
    }

    public void ResetSequence()
    {
        currentIndex = -1;
        lastPlayedIndex = -1;
    }
}