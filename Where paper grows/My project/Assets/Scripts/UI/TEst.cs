using UnityEngine;
using DG.Tweening;
using static UnityEditor.Rendering.FilterWindow;

public class Popin : MonoBehaviour
{
    [Header("Assign separate UI RectTransforms here")]
    [SerializeField] private RectTransform[] uiElements;

    [Header("Animation Settings")]
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private float staggerDelay = 0.08f;

    private Vector3[] originalScales;

    private void Awake()
    {
        // Store initial scales set in the Inspector
        originalScales = new Vector3[uiElements.Length];
        for (int i = 0; i < uiElements.Length; i++)
        {
            if (uiElements[i] != null)
            {
                originalScales[i] = uiElements[i].localScale;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(PopInAllWithDelay());
    }

    private System.Collections.IEnumerator PopInAllWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        PopInAll();
    }

    public void PopInAll()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            RectTransform element = uiElements[i];
            if (element == null) continue;

            element.DOKill();

            // Set to zero before animating
            element.localScale = Vector3.zero;
            element.gameObject.SetActive(true);

            GameManager.s_Instance.UI.GetComponent<scr_UIHandler>().RoofViewButtonOn = true;

            float delay = i * staggerDelay;

            // Animate back to its specific original scale
            element.DOScale(originalScales[i], duration)
                .SetEase(Ease.OutBack)
                .SetDelay(delay)
                .SetUpdate(true);
        }
    }
}
