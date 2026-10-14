using UnityEngine;
using UnityEngine.InputSystem;

public class scr_InputManager : MonoBehaviour
{
    public InputAction lookAction { get; private set; }
    public InputAction interactAction { get; private set; }
    public Vector2 lookValue { get; private set; }
    public bool AFK { get; set; } = false;
    public float AFKTime = 5f;
    private float AFKTimer = 0f;
    private float Alpha = 0f;

    private void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        lookValue = lookAction.ReadValue<Vector2>();
        CheckAFK();
    }

    public bool IsHitSolid(RaycastHit hit)
    {
        Renderer renderer = hit.collider.GetComponent<Renderer>();
        if (renderer == null || renderer.sharedMaterial == null) return true;

        Material material = renderer.sharedMaterial;
        if (material == null) return true;

        Texture texture = material.mainTexture;
        if (texture == null) return true;

        if (material.color.a <= Alpha)
        {
            return false;
        }

        if (texture is not Texture2D tex)
        {
            return true;
        }

        Vector2 pixelUV = hit.textureCoord;
        if (pixelUV.x < 0f || pixelUV.x > 1f || pixelUV.y < 0f || pixelUV.y > 1f)
        {
            return true;
        }

        Color pixelColor = tex.GetPixelBilinear(Mathf.Clamp01(pixelUV.x), Mathf.Clamp01(pixelUV.y));
        return (pixelColor.a * material.color.a) > Alpha;
    }

    private void CheckAFK()
    {
        if (interactAction.triggered)
        {
            AFK = false;
            AFKTimer = 0f;
        }
        AFKTimer += Time.deltaTime;
        if (AFKTimer >= AFKTime)
        {
            AFK = true;
        }

    }
}
