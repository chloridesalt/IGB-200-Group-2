using UnityEngine;
using DG.Tweening;

public class scr_EnvironmentalObject : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private Vector3 punchScale = new Vector3(0.3f, 0.3f, 0.3f); // How much larger it grows
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float elasticity = 1f;
    [SerializeField] private int vibrato = 10;
    public ParticleSystem Event;
    public string objectName;
    private Vector3 originalScale;

    [SerializeField] private AudioData clickSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectName = gameObject.name;
    }
    private void Awake()
    {
        originalScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsClicked()
    {
        transform.DOKill();
        transform.localScale = originalScale;

        // Apply punch/pop effect
        transform.DOPunchScale(punchScale, duration, vibrato, elasticity);
        AudioManager.Instance.PlayAtPosition(clickSound, transform.position);
        Event.Play();
        Debug.Log($"{objectName} Clicked");
    }
}
