using Unity.VisualScripting;
using UnityEngine;

public class scr_Roof : MonoBehaviour
{
    [SerializeField] private Texture2D rootTexture;
    [SerializeField] private Texture2D holeTexture;

    [SerializeField] private float holeScale = 1.0f;

    private Texture2D newRoofTexture;
    private Texture2D newHoleTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newRoofTexture = new Texture2D(rootTexture.width, rootTexture.height);
        newRoofTexture.SetPixels(rootTexture.GetPixels());
        GetComponent<MeshRenderer>().material.mainTexture = newRoofTexture;

        newHoleTexture = new Texture2D(holeTexture.width, holeTexture.height);
        newHoleTexture.SetPixels(holeTexture.GetPixels());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //RayCast();
            MergeTextures(250, 250);
        }
    }

    private void RayCast()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        Debug.Log(hit.collider);
    }

    private void MergeTextures(int startX, int startY)
    {
        int halfHoleX = newHoleTexture.width / 2;
        int halfHoleY = newHoleTexture.height / 2;

        for (int y=startY-halfHoleY; y<startY+halfHoleY; y++)
        {
            for (int x=startX-halfHoleX; x<startX+halfHoleX; x++)
            {
                Color pixel = newHoleTexture.GetPixel(x - startX + halfHoleX, y - startY + halfHoleY);
                if (pixel.a < 0.5f)
                {
                    newRoofTexture.SetPixel(x, y, new Color(0,0,0,pixel.a));
                }
            }
        }
        newRoofTexture.Apply();
    }
}
