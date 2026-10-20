using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance { get; private set; }
    public Camera MainCamera;
    public GameObject Sun;
    public CinemachineCamera VcamStart;
    public CinemachineCamera VcamTarget;
    public GameObject UI;
    public GameObject TutorialHandler;
    public scr_InputManager InputManager;
    public bool EnableTear = false;
    public bool RoofView = false;
    public float SunRotationSpeed = 1f;
    public float SunMax = 40f;
    public float SunMin = -40f;
    public bool AutoSunMovement = true;
    public float SunTime = 0f;
    public bool FirstObjectPlaced = false;
    [Header("UISCENE ")]
    [SerializeField] private RectTransform TransitonTopButton;
    [SerializeField] private RectTransform TransitonBottomButton;

    [SerializeField] private scr_UIHandler uIHandler;

    public Vector2 targetPosition = new Vector2(100f, 0f);
    public Vector2 targetPosition2 = new Vector2(100f, 0f);
    public Vector2 targetPosition3 = new Vector2(0f, 0f);
    public Vector2 targetPosition4 = new Vector2(0f, 0f);
    public Vector2 targetPosition5 = new Vector2(100f, 0f);
    public Vector2 targetPosition6 = new Vector2(0f, 0f);
   
    public float duration = 0.5f;
   
    [SerializeField] private AudioData view;

    private Vector2 originalPosition;
    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
       

        s_Instance = this;

    }
     void Update()
    {
        if (AutoSunMovement)
        {
            SunTime += Time.deltaTime;
        }
        SunMovement();
        if (InputManager.AFK && RoofView)
        {
            scr_Tutorial tutorial = TutorialHandler.GetComponent<scr_Tutorial>();
            tutorial.ShowAFKTutorial();
        }
        else
        {
            scr_Tutorial tutorial = TutorialHandler.GetComponent<scr_Tutorial>();
            tutorial.HideAFKTutorial();
        }
        if (InputManager.interactAction.WasPerformedThisFrame() && TutorialHandler.GetComponent<scr_Tutorial>().ButtonTutorialShown)
        {
            TutorialHandler.GetComponent<scr_Tutorial>().HideButtonTutorial();
        }
        if (RoofView && TutorialHandler.GetComponent<scr_Tutorial>().IntroTutorial.activeSelf)
        {
            TutorialHandler.GetComponent<scr_Tutorial>().HideRoofViewTutorial();
        }
         if (RoofView && TutorialHandler.GetComponent<scr_Tutorial>().FirstTearTutorial.activeSelf && InputManager.interactAction.WasPerformedThisFrame())
        {
            TutorialHandler.GetComponent<scr_Tutorial>().HideFirstTearTutorial();
        }
        if (InputManager.interactAction.WasPerformedThisFrame() && !RoofView)
        {
            InteractWithEnvironment();
        }
    }
    // Use this function to change the camera view to the top of the paper bag
    public void CameraTransitionTop()

    {
        /// this a test to see if function works
       scr_Tutorial tutorial = TutorialHandler.GetComponent<scr_Tutorial>();
        UITransitionTop();
        if (tutorial.TutorialCompleted)
        {
            UITransitionBottom();
        }

        AudioManager.Instance.PlayAtPosition(view, transform.position);
        VcamStart.Priority = 5;
        VcamTarget.Priority = 10;
        EnableTear = true;
        RoofView = true;
        
        tutorial.Invoke(nameof(scr_Tutorial.ShowFirstTearTutorial), 1f);
        tutorial.HideRoofViewTutorial();
    }

    // Use this function to change the camera view to the bottom of the paper bag
    public void CameraTransitionBottom()

    {
        
        UITransitionTopReset();
        
        UITransitionBottomReset();
        VcamStart.Priority = 10;
        VcamTarget.Priority = 5;
        EnableTear = false;
        RoofView = false;
    }
    public void CameraTransitionBottomfromcutout()

    {

        

        UITransitionBottomReset();
        VcamStart.Priority = 10;
        VcamTarget.Priority = 5;
        EnableTear = false;
        RoofView = false;
    }

    public void UITransitionTop()
    {
        TransitonTopButton.DOKill();
        TransitonTopButton.DOAnchorPos(targetPosition, duration).SetEase(Ease.OutCubic);
        UI.GetComponent<scr_UIHandler>().PhotoButton.DOKill();
        UI.GetComponent<scr_UIHandler>().PhotoButton.DOAnchorPos(targetPosition6, duration).SetEase(Ease.OutCubic);
    }

    public void UITransitionTopReset()
    {
        TransitonTopButton.DOKill();
        TransitonTopButton.DOAnchorPos(targetPosition3, duration).SetEase(Ease.OutCubic);
    }

    public void UITransitionBottom()
    {
        TransitonBottomButton.DOKill();
        TransitonBottomButton.DOAnchorPos(targetPosition2, duration).SetEase(Ease.OutCubic);
    }
    public void UITransitionBottomReset()
    {
        TransitonBottomButton.DOKill();
        TransitonBottomButton.DOAnchorPos(targetPosition4, duration).SetEase(Ease.OutCubic);
    }
    public void UiTransitionCameraReset()
    {
        UI.GetComponent<scr_UIHandler>().PhotoButton.DOKill();
        UI.GetComponent<scr_UIHandler>().PhotoButton.DOAnchorPos(targetPosition5, duration).SetEase(Ease.OutCubic);
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
    private void InteractWithEnvironment()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.lookValue);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");
            scr_EnvironmentalObject envObject = hit.collider.GetComponent<scr_EnvironmentalObject>();
            if (envObject != null)
            {
                envObject.IsClicked();
            }
        }
    }

}