using UnityEngine;

public class RadarObjectInfo : MonoBehaviour
{
    public RadarContactType Type = RadarContactType.Resource;
    public float DetectionRadius = 0;
}

public enum RadarContactType
{
    Resource,
    Wreck,
    Friendly,
    Hostile,
    Hazard
}
