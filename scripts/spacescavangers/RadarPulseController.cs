using System.Collections.Generic;
using UnityEngine;

public class RadarPulseController : MonoBehaviour
{
    public static RadarPulseController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform radarUI;
    [SerializeField] private float radarUIRadius = 100f;
    [SerializeField] private RadarBlipPool blipPool;

    [Header("Radar Settings")]
    [SerializeField] private float radarRange = 25f;
    [SerializeField, Min(0f)] private float updateInterval = 0.25f;

    [Header("Cosmetic Pulse")]
    [SerializeField] private RectTransform pulseCircle;
    [SerializeField] private float pulseMaxScale = 1f;
    [SerializeField, Min(0.01f)] private float pulseDuration = 1f;

    [Header("UI")]
    [SerializeField] private RectTransform directionArrow;

    private float updateTimer;
    private float pulseTimer;

    private readonly List<GameObject> trackedObjects = new();
    private readonly List<GameObject> removeBuffer = new();

    private RectTransform cachedRadarUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cachedRadarUI = radarUI;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        TryResolvePlayerAndRange();
        if (player == null)
            return;

        UpdatePulse();
        UpdateDirectionArrow();

        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            DetectObjects();
        }
    }

    private void TryResolvePlayerAndRange()
    {
        if (player == null && PlayerController.Instance != null)
            player = PlayerController.Instance.transform;

        if (GameManager.Instance != null)
            radarRange = GameManager.Instance.occlusionRadius;
    }

    private void UpdateDirectionArrow()
    {
        if (directionArrow == null || player == null)
            return;

        Vector2 up = player.up;
        float angle = Mathf.Atan2(up.y, up.x) * Mathf.Rad2Deg;
        directionArrow.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void RegisterRadarObject(GameObject obj)
    {
        if (obj == null) return;
        if (!trackedObjects.Contains(obj))
            trackedObjects.Add(obj);
    }

    public void UnregisterRadarObject(GameObject obj)
    {
        if (obj == null) return;
        trackedObjects.Remove(obj);
    }

    private void DetectObjects()
    {
        if (trackedObjects.Count == 0 || player == null)
            return;

        removeBuffer.Clear();

        Vector2 playerPos = player.position;

        for (int i = 0; i < trackedObjects.Count; i++)
        {
            GameObject obj = trackedObjects[i];
            if (obj == null)
            {
                removeBuffer.Add(obj);
                continue;
            }

            if (!obj.TryGetComponent(out RadarObjectInfo info))
                continue;

            Vector2 objPos = obj.transform.position;

            float distance = Vector2.Distance(objPos, playerPos);
            float compareDist = distance - info.DetectionRadius;

            if (compareDist <= radarRange)
            {
                SpawnBlip(objPos, info.Type, distance);
            }
        }

        for (int i = 0; i < removeBuffer.Count; i++)
            trackedObjects.Remove(removeBuffer[i]);

        removeBuffer.Clear();
    }

    private void SpawnBlip(Vector2 worldPos, RadarContactType type, float distance)
    {
        if (blipPool == null || player == null)
            return;

        RadarBlip blip = blipPool.GetBlip();
        if (blip == null)
            return;

        blip.Initialize(blipPool);

        Vector2 dir = worldPos - (Vector2)player.position;
        Vector2 normalizedDir = dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector2.zero;

        float normalizedDist = (radarRange <= 0.0001f) ? 0f : Mathf.Clamp01(distance / radarRange);
        Vector2 uiPos = normalizedDir * normalizedDist * radarUIRadius;

        RectTransform rt = blip.GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = uiPos;

        blip.Activate(GetColorForType(type));
    }

    private void UpdatePulse()
    {
        if (pulseCircle == null || pulseDuration <= 0f)
            return;

        pulseTimer += Time.deltaTime;

        float t = pulseTimer / pulseDuration;
        if (t >= 1f)
        {
            pulseTimer = 0f;
            t = 0f;
        }

        float scale = Mathf.Lerp(0f, pulseMaxScale, t);
        pulseCircle.localScale = Vector3.one * scale;
    }

    private static Color GetColorForType(RadarContactType type)
    {
        return type switch
        {
            RadarContactType.Resource => Color.cyan,
            RadarContactType.Wreck => new Color(1f, 0.5f, 0f),
            RadarContactType.Friendly => Color.blue,
            RadarContactType.Hostile => Color.red,
            RadarContactType.Hazard => Color.yellow,
            _ => Color.white
        };
    }
}
