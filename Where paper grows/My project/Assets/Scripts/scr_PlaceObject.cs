using UnityEngine;

public class scr_PlaceObject : MonoBehaviour
{
    private bool IsPlacingObject = false;
    private Camera MainCamera;
    public GameObject ObjectToPlace;

    void Start()
    {
        MainCamera = GameManager.s_Instance != null ? GameManager.s_Instance.MainCamera : null;
        if (MainCamera == null)
            MainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsPlacingObject)
            PlaceObject();
    }

    public void TargetPosition()
    {
        IsPlacingObject = true;
    }

    public void PlaceObject()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
            if (MainCamera == null) return;
        }

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 1000f;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 5f);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Instantiate(ObjectToPlace, hit.point, Quaternion.identity);
            IsPlacingObject = false;
            return;
        }

        float planeY = 0f;
        var cols = FindObjectsOfType<Collider>();
        foreach (var c in cols)
        {
            if (c == null) continue;
            if (c.gameObject.CompareTag("Floor") || c.gameObject.name.ToLower().Contains("ground"))
            {
                planeY = c.transform.position.y;
                break;
            }
        }

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, planeY, 0));
        float enter;
        if (groundPlane.Raycast(ray, out enter))
        {
            Vector3 planePoint = ray.GetPoint(enter);
            Instantiate(ObjectToPlace, planePoint, Quaternion.identity);
            IsPlacingObject = false;
            return;
        }
    }
}
