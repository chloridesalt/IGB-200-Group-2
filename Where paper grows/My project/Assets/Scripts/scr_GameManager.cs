using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance { get; private set; }
    public Camera MainCamera;
    public GameObject Sun;
    public CinemachineCamera VcamStart;
    public CinemachineCamera VcamTarget;
    public GameObject UI;
    public bool EnableTear = false;
    public bool RoofView = false;
    public float SunRotationSpeed = 1f;
    public float SunMax = 40f;
    public float SunMin = -40f;
    public bool AutoSunMovement = true;
    public float SunTime = 0f;
    [SerializeField] private AudioData Test; 
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
        if (AutoSunMovement)
        {
            SunTime += Time.deltaTime;
        }
        SunMovement();


    }
    // Use this function to change the camera view to the top of the paper bag
    public void CameraTransitionTop()

    {
        /// this a test to see if function works
        AudioManager.Instance.PlayAtPosition(Test, transform.position);
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

    private void SunMovement()
    {
        float value = Mathf.PingPong(SunTime * SunRotationSpeed, SunMax - SunMin) + SunMin;
        Sun.transform.position = new Vector3(Sun.transform.position.x, Sun.transform.position.y, value);
    }

    public void DisableAutoSunMovement()
    {
        AutoSunMovement = false;
    }

    public void EnableAutoSunMovement()
    {
        AutoSunMovement = true;
    }
    
}