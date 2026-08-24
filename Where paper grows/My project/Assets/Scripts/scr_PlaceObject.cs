using UnityEngine;
using UnityEngine.UI;

public class scr_PlaceObject : MonoBehaviour
{
    private bool IsPlacingObject = false;
    private Camera MainCamera;
    public GameObject ObjectToPlace;
    public int TreeCount = 0;
    public int BushCount = 0;
    public int FlowerCount = 0;
    private string ObjectName;

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
        ObjectName = ObjectToPlace.name;
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
            if (MainCamera == null) return;
        }

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 1000f;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 5f);

        if (Physics.Raycast(ray, out hit, maxDistance) && hit.collider.CompareTag("Floor") )
        {
            Instantiate(ObjectToPlace, hit.point, Quaternion.identity);
            switch (ObjectName)
            {
                case "pre_Tree":
                    IsPlacingObject = false;
                    TreeCount += 1;
                    return;
                case "pre_Bush":
                    IsPlacingObject = false;
                    BushCount += 1;
                    return;
                case "pre_Flower":
                    IsPlacingObject = false;
                    FlowerCount += 1;
                    return;
            }
            
        }


          

        }

    }

