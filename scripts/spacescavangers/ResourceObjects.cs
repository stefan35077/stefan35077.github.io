using UnityEngine;

[CreateAssetMenu(fileName = "ResourceObjects", menuName = "Scriptable Objects/newResource")]
public class ResourceObjects : ScriptableObject
{
    public float MaxResourceWeight;
    public float MinResourceWeight;
    public int Rarity;
    public string ResourceName;
    public GameObject ResourcePrefab;
    public Sprite ResourceIcon;
}
