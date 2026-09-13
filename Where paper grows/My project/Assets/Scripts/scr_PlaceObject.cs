using NUnit.Framework.Internal;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class scr_PlaceObject : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navmesh;
    [SerializeField] private AudioData placeObjectSound;
    

    private bool IsPlacingObject = false;
    private Camera MainCamera;
    public GameObject ObjectToPlace;
    private scr_InputManager inputManager;
    public int TreeCount = 0;
    public int BushCount = 0;
    public int FlowerCount = 0;
    private string ObjectName;
    private bool MusicOn = false;

    void Start()
    {
        inputManager = GameManager.s_Instance.GetComponent<scr_InputManager>();
        MainCamera = GameManager.s_Instance != null ? GameManager.s_Instance.MainCamera : null;
        if (MainCamera == null)
            MainCamera = Camera.main;
    }

    void Update()
    {
        Debug.Log(inputManager.interactAction.WasPerformedThisFrame());
        if (inputManager.interactAction.WasPerformedThisFrame() && IsPlacingObject)
        {
            Debug.Log("wasd");
            PlaceObject(ObjectToPlace);
            MusicOn = !MusicOn;
            EventManager.RaiseOnMusicStart(MusicOn);
        }
    }

    public void TargetPosition(GameObject ObjectToPlace)
    {
        this.ObjectToPlace = ObjectToPlace;
        IsPlacingObject = true;
    }

    public void PlaceObject(GameObject ObjectToPlace)
    {
        ObjectName = ObjectToPlace.name;
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
            if (MainCamera == null) return;
        }
       

        Ray ray = MainCamera.ScreenPointToRay(inputManager.lookValue);
        RaycastHit hit;
        float maxDistance = 1000f;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 5f);

        if (Physics.Raycast(ray, out hit, maxDistance) && hit.collider.CompareTag("Floor") )
        {
            
            Instantiate(ObjectToPlace, hit.point, Quaternion.identity);
            GameManager.s_Instance.UI.GetComponent<scr_UIHandler>().RoofViewButtonOn = true;
            switch (ObjectName)
            {
                case "pre_Tree":
                    IsPlacingObject = false;
                    AudioManager.Instance.PlayAtPosition(placeObjectSound, transform.position);
                    TreeCount += 1;
                    
                    return;
                case "pre_Bush":
                    IsPlacingObject = false;
                    AudioManager.Instance.PlayAtPosition(placeObjectSound, transform.position);
                    BushCount += 1;
        
                    return;
                case "pre_Flower":
                    IsPlacingObject = false;
                    AudioManager.Instance.PlayAtPosition(placeObjectSound, transform.position);
                    FlowerCount += 1;
                    return;
            }
            
        }


          

        }

    }

