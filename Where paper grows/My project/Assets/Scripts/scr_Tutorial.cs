using UnityEngine;

public class scr_Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject UITutorial;
    [SerializeField] public GameObject FirstTearTutorial;
    [SerializeField] private GameObject AFKTearTutorial;
    [SerializeField] private GameObject CutoutTutorial;
    [SerializeField] public GameObject IntroTutorial;
    [SerializeField] private GameObject PlaceObjectTutorial;
    [SerializeField] private GameObject EditObjectTutorial;
    [SerializeField] private float startDelay = 0.5f;
    private bool IntroTutorialShown = false;
    private bool FirstTearTutorialShown = false;
    private bool CutoutTutorialShown = false;
    private bool PlaceObjectTutorialShown = false;
    private bool EditObjectTutorialShown = false;
    public bool ButtonTutorialShown = false;
    private bool AFKTutorialActive = false;
    public bool TutorialCompleted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShowWithDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private System.Collections.IEnumerator ShowWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        ShowRoofViewTutorial();
    }

    // all of these go show/hide in order of appearance
    public void ShowRoofViewTutorial() //triggered at start, asks to press roof view button
    {
        if (!IntroTutorialShown)
        {
            IntroTutorial.SetActive(true);
            IntroTutorialShown = true;
        }
    }

    public void HideRoofViewTutorial() //triggered when roof view button is pressed
    {
        if (IntroTutorialShown)
        {
            IntroTutorial.SetActive(false);
        }
    }

    public void ShowFirstTearTutorial() //triggered when roof view button is pressed, asks to tear a hole in the roof
    {
        if (!FirstTearTutorialShown)
        {
            FirstTearTutorial.SetActive(true);
            GameManager.s_Instance.UITransitionTop();
            GameManager.s_Instance.UITransitionBottomReset();
            FirstTearTutorialShown = true;
        }
    }

    public void HideFirstTearTutorial() //triggered when a tear is made
    {
        if (FirstTearTutorialShown)
        {
            FirstTearTutorial.SetActive(false);
        }
    }

    public void ShowCutoutTutorial() //triggered when choice is made, asks to cut out a shape
    {
        if (!CutoutTutorialShown)
        {
            CutoutTutorial.SetActive(true);
            CutoutTutorialShown = true;
        }
    }
    public void HideCutoutTutorial() //triggered when a cutout is made
    {
        if (CutoutTutorialShown)
        {
            CutoutTutorial.SetActive(false);
        }
    }

    public void ShowPlaceObjectTutorial() //triggered when a cutout is made, asks to place an object
    {
        if (!PlaceObjectTutorialShown)
        {
            PlaceObjectTutorial.SetActive(true);
            PlaceObjectTutorialShown = true;
        }
    }

    public void HidePlaceObjectTutorial() //triggered when an object is placed
    {
        if (PlaceObjectTutorialShown)
        {
            PlaceObjectTutorial.SetActive(false);
        }
    }

    public void ShowEditObjectTutorial() //triggered when an object is placed, asks to edit the object
    {
        if (!EditObjectTutorialShown)
        {
            EditObjectTutorial.SetActive(true);
            EditObjectTutorialShown = true;
        }
    }
    public void HideEditObjectTutorial() //triggered when an object is finalized
    {
        if (EditObjectTutorialShown)
        {
            EditObjectTutorial.SetActive(false);
        }
    }
    public void ShowButtonTutorial() //triggered when an object is placed, informs about photo and gallery
    {
        if (!ButtonTutorialShown)
        {
            UITutorial.SetActive(true);
            ButtonTutorialShown = true;
        }
    }
    public void HideButtonTutorial() //triggered when the button tutorial is clicked or...
    {
        if (ButtonTutorialShown)
        {
            UITutorial.SetActive(false);
            TutorialCompleted = true;
        }
    }

    public void ShowAFKTutorial() //triggered when the player is AFK for a certain amount of time and in the roof view, informs about tearing a hole in the roof
    {
        if (AFKTutorialActive == false && TutorialCompleted == true)
        {
            AFKTearTutorial.SetActive(true);
            AFKTutorialActive = true;
        }
    }

    public void HideAFKTutorial() //triggered when the player is no longer AFK or not in the roof view, hides the AFK tutorial
    {
        if (AFKTutorialActive)
        {
            AFKTearTutorial.SetActive(false);
            AFKTutorialActive = false;
        }
    }
}
