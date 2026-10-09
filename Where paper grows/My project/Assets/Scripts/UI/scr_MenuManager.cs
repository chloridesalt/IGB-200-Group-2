using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject exampleGalleryPanel;
    [SerializeField] private AudioData view;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider volumeSlider;
    public GameObject photoCanvas;
    public GameObject PopupPanel;
    public GameObject creditsPanel;
    public GameObject brief;
    public GameObject attributes;
    private const string MIXER_PARAMETER = "masterMixer";
    public Image Mainvolume;
    public GameObject Volume1;
    public GameObject Volume11;
    public GameObject Volume2;
    public GameObject Volume22;

    private Image volume1Image;
    private Image volume11Image;

    private Image volume2Image;
    private Image volume22Image;

    public Sprite Maxvolume;
    public Sprite HalfVolume;
    public Sprite VolumeOff;
    private void Start()
    {
        volume1Image = Volume1.GetComponent<Image>();
        volume11Image = Volume11.GetComponent<Image>();

        volume2Image = Volume2.GetComponent<Image>();
        volume22Image = Volume2.GetComponent<Image>();


    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToGame()
    {
        AudioManager.Instance.PlayAtPosition(view, transform.position);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void ShowAttributes()
    {
        attributes.SetActive(true);
        brief.SetActive(false);
    }

    public void ShowBrief()
    {
        attributes.SetActive(false);
        brief.SetActive(true);
    }

    public void OpenPanel()
    {
        PopupPanel.SetActive(!PopupPanel.activeSelf);
    }

    public void VoumeFull()
    {
        Mainvolume.sprite = Maxvolume;

        

        Volume2.SetActive(true);
        Volume11.SetActive(false);
        
        float decibelValue = Mathf.Log10(0.5f) * 20;
        masterMixer.SetFloat(MIXER_PARAMETER, decibelValue);
    }
    public void Volumehalf()
    {
        Mainvolume.sprite = HalfVolume;
        Volume1.SetActive(true);
       // Volume11.SetActive(true);
       
        Volume2.SetActive(false);
        //Volume22.SetActive(true);
        masterMixer.SetFloat(MIXER_PARAMETER, -80f);
    }
    public void VoumeOff()
    {
        Mainvolume.sprite = VolumeOff;
        Volume1.SetActive(false);
        Volume11.SetActive(true);
       
       // Volume2.SetActive(true);
        //Volume22.SetActive(false);
       
        masterMixer.SetFloat(MIXER_PARAMETER, 0f);
    }
    public void ClosePanel()
    {
        PopupPanel.SetActive(false);
    }

    public void GoToGallery()
    {
        AudioManager.Instance.PlayAtPosition(view, transform.position);
        SceneManager.LoadScene("GalleryTemplate", LoadSceneMode.Additive); 
    }

    public void EnablePhoto()
    {
        AudioManager.Instance.PlayAtPosition(view, transform.position);
        photoCanvas.SetActive(true);
        mainUI.SetActive(false);
    }

    public void ClosePhoto()
    {
        AudioManager.Instance.PlayAtPosition(view, transform.position);
        photoCanvas.SetActive(false);
        mainUI.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenExampleGallery()
    {
        exampleGalleryPanel.SetActive(true);
    }

    public void CloseExampleGallery()
    {
        exampleGalleryPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}