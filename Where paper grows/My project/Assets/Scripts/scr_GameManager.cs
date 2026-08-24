using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance { get; private set; }
    public Camera MainCamera;
    public GameObject Sun;
    public CinemachineCamera VcamStart;
    public CinemachineCamera VcamTarget;
    public KeyCode triggerKey = KeyCode.Space;
    public KeyCode triggerKey2 = KeyCode.P;
    public bool EnableTear = false;
    public bool RoofView = false;

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        s_Instance = this;

        DontDestroyOnLoad(gameObject); 
    }
     void Update()
    {
       // Used to test camera transition  
        if (Input.GetKeyDown(triggerKey))
        {
            CameraTransitionTop();
        }
        if (Input.GetKeyDown(triggerKey2))
        {
            CameraTransitionBottom();
        }


    }
    // Use this function to change the camera view to the top of the paper bag
    public void CameraTransitionTop()

    {
        VcamStart.Priority = 5;
        VcamTarget.Priority = 10;
        EnableTear = true;
        RoofView = true;
    }

    // Use this function to change the camera view to the bottom of the paper bag
    public void CameraTransitionBottom()

    {
        VcamStart.Priority = 10;
        VcamTarget.Priority = 5;
        EnableTear = false;
        RoofView = false;
    }

    public void ChangeView()
    {
        if (RoofView)
        {
            CameraTransitionBottom();
        }
        else
        {
            CameraTransitionTop();
        }
    }

}