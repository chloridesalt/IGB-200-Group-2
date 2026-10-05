using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OrbitalNine.QRCode;

public class scr_GalleryHandler : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject openedImage;

    public Texture2D[] images;
    public RawImage[] imageHolders;
    public scr_PhotoHandler photoHandler;

    private int index = 0;
    private int page = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        photoHandler = FindAnyObjectByType<scr_PhotoHandler>(FindObjectsInactive.Include);
        SetImages();
        UpdateImages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchPageBack()
    {
        index -= 6;
        page--;
        UpdateImages();
    }

    public void SwitchPageForward()
    {
        index += 6;
        page++;
        UpdateImages();
    }

    public void OpenImage(RawImage image)
    {
        openedImage.SetActive(true);
        openedImage.GetComponentInChildren<RawImage>().texture = image.texture;
    }

    public void CloseImage()
    {
        openedImage.SetActive(false);
    }

    public void DownloadImage(RawImage image)
    {
        /*string textToEncode = "https://unity.com/";
        openedImage.GetComponentInChildren<QRCodeGenerator>().GenerateQRCode(textToEncode);*/
    }

    private void SetImages()
    {
        images = new Texture2D[photoHandler.photos.Count(s => s != null)];
        for (int i = 0; i < images.Length; i++)
        {
            images[i] = photoHandler.photos[i];
        }   
    }

    private void UpdateImages()
    {
        // Checks if there's more images than can be placed on the page
        if (images.Length - 1 > index + 5)
        {
            forwardButton.SetActive(true);

            // Sets the images
            for (int i = index; i < index + 6; i++)
            {
                imageHolders[i / page].texture = images[i];
                imageHolders[i / page].color = Color.white;
            }
        }
        else
        {
            forwardButton.SetActive(false);

            int i = 0;

            // Sets as many image holders as there are images to the images
            for (i=i; i < images.Length - index; i++)
            {
                imageHolders[i].texture = images[index + i];
                imageHolders[i].color = Color.white;
            }

            // Turns the rest of the image holders transparent
            for (i=i; i < 6; i++)
            {
                imageHolders[i].texture = null;
                imageHolders[i].color = new Color(1,1,1,0);
            }
        }

        // Enables back button if past page 1
        if (page > 1) backButton.SetActive(true);
        else backButton.SetActive(false);
    }
}
