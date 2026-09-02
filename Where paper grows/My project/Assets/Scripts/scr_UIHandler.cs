using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scr_UIHandler : MonoBehaviour
{
    public GameObject ChoiceContainer;
    public GameObject RoofViewButton;
    public bool RoofViewButtonOn = true;
    public Slider SunSlider;
    private bool isSliderBeingDragged = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SunSlider.onValueChanged.AddListener(OnSliderValueChanged);
        EventTrigger trigger = SunSlider.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = SunSlider.gameObject.AddComponent<EventTrigger>();
        
        AddEventTrigger(trigger, EventTriggerType.PointerDown, OnSliderDragStart);
        AddEventTrigger(trigger, EventTriggerType.PointerUp, OnSliderDragEnd);
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

    private void OnSliderDragStart(BaseEventData data)
    {
        isSliderBeingDragged = true;
        GameManager.s_Instance.DisableAutoSunMovement();
    }

    private void OnSliderDragEnd(BaseEventData data)
    {
        isSliderBeingDragged = false;
        GameManager.s_Instance.EnableAutoSunMovement();
    }

    private void OnSliderValueChanged(float value)
    {
        if (isSliderBeingDragged)
        {
            MoveSun();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RoofViewButtonOn)
        {
            RoofViewButton.SetActive(true);
        }
        else
        {
            RoofViewButton.SetActive(false);
        }
        UpdateSunSlider();
    }

    public void ChooseObject(scr_EnvironmentObjects ObjectName)
    {  
        ChoiceContainer.SetActive(false);
        FindAnyObjectByType<scr_CutoutShape>().EnvironmentObject = ObjectName;
    }

    public void MoveSun()
    {
        GameManager.s_Instance.SunTime = SunSlider.value;
    }

    public void UpdateSunSlider()
    {
        if (!isSliderBeingDragged && SunSlider.value != GameManager.s_Instance.SunTime)
        {
            if(GameManager.s_Instance.SunTime > SunSlider.maxValue){
                GameManager.s_Instance.SunTime = 0f;
            }
            SunSlider.value = GameManager.s_Instance.SunTime;
        }

    }

    public void ReEnableAutoSunMovement()
    {
        GameManager.s_Instance.EnableAutoSunMovement();
    }
}
