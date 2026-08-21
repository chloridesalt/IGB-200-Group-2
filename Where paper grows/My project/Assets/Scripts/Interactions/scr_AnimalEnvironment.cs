using Unity.Mathematics;
using UnityEngine;

public enum AnimalName
{
    Fox,
    Bird
}
public enum EnvironmentName
{
    Tree,
    Bush,
    Flower
}
[CreateAssetMenu(fileName = "AnimalEnvironment", menuName = "Scriptable Objects/AnimalEnvironment")]
public class scr_AnimalEnvironment : ScriptableObject
{
    public AnimalName AName;
    public EnvironmentName EName;

    public int ObjectCount;
    public float TimeLowerBound;
    public float TimeUpperBound;
}
