using UnityEngine;
using UnityEngine.UI;

public class scr_Tray : MonoBehaviour
{

    [SerializeField] private GameObject Tray;
    [SerializeField] private scr_PlaceObject ObjectScript;
    [SerializeField] private GameObject DraggableObject;
    [SerializeField] private Sprite[] ObjectImages;
    private string ObjectName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetObjectSprite()
    {
        ObjectName = ObjectScript.ObjectToPlace.name;
        switch (ObjectName)
        {
            case "pre_Tree":
                DraggableObject.GetComponent<Image>().sprite = ObjectImages[0];
                DraggableObject.transform.localScale = new Vector3(1, 1.5f, 1);
                break;
            case "pre_Bush":
                DraggableObject.GetComponent<Image>().sprite = ObjectImages[1];
                DraggableObject.transform.localScale = new Vector3(2, 1, 1);
                break;
            case "pre_Flower":
                DraggableObject.GetComponent<Image>().sprite = ObjectImages[2];
                DraggableObject.transform.localScale = new Vector3(1, 2, 1);
                break;
        }
    }
}
