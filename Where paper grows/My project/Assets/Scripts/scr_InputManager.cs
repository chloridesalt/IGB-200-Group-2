using UnityEngine;
using UnityEngine.InputSystem;

public class scr_InputManager : MonoBehaviour
{
    public InputAction lookAction { get; private set; }
    public InputAction interactAction { get; private set; }
    public Vector2 lookValue { get; private set; }

    private void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        lookValue = lookAction.ReadValue<Vector2>();
    }
}
