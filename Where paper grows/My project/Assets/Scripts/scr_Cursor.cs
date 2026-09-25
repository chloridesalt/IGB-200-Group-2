using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class src_Cursor : MonoBehaviour
{
    private Image cursorImage;
    private bool isMinigameActive = false;

    // Global events that any script can trigger at any time
    public static System.Action OnMinigameStarted;
    public static System.Action OnMinigameEnded;
    [SerializeField] private float hideDelayDuration = 3f;
    private float mouseStillTimer = 0f;


    void Awake()
    {
        cursorImage = GetComponent<Image>();
        DeactivateCursor();

        // Subscribe to the global start and end events
        OnMinigameStarted += ActivateCursor;
        OnMinigameEnded += DeactivateCursor;
    }

    void OnDestroy()
    {

        OnMinigameStarted -= ActivateCursor;
        OnMinigameEnded -= DeactivateCursor;
    }

    private void ActivateCursor()
    {
        isMinigameActive = true;
        Cursor.visible = false;
    }

    private void DeactivateCursor()
    {
        isMinigameActive = false;
        cursorImage.enabled = false;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!isMinigameActive) return;

        // Touch input tracking logic
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                cursorImage.enabled = true;
                transform.position = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                cursorImage.enabled = false;
            }
        }
        else if (Input.mousePresent)
        {
            // Check if the mouse cursor is actively moving or clicking
            bool isMouseActive = Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetMouseButton(0);

            if (isMouseActive)
            {
                // Reset the timer and keep the cursor visible
                mouseStillTimer = 0f;
                cursorImage.enabled = true;
                transform.position = Input.mousePosition;
            }
            else
            {
                // If the mouse has stopped, start counting up
                mouseStillTimer += Time.deltaTime;

                // Only turn off the image once the timer passes our specified delay duration
                if (mouseStillTimer >= hideDelayDuration)
                {
                    cursorImage.enabled = false;
                }
            }
        }
    }
}
