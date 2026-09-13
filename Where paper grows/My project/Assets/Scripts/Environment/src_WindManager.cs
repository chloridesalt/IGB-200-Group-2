using UnityEngine;

public class src_WindManager : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static readonly int IntensityID = Shader.PropertyToID("_ExternalInfluence");
    void Awake()
    {

        // DEFAULT VALUES FOR WIND 

      
        // Controls the intensity of the wind (The wobble/noise)
        Shader.SetGlobalFloat("_Windintensity", 0.5f);

        // Eases the intensity of windspeed and Windintensity )
        Shader.SetGlobalFloat("_WindScale", 0.5f);

        // dictates the speed of wind 
        Shader.SetGlobalFloat("_Windspeeed", 3f);

        // dictates where the wind is pushing (negative for left, postive for right)
        Shader.SetGlobalFloat("_ExternalInfluence", 5.0f);

     

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
