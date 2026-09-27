using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject exampleGalleryPanel;
    [SerializeField] private AudioData view;

    public GameObject photoCanvas;
    public GameObject PopupPanel;
    public GameObject creditsPanel;

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

    public void OpenPanel()
    {
        PopupPanel.SetActive(true);
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