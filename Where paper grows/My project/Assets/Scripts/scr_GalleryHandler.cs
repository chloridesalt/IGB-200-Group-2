using UnityEngine;
using UnityEngine.UI;

public class scr_GalleryHandler : MonoBehaviour
{
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private GameObject backButton;

    public Sprite[] images;
    public Image[] imageHolders;

    private int index = 0;
    private int page = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    private void UpdateImages()
    {
        // Checks if there's more images than can be placed on the page
        if (images.Length - 1 > index + 5)
        {
            forwardButton.SetActive(true);

            // Sets the images
            for (int i = index; i < index + 6; i++)
            {
                imageHolders[i / page].sprite = images[i];
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
                imageHolders[i].sprite = images[index + i];
                imageHolders[i].color = Color.white;
            }

            // Turns the rest of the image holders transparent
            for (i=i; i < 6; i++)
            {
                imageHolders[i].sprite = null;
                imageHolders[i].color = new Color(1,1,1,0);
            }
        }

        // Enables back button if past page 1
        if (page > 1) backButton.SetActive(true);
        else backButton.SetActive(false);
    }
}
