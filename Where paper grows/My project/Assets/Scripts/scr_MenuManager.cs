using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject photoCanvas;
    [SerializeField] private GameObject mainUI;

    public GameObject PopupPanel;
    public GameObject creditsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToGame()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount-1));
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
        SceneManager.LoadScene("GalleryTemplate", LoadSceneMode.Additive);
    }

    public void EnablePhoto()
    {
        photoCanvas.SetActive(true);
        mainUI.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}