using Unity.Mathematics;
using UnityEngine;

public enum AnimalName
{
    pre_Fox,
    pre_Bird
}
public enum EnvironmentName
{
    pre_Tree,
    pre_Bush,
    pre_Flower
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
