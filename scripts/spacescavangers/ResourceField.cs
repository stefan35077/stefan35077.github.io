using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceField : MonoBehaviour
{
    [SerializeField] private List<ResourceObjects> availableResources;
    public float spawnRadius;
    [SerializeField] private int maxResources;

    public RadarObjectInfo radarInfo;
    [HideInInspector] public List<ResourceInstance> resourceList = new List<ResourceInstance>();

    private void Start()
    {
        SpawnResources();

        radarInfo = GetComponent<RadarObjectInfo>();
        RadarPulseController.Instance.RegisterRadarObject(gameObject);
    }

    private void SpawnResources()
    {
        maxResources = Random.Range(maxResources - 5, maxResources + 1);

        ResourceObjects selectedResource = SelectResourceByRarity();

        for (int i = 0; i < maxResources; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0);
            spawnPos.z = 0;

            GameObject instance = Instantiate(selectedResource.ResourcePrefab, spawnPos, Quaternion.identity, transform);

            float randomWeight = Random.Range(selectedResource.MinResourceWeight, selectedResource.MaxResourceWeight);
            ResourceInstance resourceInstance = instance.GetComponent<ResourceInstance>();
            if (resourceInstance != null)
            {
                resourceInstance.SetWeight(Mathf.FloorToInt(randomWeight));
                resourceInstance.SetRarity(selectedResource.Rarity);
                resourceInstance.SetName(selectedResource.ResourceName);
                resourceInstance.SetIcon(selectedResource.ResourceIcon);

                float scaleFactor = randomWeight / selectedResource.MaxResourceWeight;
                instance.transform.localScale = Vector3.one * scaleFactor;

                resourceList.Add(resourceInstance);
            }

            instance.transform.eulerAngles = new Vector3(Random.Range(0, 181), Random.Range(0, 181), Random.Range(0, 181));
        }
    }

    private ResourceObjects SelectResourceByRarity()
    {
        int maxRarity = 0;
        foreach (ResourceObjects resource in availableResources)
        {
            if (resource.Rarity >= maxRarity)
            {
                maxRarity = resource.Rarity;
            }
        }

        int[] weights = new int[availableResources.Count];
        int weightSum = 0;
        for(int i = 0; i < availableResources.Count; i++)
        {
            weights[i] = (maxRarity + 1) - availableResources[i].Rarity;
            weightSum += availableResources[i].Rarity;
        }

        int randout = WeightedRandomizer(weights, weightSum);
        return availableResources[randout];
    }

    private int WeightedRandomizer(int[] weights, int weightSum)
    {
        int i = 0;
        int last = weights.Length - 1;
        while (i < last)
        {
            if(Random.Range(0, weightSum) < weights[i])
            {
                return i;
            }

            weightSum -= weights[i++];
        }

        return i;
    }
}
