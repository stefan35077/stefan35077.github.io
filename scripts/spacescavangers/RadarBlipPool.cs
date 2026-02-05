using System.Collections.Generic;
using UnityEngine;

public class RadarBlipPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject blipPrefab;
    [SerializeField] private Transform blipContainer;
    [SerializeField] private int poolSize = 100;

    private readonly Queue<RadarBlip> pool = new Queue<RadarBlip>();

    private void Awake()
    {
        PrewarmPool();
    }

    private void PrewarmPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateAndStoreBlip();
        }
    }

    private RadarBlip CreateAndStoreBlip()
    {
        GameObject obj = Instantiate(blipPrefab, blipContainer);
        obj.SetActive(false);

        RadarBlip blip = obj.GetComponent<RadarBlip>();
        pool.Enqueue(blip);

        return blip;
    }

    public RadarBlip GetBlip()
    {
        RadarBlip blip = pool.Count > 0
            ? pool.Dequeue()
            : CreateAndStoreBlip();

        blip.gameObject.SetActive(true);
        return blip;
    }

    public void ReturnBlip(RadarBlip blip)
    {
        if (blip == null)
            return;

        blip.transform.SetParent(blipContainer);
        blip.gameObject.SetActive(false);
        pool.Enqueue(blip);
    }
}
