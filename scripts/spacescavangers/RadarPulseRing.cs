using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RadarPulseRing : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float maxRadius = 5f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float startAlpha = 1f;

    private float timer;
    private Transform ringTransform;
    private SpriteRenderer ringRenderer;

    public float CurrentRadius =>
        Mathf.Lerp(0f, maxRadius, Mathf.Clamp01(timer / lifetime));

    private void Awake()
    {
        ringTransform = transform;
        ringRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float progress = Mathf.Clamp01(timer / lifetime);

        // Scale outward
        float radius = Mathf.Lerp(0f, maxRadius, progress);
        ringTransform.localScale = new Vector3(radius, radius, 1f);

        // Fade out
        Color color = ringRenderer.color;
        color.a = Mathf.Lerp(startAlpha, 0f, progress);
        ringRenderer.color = color;

        if (progress >= 1f)
            Destroy(gameObject);
    }
}
