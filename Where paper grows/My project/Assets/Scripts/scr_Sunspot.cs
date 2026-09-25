using UnityEngine;

public class scr_Sunspot : MonoBehaviour
{
    private scr_InputManager inputManager;
    private scr_Sunlight sunlight;
    private const int MaxTransparentHits = 32;
    private const float RayOffset = 0.001f;
    public GameObject GrassPrefab;
    public GameObject Godray;

    void Start()
    {
        inputManager = GameManager.s_Instance.GetComponent<scr_InputManager>();
        sunlight = FindAnyObjectByType<scr_Sunlight>();
    }

    void LateUpdate()
    {
        Sunlight();
    }

    private void Sunlight()
    {
        if (sunlight != null)
        {
            transform.rotation = sunlight.transform.rotation;
        }

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = sunlight != null
            ? sunlight.RayDirection
            : transform.forward;

        if (rayDirection.sqrMagnitude <= 0.000001f)
        {
            return;
        }

        rayDirection.Normalize();

        for (int hitCount = 0; hitCount < MaxTransparentHits; hitCount++)
        {
            if (!Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo))
            {
                break;
            }

            if (inputManager.IsHitSolid(hitInfo))
            {
                if (hitInfo.collider.CompareTag("Floor"))
                {
                    Instantiate(GrassPrefab, hitInfo.point, Quaternion.identity);
                    Godray.SetActive(true);
                }
                else if (hitInfo.collider.CompareTag("Grass"))
                {
                    scr_GrassGrowth grassGrowth = hitInfo.collider.GetComponent<scr_GrassGrowth>();
                    if (grassGrowth != null)
                    {
                        grassGrowth.GrowGrass();
                    }
                    Godray.SetActive(true);
                }
                else
                {
                    Godray.SetActive(false);
                }

                break;
            }

            rayOrigin = hitInfo.point + rayDirection * RayOffset;
        }

        Debug.DrawRay(rayOrigin, rayDirection * 10000f, Color.yellow);
    }
}
