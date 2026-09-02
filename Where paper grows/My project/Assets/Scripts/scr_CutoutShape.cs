using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class scr_CutoutShape : MonoBehaviour
{
    public scr_EnvironmentObjects EnvironmentObject;
    public GameObject ObjectHandler;

    private List<BoxCollider> colliders = new List<BoxCollider>();
    private GameObject environmentSilhouette;
    private bool lateInit = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectHandler = FindObjectOfType<scr_PlaceObject>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!lateInit && EnvironmentObject != null)
        {
            LateStart();
        }*/

        if (EnvironmentObject != null)
        {
            //CheckCutout();
            CutoutComplete();
        }
    }

    private void CutoutComplete()
    {
        GameManager.s_Instance.ChangeView();
        ObjectHandler.GetComponent<scr_PlaceObject>().TargetPosition(EnvironmentObject.environmentPrefab);
        Destroy(gameObject);
    }

    private void CheckCutout()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == colliders[i])
            {
                colliders.Remove(colliders[i]);
                Debug.Log("Collider Removed");
                i--;
            }
        }
    }

    private void LateStart()
    {
        environmentSilhouette = Instantiate(EnvironmentObject.environmentSilhouette, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(-90, 90, 0));
        BoxCollider[] colliderArray = environmentSilhouette.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliderArray.Length; i++)
        {
            colliders.Add(colliderArray[i]);
        }

        lateInit = true;
    }
}
