using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scr_UIHandler : MonoBehaviour
{
    public GameObject ChoiceContainer;
    public GameObject RoofViewButton;
    public GameObject RoofViewButton2;
    public GameObject PhotoButton;
    public bool RoofViewButtonOn = true;

    public Slider ScaleSlider;
    private bool isSliderBeingDragged = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

   

    // Update is called once per frame
    void Update()
    {
        if (RoofViewButtonOn)
        {
            RoofViewButton.SetActive(true);
            RoofViewButton2.SetActive(true);
        }
        else
        {
            RoofViewButton.SetActive(false);
            RoofViewButton2.SetActive(false);
            PhotoButton.SetActive(false);
        }
    }

    public void ChooseObject(scr_EnvironmentObjects ObjectName)
    {  
        ChoiceContainer.SetActive(false);
        FindAnyObjectByType<scr_CutoutShape>().EnvironmentObject = ObjectName;
        GameManager.s_Instance.TutorialHandler.GetComponent<scr_Tutorial>().ShowCutoutTutorial();
    }

    public void EnableScaleSlider()
    {
        GameManager.s_Instance.TutorialHandler.GetComponent<scr_Tutorial>().ShowEditObjectTutorial();
        ScaleSlider.gameObject.SetActive(true);
    }

    public void ReActivateCamera()
    {
        PhotoButton.SetActive(true);
    }


}
