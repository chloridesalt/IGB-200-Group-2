using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class scr_UIJUICE : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] public float hoverScale = 1.1f;
    [SerializeField] public float animationDuration = 0.2f;
    [SerializeField] public Ease easeType = Ease.OutSine;

    private Vector3 originalScale;
    private bool isInitialized = false;

    // Shifted from Awake to Start to let Layout Groups calculate first
    void Start()
    {
        InitializeScale();
    }

    void OnEnable()
    {
        
        InitializeScale();
    }

    private void InitializeScale()
    {
        if (isInitialized) return;

   
        if (transform.localScale != Vector3.zero)
        {
            originalScale = transform.localScale;
            isInitialized = true;
        }
        else
        {
           
            originalScale = Vector3.one;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Double-check initialization state
        if (!isInitialized) InitializeScale();

       
        transform.DOScale(originalScale * hoverScale, animationDuration).SetEase(easeType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInitialized) InitializeScale();

       
        transform.DOScale(originalScale, animationDuration).SetEase(easeType);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
