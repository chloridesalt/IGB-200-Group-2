using UnityEngine;

public class scr_GrassGrowth : MonoBehaviour
{
    public float Growth = 0f;
    public float GrowthRate = 0.1f; 
    public float TimeToGrow = 5.0f;
    public float MaxScale = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrowGrass()
    {
        Growth += GrowthRate * Time.deltaTime;
        float scale = Mathf.Lerp(0f, MaxScale, Growth / TimeToGrow);
        transform.localScale = new Vector3(1, scale, 1);
        float yposition = Mathf.Lerp(5.1f, 6f, Growth / TimeToGrow);
        transform.position = new Vector3(transform.position.x, yposition, transform.position.z);
    }
}
