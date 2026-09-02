using UnityEditor;
using UnityEngine;

public class scr_CutoutShape : MonoBehaviour
{
    public scr_EnvironmentObjects EnvironmentObject;
    public GameObject ObjectHandler;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectHandler = FindObjectOfType<scr_PlaceObject>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnvironmentObject != null)
        {
            CutoutComplete();
        }
    }

    private void CutoutComplete()
    {
        GameManager.s_Instance.ChangeView();
        ObjectHandler.GetComponent<scr_PlaceObject>().TargetPosition(EnvironmentObject.environmentPrefab);
        Destroy(gameObject);
    }
}
