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
        if (Input.GetMouseButtonDown(0) && GameManager.s_Instance != null && GameManager.s_Instance.EnableTear)
        {
            RayCast();
            //MergeTextures(250, 250);
            GameManager.s_Instance.EnableTear = false;
            GameManager.s_Instance.UI.GetComponent<scr_UIHandler>().ChoiceContainer.SetActive(true);
        }
    }

    private void RayCast()
    {
        // Draw a raycast from the place clicked on the screen forward
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        // If the object hit is not null and is this game object
        if (hit.collider != null && hit.collider.gameObject == this.gameObject)
        {
            // Find the texture coordinates that the raycast hits and multiply it by the roof texture size to get the true pixels
            Vector2 textureCoords = hit.textureCoord;
            textureCoords.x *= newRoofTexture.width;
            textureCoords.y *= newRoofTexture.height;

            // Create the hole in the texture
            MergeTextures((int)textureCoords.x, (int)textureCoords.y);

            // Attempt at inversing the texture removal for 3D objects to make the hole appear on the underside
            /*textureCoords = hit.textureCoord;
            textureCoords.x = (1 - textureCoords.x) * newRoofTexture.width;
            textureCoords.y = (1 - textureCoords.y) * newRoofTexture.height;
            MergeTextures((int)textureCoords.x, (int)textureCoords.y);*/
        }
    }
    private void MergeTextures(int startX, int startY)
    {
        // Get half the hole size to allow centering the hole on the cursor
        int halfHoleX = newHoleTexture.width / 2;
        int halfHoleY = newHoleTexture.height / 2;

        // Creates a temporary texture storing the texture being removed
        Texture2D cutTexture = new Texture2D(newHoleTexture.width, newHoleTexture.height);

        // For each pixel in the y coordinate of the hole
        for (int y=startY-halfHoleY; y<startY+halfHoleY; y++)
        {
            // Skips if it goes over the edge of the sprite
            if (y < 0 || y > newRoofTexture.height) continue;

            // For each pixel in the x coordinate of the hole
            for (int x=startX-halfHoleX; x<startX+halfHoleX; x++)
            {
                // Skips if it goes over the edge of the sprite
                if (x < 0 || x > newRoofTexture.width) continue;

                // Get the position of this pixel of the hole
                int holePosX = x - startX + halfHoleX;
                int holePosY = y - startY + halfHoleY;
                
                // Get the pixels on both the hole texture and roof texture
                Color holePixel = newHoleTexture.GetPixel(holePosX, holePosY);
                Color roofPixel = newRoofTexture.GetPixel(x, y);

                // Checks if the hole pixel is below a certain transparency
                if (holePixel.a < 0.5f)
                {
                    // Adds the roof pixel to the temporary cutout texture
                    cutTexture.SetPixel(holePosX, holePosY, roofPixel);

                    // Sets the roof pixel's opacity to the hole pixel's transparency
                    newRoofTexture.SetPixel(x, y, new Color(roofPixel.r, roofPixel.g, roofPixel.b, holePixel.a));
                }
                else
                {
                    // Makes the pixel on the temporary cutout texture transparent
                    cutTexture.SetPixel(holePosX, holePosY , new Color(0,0,0,0));
                }
            }
        }
        // Confirms the temporary cutout texture and adds it to a list
        cutTexture.Apply();
        holeTextures.Add(cutTexture);

        // Confirms and applies the hole to the roof texture render
        newRoofTexture.Apply();
    }
}
