using System.Collections.Generic;
using UnityEngine;

public class StationScript : MonoBehaviour
{
    [Header("resource scanner")]
    [Tooltip("scan time in minutes")]
    public float resourceScanTime = 1;
    public float resourceScanRadius = 50;
    public LayerMask resourceLayer;
    [Header("fuel cost")]
    public int fuelMaxCost = 8;
    public int fuelMinCost = 1;
    [Header("Repair cost")]
    public int repairMaxCost = 285;
    public int repairMinCost = 100;

    float ScanTimer;

    PlayerController C;
    Vector3 playerLocal;

    bool firstTimeScan = true;

    //what things are common in the area
    public float rarityInArea = 0;
    //how much does 1 unit of fuel cost in credits, 1 is lowest
    public int fuelPrice = 1;
    // Same thing apply for the health
    public int repairPrice = 1;


    private void Update()
    {
        //scan when the timer hits the target
        ScanTimer += Time.deltaTime;
        if (ScanTimer >= (resourceScanTime * 60) || firstTimeScan)
        {
            CheckNearbyResources();
            //change fuel cost
            fuelPrice = Random.Range(fuelMinCost, fuelMaxCost + 1);
            // change repair cost
            repairPrice = Random.Range(repairMinCost, repairMaxCost + 1);

            //add randomizer to not have all the stations to scan at once
            ScanTimer = 0 - Random.Range(0, 20);
            firstTimeScan = false;
        }

        //write any aditional code above this
        if(!C)
        {
            return;
        }

        if(C.isMainStationUIEnabled)
        {
            C.transform.position = transform.position + playerLocal;
            C.rb.linearVelocity = Vector2.zero;
        }
        else
        {
            playerLocal = C.transform.position - transform.position;
        }
    }

    void CheckNearbyResources()
    {
        float newRarity = 0;
        float divisionCount = 0;
        foreach(ResourceInstance r in GameManager.Instance.GetResourceList())
        {
            if(Vector2.Distance(transform.position, r.transform.position) < resourceScanRadius)
            {
                newRarity += r.Rarity;
                divisionCount++;
            }
        }

        rarityInArea = (newRarity / divisionCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.isInStationArea = true;
            controller.EnableInteractionIcon();
            controller.StationRarity = rarityInArea;
            controller.fuelCost = fuelPrice;
            controller.repairCost = repairPrice;

            C = controller;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.isInStationArea = false;
            controller.DisableInteractionIcon();

            C = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, resourceScanRadius);
    }
}
