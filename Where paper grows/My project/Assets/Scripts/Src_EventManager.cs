using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<bool> OnMusicStart;


    public static void RaiseOnMusicStart(bool isOn)
    {
        OnMusicStart?.Invoke(isOn);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
