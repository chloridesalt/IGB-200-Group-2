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
        if (images.Length < index + 5)
        {
            for (int i = index; i < images.Length; i++)
            {
                Debug.Log(i);
                imageHolders[i/page].sprite = images[i];
                imageHolders[i/page].color = Color.white;
            }
        }
        else
        {
            for (int i = index; i < index+6; i++)
            {
                imageHolders[i/page].sprite = images[i];
                imageHolders[i/page].color = Color.white;
                imageHolders[i/page].color = Color.white;
            }
        }

        if (images.Length-1 > index + 5)
        {
            forwardButton.SetActive(true);
        }
    }
}
