using UnityEngine;

public class scr_UIHandler : MonoBehaviour
{
    public GameObject ObjectHandler;
    public GameObject ChoiceContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseObject(GameObject ObjectName)
    {        
        ChoiceContainer.SetActive(false);
        GameManager.s_Instance.ChangeView();
        ObjectHandler.GetComponent<scr_PlaceObject>().TargetPosition(ObjectName);

    }
}
