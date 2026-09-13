using UnityEngine;

public class scr_Sunlight : MonoBehaviour
{
    private Vector3 SunPosition;
    private Vector3 AimPosition;
    private scr_InputManager inputManager;
    private const int MaxTransparentHits = 32;
    private const float RayOffset = 0.001f;
    public GameObject GrassPrefab;
    public GameObject Godray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GameManager.s_Instance.GetComponent<scr_InputManager>();
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
                } else
                {
                    Godray.SetActive(false);
                }
                transform.LookAt(hitInfo.point);
                break;
            }

            rayOrigin = hitInfo.point + rayDirection * RayOffset;
        }

        Debug.DrawRay(SunPosition, rayDirection * 10000f, Color.yellow);
    }
}
