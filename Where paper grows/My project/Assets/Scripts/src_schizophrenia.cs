using System;
using UnityEngine;

public class schizophrenia : MonoBehaviour
{
    void Update()
    {
        // Triggers once exactly when the Spacebar is pressed down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Schizophrenia();
        }
    }

    void Schizophrenia()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.LogError($"Im in your walls [Frame: {Time.frameCount}]");
        } 
      
    }
}
