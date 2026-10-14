using UnityEngine;

public class scr_EnvironmentalObject : MonoBehaviour
{
    public string objectName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsClicked()
    {
        //Billy add your juice here
        Debug.Log($"{objectName} Clicked");
    }
}
