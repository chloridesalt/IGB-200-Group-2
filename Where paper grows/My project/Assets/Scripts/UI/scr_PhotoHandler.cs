using NUnit.Framework;
using UnityEngine;

public class scr_PhotoHandler : MonoBehaviour
{
    [SerializeField] private Camera photoCamera;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private AudioData cameraClick;

    public Texture2D[] photos = new Texture2D[30];
    public int newPhoto = 4;
    public bool cameFromPhoto;
    private int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < photos.Length; i++)
        {
            if (photos[i] == null)
            {
                index = i;
                break;
            }
        }
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
        AudioManager.Instance.PlayAtPosition(cameraClick, transform.position);
        RenderTexture screenRender = new RenderTexture(Screen.width, Screen.height, 1000);
        photoCamera.targetTexture = screenRender;
        RenderTexture.active = screenRender;
        photoCamera.Render();

        Texture2D photo =  new Texture2D(Screen.width,Screen.height);
        photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height),0,0);
        photo.Apply();
        RenderTexture.active = null;

        photos[index] = photo;
        newPhoto = index;
        cameFromPhoto = true;
        index++;

        menuManager.ClosePhoto();
        menuManager.GoToGalleryfromPhoto();
    }
}
