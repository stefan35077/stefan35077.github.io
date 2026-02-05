using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationRefuel : MonoBehaviour
{
    private float neededFuel;
    public TextMeshProUGUI fuelText;
    public Slider fuelFillImage;
    public TextMeshProUGUI fuelPrice;

    public GameObject fuelFullText;
    public GameObject notEnoughCredit;

    public float hideTextTimer = 0.5f;

    float fuelTick = 0.2f;
    float fuelTimer = 0;

    bool doRefuel = false;

    private void Start()
    {
        if(notEnoughCredit != null)
        {
            notEnoughCredit.SetActive(false);
        }

        if(fuelFullText != null)
        {
            fuelFullText.SetActive(false);
        }
    }

    private void Update()
    {
        GetNeededFuel();
        UpdateFuelFillImage();
        UpdatePrice();

        if(doRefuel)
        {
            fuelTimer += Time.deltaTime;
            if (fuelTimer >= fuelTick)
            {
                RefuelShip();
                fuelTimer = 0;
            }
        }
        else
        {
            fuelTimer = 0;
        }
    }

    public void GetNeededFuel()
    {
        neededFuel = Mathf.Clamp(PlayerController.Instance.maxFuel - PlayerController.Instance.fuel, 0, PlayerController.Instance.maxFuel);
        float rounded = Mathf.Round(neededFuel * 10f) / 10f;
        fuelText.text = rounded.ToString();

        UpdateFuelFillImage();
    }

    public void SetRefuel(bool active)
    {
        doRefuel = active;

        if(active)
        {
            RefuelShip();
        }
        else
        {
            notEnoughCredit.SetActive(false);
        }
    }

    public void RefuelShip()
    {
        if (GameManager.Instance.credit >= PlayerController.Instance.fuelCost && PlayerController.Instance.fuel < PlayerController.Instance.maxFuel)
        {
            GameManager.Instance.RemoveCredit(PlayerController.Instance.fuelCost);
            PlayerController.Instance.fuel += 5;
            PlayerController.Instance.UpdateFuelBar();
            PlayerController.Instance.audioManager.PlayFuelBuy();
        }
        else
        {
            notEnoughCredit.SetActive(true);
            fuelFullText.SetActive(false);
            StartCoroutine(HideText(notEnoughCredit));
            PlayerController.Instance.audioManager.StopFuelBuy();
            PlayerController.Instance.audioManager.PlayDeny();
        }

        if(PlayerController.Instance.fuel >= PlayerController.Instance.maxFuel)
        {
            fuelFullText.SetActive(true);
            StartCoroutine(HideText(fuelFullText));
            notEnoughCredit.SetActive(false);
            PlayerController.Instance.audioManager.PlayDeny();
        }
    }

    private IEnumerator HideText(GameObject text)
    {
        yield return new WaitForSeconds(hideTextTimer);
        text.SetActive(false);
    }


    private void UpdateFuelFillImage()
    {
        float normalizedFuel = PlayerController.Instance.fuel / PlayerController.Instance.maxFuel;

        if (fuelFillImage != null)
        {
            fuelFillImage.value = normalizedFuel;
        }
    }

    public void BackToMain()
    {
        PlayerController.Instance.stationUIManager.EnableMain();
        if(notEnoughCredit != null ||  fuelFullText != null)
        {
            notEnoughCredit.SetActive(false);
            fuelFullText.SetActive(false);
        }
    }

    private void UpdatePrice()
    {
        fuelPrice.text = PlayerController.Instance.fuelCost.ToString();
    }
}
