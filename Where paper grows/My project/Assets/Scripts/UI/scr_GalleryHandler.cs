using NUnit.Framework;
using OrbitalNine.QRCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class scr_GalleryHandler : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject openedImage;
    [SerializeField] private GameObject qrCode;
    [SerializeField] private GameObject qrExit;
    [SerializeField] private GameObject imageHolder;
    [SerializeField] private AudioData pageFlip;

    public Texture2D[] images;
    public RawImage[] imageHolders;
    public scr_PhotoHandler photoHandler;

    private int index = 0;
    private int page = 1;

    private string downloadString;

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
        AudioManager.Instance.PlayAtPosition(pageFlip, transform.position);
        index -= 6;
        page--;
        UpdateImages();
    }

    public void SwitchPageForward()
    {
        AudioManager.Instance.PlayAtPosition(pageFlip, transform.position);
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
        OpenQR();
        StartCoroutine(PostImage(((Texture2D)image.texture).EncodeToPNG()));
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

    public void CloseQR()
    {
        qrExit.SetActive(false);
        qrCode.SetActive(false);
        imageHolder.SetActive(true);
    }

    private void OpenQR()
    {
        qrCode.SetActive(true);
        qrCode.GetComponentInChildren<QRCodeGenerator>().gameObject.GetComponent<RawImage>().texture = null;
        imageHolder.SetActive(false);
    }

    IEnumerator PostImage(byte[] image)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", image);

        using (UnityWebRequest www = UnityWebRequest.Post("https://tmper.co/?time=5mins&n=1", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                downloadString = www.downloadHandler.text;
                Debug.Log(downloadString);
                SetupQR();
            }
        }
    }

    private void SetupQR()
    {
        string textToEncode = $"https://tmper.co/{downloadString}";
        Debug.Log(textToEncode);
        qrCode.GetComponentInChildren<QRCodeGenerator>().GenerateQRCode(textToEncode);
        qrExit.SetActive(true);
    }
}
