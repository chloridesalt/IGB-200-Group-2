using UnityEngine;
using UnityEngine.UI;

public class scr_UIHandler : MonoBehaviour
{
    public GameObject ObjectHandler;
    public GameObject ChoiceContainer;
    public GameObject RoofViewButton;
    public bool RoofViewButtonOn = true;
    public Slider SunSlider;

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
        }
        else
        {
            RoofViewButton.SetActive(false);
        }
    }

    public void ChooseObject(GameObject ObjectName)
    {  
        ChoiceContainer.SetActive(false);
        GameManager.s_Instance.ChangeView();
        ObjectHandler.GetComponent<scr_PlaceObject>().TargetPosition(ObjectName);
    }

    public void MoveSun()
    {
        float value = SunSlider.value;
        GameManager.s_Instance.Sun.transform.position = new Vector3(GameManager.s_Instance.Sun.transform.position.x, GameManager.s_Instance.Sun.transform.position.y, value);
    }
}
