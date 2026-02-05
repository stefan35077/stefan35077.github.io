using UnityEngine;
using UnityEngine.UI;

public class RadarBlip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image blipImage;

    [Header("Fade")]
    [SerializeField, Min(0f)] private float fadeDuration = 0.5f;

    private float timer;
    private RadarBlipPool pool;

    public void Initialize(RadarBlipPool poolRef)
    {
        pool = poolRef;
    }

    public void Activate(Color color)
    {
        if (blipImage == null)
            return;

        timer = 0f;

        // Force full alpha
        color.a = 1f;
        blipImage.color = color;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (blipImage == null)
            return;

        timer += Time.deltaTime;

        // If duration is 0 instantly finish.
        float progress = (fadeDuration <= 0f) ? 1f : Mathf.Clamp01(timer / fadeDuration);
        float alpha = 1f - progress;

        Color c = blipImage.color;
        c.a = alpha;
        blipImage.color = c;

        if (progress >= 1f)
        {
            // Prefer pooling
            if (pool != null) pool.ReturnBlip(this);
            else gameObject.SetActive(false);
        }
    }
}
