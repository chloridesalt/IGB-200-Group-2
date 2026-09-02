using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentObjects", menuName = "Scriptable Objects/EnvironmentObjects")]
public class scr_EnvironmentObjects : ScriptableObject
{
    public GameObject environmentPrefab;
    public Texture2D environmentSilhouette;
}
