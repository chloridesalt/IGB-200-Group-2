using NUnit.Framework;
using UnityEngine;

public class scr_PhotoHandler : MonoBehaviour
{
    [SerializeField] private Camera photoCamera;
    [SerializeField] private MenuManager menuManager;

    public Texture2D[] photos = new Texture2D[30];

    private int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakePhoto()
    {
        if (index > photos.Length)
        {
            return;
        }
        RenderTexture screenRender = new RenderTexture(Screen.width, Screen.height, 1000);
        photoCamera.targetTexture = screenRender;
        RenderTexture.active = screenRender;
        photoCamera.Render();

        Texture2D photo =  new Texture2D(Screen.width,Screen.height);
        photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height),0,0);
        photo.Apply();
        RenderTexture.active = null;

        photos[index] = photo;
        index++;

        menuManager.ClosePhoto();
    }
}
