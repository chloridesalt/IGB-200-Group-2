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
            RayCast();
            //MergeTextures(250, 250);
        }
    }

    private void RayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 1000f;

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 5f);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Vector2 texturePos = hit.textureCoord;
            MergeTextures((int)texturePos.x, (int)texturePos.y);
            return;
        }

        float planeY = 0f;
        var cols = FindObjectsOfType<Collider>();
        foreach (var c in cols)
        {
            if (c == null) continue;
            if (c.gameObject.CompareTag("Roof") || c.gameObject.name.ToLower().Contains("roof"))
            {
                planeY = c.transform.position.y;
                break;
            }
        }

        Plane groundPlane = new Plane(Vector3.forward, new Vector3(0, planeY, 0));
        float enter;
        if (groundPlane.Raycast(ray, out enter))
        {
            /*Vector3 planePoint = ray.Get;
            Vector2 texturePos = 
            MergeTextures((int)texturePos.x, (int)texturePos.y);*/
            return;
        }
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
