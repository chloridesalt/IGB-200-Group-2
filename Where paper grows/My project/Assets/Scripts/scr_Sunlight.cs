using UnityEngine;

public class scr_Sunlight : MonoBehaviour
{
    private Vector3 SunPosition;
    private Vector3 AimPosition;
    private float Alpha = 0f;
    private const int MaxTransparentHits = 32;
    private const float RayOffset = 0.001f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SunPosition = transform.position;
        AimPosition = new Vector3(-transform.position.x, 5, -transform.position.z);
        Sunlight();
    }

    private void Sunlight()
    {
        Vector3 rayDirection = (AimPosition - SunPosition).normalized;
        Vector3 rayOrigin = SunPosition;

        for (int hitCount = 0; hitCount < MaxTransparentHits; hitCount++)
        {
            if (!Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo))
            {
                break;
            }

            if (IsHitSolid(hitInfo))
            {
                //Start grass growth at the hit point 
                break;
            }

            rayOrigin = hitInfo.point + rayDirection * RayOffset;
        }

        Debug.DrawRay(SunPosition, rayDirection * 10000f, Color.yellow);
    }

    bool IsHitSolid(RaycastHit hit)
    {

        Renderer renderer = hit.collider.GetComponent<Renderer>();
        if (renderer == null || renderer.sharedMaterial == null) return true;

        Material material = renderer.sharedMaterial;
        if (material.color.a <= Alpha) return false;

        Texture2D tex = material.mainTexture as Texture2D;
        if (tex == null) return true; 

        Vector2 pixelUV = hit.textureCoord;
        
        Color pixelColor = tex.GetPixelBilinear(pixelUV.x, pixelUV.y);
        
        return pixelColor.a * material.color.a > Alpha;
    }

}
