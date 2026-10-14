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

    }
    // Use this function to change the camera view to the top of the paper bag
    public void CameraTransitionTop()

    {
        /// this a test to see if function works
        UiTranstionTop();
        UiTranstionBottom();
        AudioManager.Instance.PlayAtPosition(view, transform.position);
        VcamStart.Priority = 5;
        VcamTarget.Priority = 10;
        EnableTear = true;
        RoofView = true;
        scr_Tutorial tutorial = TutorialHandler.GetComponent<scr_Tutorial>();
        tutorial.Invoke(nameof(scr_Tutorial.ShowFirstTearTutorial), 1f);
        tutorial.HideRoofViewTutorial();
    }

    // Use this function to change the camera view to the bottom of the paper bag
    public void CameraTransitionBottom()

    {
        UiTranstionTopReset();
        UiTranstionBottomReset();
        VcamStart.Priority = 10;
        VcamTarget.Priority = 5;
        EnableTear = false;
        RoofView = false;
    }

    public void UiTranstionTop()
    {
        TransitonTopButton.DOKill();
        TransitonTopButton.DOAnchorPos(targetPosition, duration).SetEase(Ease.OutCubic);
        UI.GetComponent<scr_UIHandler>().PhotoButton.DOKill();
        UI.GetComponent<scr_UIHandler>().PhotoButton.DOAnchorPos(targetPosition6, duration).SetEase(Ease.OutCubic);
    }

    public void UiTranstionTopReset()
    {
        TransitonTopButton.DOKill();
        TransitonTopButton.DOAnchorPos(targetPosition3, duration).SetEase(Ease.OutCubic);
    }

    public void UiTranstionBottom()
    {
        TransitonBottomButton.DOKill();
        TransitonBottomButton.DOAnchorPos(targetPosition2, duration).SetEase(Ease.OutCubic);
    }
    public void UiTranstionBottomReset()
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
    

}