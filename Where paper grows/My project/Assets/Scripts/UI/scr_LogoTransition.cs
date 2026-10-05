using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoTransition : MonoBehaviour
{
    [SerializeField] private RectTransform logo;

    public void PlayTransition()
    {
        gameObject.SetActive(true);

        // logo starts small
        logo.localScale = Vector3.one * 0.5f;

        StartCoroutine(ScaleLogo());
    }

    private IEnumerator ScaleLogo()
    {
        float duration = 0.75f;
        float timer = 0f;

        Vector3 startScale = Vector3.one * 1f;
        Vector3 endScale = Vector3.one * 10;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(timer / duration);

            // Scale smooth
            progress = Mathf.SmoothStep(0f, 1f, progress);

            logo.localScale = Vector3.Lerp(
                startScale,
                endScale,
                progress
            );

            yield return null;
        }

        logo.localScale = endScale;

        SceneManager.LoadScene("MainScene");
    }
}