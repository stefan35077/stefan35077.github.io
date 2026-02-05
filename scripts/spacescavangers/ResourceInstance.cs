using Unity.VisualScripting;
using UnityEngine;

public class ResourceInstance : MonoBehaviour
{
    private Rigidbody2D rb;

    public int Weight { get; private set; }
    public int Rarity { get; private set; }
    public string Name { get; private set; }

    public Sprite Icon { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetWeight(int weight)
    {
        Weight = weight;
        rb.mass = weight;
    }
    public void SetRarity(int rarity)
    {
        Rarity = rarity;
    }
    public void SetName(string name)
    {
        Name = name;
    }
    public void SetIcon(Sprite icon)
    {
        Icon = icon;
    }
}
