using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeafTransition : MonoBehaviour
{
    [Header("Leaf Settings")]
    [SerializeField] private Sprite[] leafSprites;
    [SerializeField] private RectTransform leafContainer;
    [SerializeField] private float spawnInterval = 0.08f;
    [SerializeField] private int leafCount = 80;

    [Header("Leaf Movement")]
    [SerializeField] private float fallSpeedMin = 250f;
    [SerializeField] private float fallSpeedMax = 500f;
    [SerializeField] private float driftMin = 40f;
    [SerializeField] private float driftMax = 120f;
    [SerializeField] private float rotationMin = 60f;
    [SerializeField] private float rotationMax = 180f;
    [SerializeField] private float leafScaleMin = 0.6f;
    [SerializeField] private float leafScaleMax = 1.3f;


    private bool transitioning = false;

    public void PlayTransition()
    {
        if (transitioning)
            return;

        transitioning = true;
        StartCoroutine(LeafTransitionRoutine());
    }

    private IEnumerator LeafTransitionRoutine()
    {
        // Make sure the transition object is visible
        gameObject.SetActive(true);

        // Spawn leaves 
        for (int i = 0; i < leafCount; i++)
        {
            SpawnLeaf();

            yield return new WaitForSeconds(spawnInterval);
        }
        // Load game
        SceneManager.LoadScene("MainScene");
    }

    private void SpawnLeaf()
    {
        if (leafSprites.Length == 0)
            return;

        // Create the leaf object
        GameObject leaf = new GameObject("Leaf");

        leaf.transform.SetParent(leafContainer, false);

        Image image = leaf.AddComponent<Image>();

        // Randomly pick leaf
        image.sprite = leafSprites[Random.Range(0, leafSprites.Length)];

        image.preserveAspect = true;
        image.raycastTarget = false;

        RectTransform rect = leaf.GetComponent<RectTransform>();

        // STart random position
        float screenWidth = leafContainer.rect.width;
        float screenHeight = leafContainer.rect.height;

        float x = Random.Range(-screenWidth * 0.1f, screenWidth * 1.1f);
        float y = screenHeight + Random.Range(50f, 200f);

        rect.anchoredPosition = new Vector2(x, y);

        // Random leaf size
        float scale = Random.Range(leafScaleMin, leafScaleMax);
        rect.localScale = Vector3.one * scale;

        // Make rotation random
        rect.localRotation = Quaternion.Euler(
            0f,
            0f,
            Random.Range(0f, 360f)
        );

        // Start movement
        StartCoroutine(FallLeaf(
            rect,
            Random.Range(fallSpeedMin, fallSpeedMax),
            Random.Range(driftMin, driftMax) *
            (Random.value > 0.5f ? 1f : -1f),
            Random.Range(rotationMin, rotationMax) *
            (Random.value > 0.5f ? 1f : -1f)
        ));
    }

    private IEnumerator FallLeaf(
        RectTransform leaf,
        float fallSpeed,
        float drift,
        float rotationSpeed)
    {
        float driftOffset = Random.Range(0f, Mathf.PI * 2f);
        float time = 0f;

        while (leaf != null)
        {
            time += Time.deltaTime;

            Vector2 position = leaf.anchoredPosition;

            // Fall down
            position.y -= fallSpeed * Time.deltaTime;

            // Sideways movement
            position.x +=
                Mathf.Sin(time * 1.5f + driftOffset)
                * drift
                * Time.deltaTime;

            leaf.anchoredPosition = position;

            // Rotate while falling
            leaf.Rotate(
                0f,
                0f,
                rotationSpeed * Time.deltaTime
            );

            // Destroy the leaf after its gone
            if (position.y < -leafContainer.rect.height - 300f)
            {
                Destroy(leaf.gameObject);
                yield break;
            }

            yield return null;
        }
    }

}
