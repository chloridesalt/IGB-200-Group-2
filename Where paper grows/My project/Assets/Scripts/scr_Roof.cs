using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class scr_Roof : MonoBehaviour
{
    [SerializeField] private Texture2D rootTexture;
    [SerializeField] private Texture2D holeTexture;

    [SerializeField] private List<Texture2D> holeTextures = new List<Texture2D>();

    [SerializeField] private float holeScale = 1.0f;

    private Texture2D newRoofTexture;
    private Texture2D newHoleTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Copy roof texture onto a new texture
        newRoofTexture = new Texture2D(rootTexture.width, rootTexture.height);
        newRoofTexture.SetPixels(rootTexture.GetPixels());

        // Apply the new texture back onto the material
        GetComponent<MeshRenderer>().material.mainTexture = newRoofTexture;

        // Copy hole texture onto a new texture
        newHoleTexture = new Texture2D(holeTexture.width, holeTexture.height);
        newHoleTexture.SetPixels(holeTexture.GetPixels());

        // Here will be a segment to rescale the image to the holeScale

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

        Texture2D cutTexture = new Texture2D(newHoleTexture.width, newHoleTexture.height);
        Debug.Log($"{cutTexture.width}, {cutTexture.height}");

        for (int y=startY-halfHoleY; y<startY+halfHoleY; y++)
        {
            for (int x=startX-halfHoleX; x<startX+halfHoleX; x++)
            {
                int holePosX = x - startX + halfHoleX;
                int holePosY = y - startY + halfHoleY;

                Color holePixel = newHoleTexture.GetPixel(holePosX, holePosY);
                Color roofPixel = newRoofTexture.GetPixel(x, y);

                if (holePixel.a < 0.5f)
                {
                    cutTexture.SetPixel(holePosX, holePosY, roofPixel);
                    newRoofTexture.SetPixel(x, y, new Color(holePixel.r, holePixel.g, holePixel.b, holePixel.a));
                }
                else
                {
                    cutTexture.SetPixel(holePosX, holePosY , new Color(0,0,0,0));
                }
            }
        }
        cutTexture.Apply();
        holeTextures.Add(cutTexture);
        newRoofTexture.Apply();
    }
}
