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
        if (EnvironmentObject != null)
        {
            if (!lateInit)
            {
                LateStart();
            }
            CheckCutout();
            //CutoutComplete();
        }
    }

    private void OnMouseEnter()
    {
        
    }

    private void CutoutComplete()
    {
        GameManager.s_Instance.ChangeView();
        ObjectHandler.GetComponent<scr_PlaceObject>().TargetPosition(EnvironmentObject.environmentPrefab);
        Destroy(environmentSilhouette);
        Destroy(gameObject);
    }

    private void CheckCutout()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider == colliders[i])
            {
/*                SpriteMask spriteMask = colliders[i].GetComponent<SpriteMask>();
                spriteMask.enabled = true;

                Vector3 posStart = Camera.main.WorldToScreenPoint(new Vector3(colliders[i].bounds.min.x, colliders[i].bounds.min.y, colliders[i].bounds.min.z));
                Vector3 posEnd = Camera.main.WorldToScreenPoint(new Vector3(colliders[i].bounds.max.x, colliders[i].bounds.max.y, colliders[i].bounds.min.z));

                int widthX = (int)(posEnd.x - posStart.x);
                int widthY = (int)(posEnd.y - posStart.y);

                Debug.Log($"{widthX}, {widthY}");

                Texture2D spriteTex = new Texture2D(widthX, widthY);
                spriteMask.sprite = Sprite.Create(spriteTex, new Rect(0, 0, spriteTex.width, spriteTex.height), Vector2.zero);*/
                colliders.Remove(colliders[i]);
                Debug.Log("Collider Removed");
                i--;
            }
        }
        if (colliders.Count < 1)
        {
            CutoutComplete();
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
