using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationRepair : MonoBehaviour
{
    private float neededHealth;
    public TextMeshProUGUI healthText;
    public Slider healthFillImage;
    [Space]
    public TextMeshProUGUI repairPrice;
    public GameObject notEnoughCredit;
    private float disableTextTimer = 1.5f;
    [Space]
    public GameObject healthIsFullText;

    private void Start()
    {
        if (notEnoughCredit != null)
        {
            notEnoughCredit.SetActive(false);
        }

        if (healthIsFullText != null)
        {
            healthIsFullText.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateRepairPrice();
        CalculateNeededHealth();
    }

    public void CalculateNeededHealth()
    {
        neededHealth = PlayerController.Instance.maxHealth - PlayerController.Instance.currentHealth;
        healthText.text = Mathf.FloorToInt(neededHealth).ToString();
    }

    public void RepairShip()
    {
        if (PlayerController.Instance.currentHealth >= PlayerController.Instance.maxHealth)
        {
            healthIsFullText.SetActive(true);
            StartCoroutine(DisableText(healthIsFullText));
            PlayerController.Instance.audioManager.PlayDeny();
            return;
        }

        if (GameManager.Instance.credit >= PlayerController.Instance.repairCost)
        {
            PlayerController.Instance.ResetHealth();
            GameManager.Instance.RemoveCredit(PlayerController.Instance.repairCost);
            PlayerController.Instance.audioManager.PlayerRepair();
        }
        else
        {
            notEnoughCredit.SetActive(true);
            StartCoroutine(DisableText(notEnoughCredit));
            PlayerController.Instance.audioManager.PlayDeny();
        }
    }


    private IEnumerator DisableText(GameObject text)
    {
        yield return new WaitForSeconds(disableTextTimer);
        text.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        healthFillImage.maxValue = PlayerController.Instance.maxHealth;
        healthFillImage.value = PlayerController.Instance.currentHealth;
    }

    public void BackToMain()
    {
        PlayerController.Instance.stationUIManager.EnableMain();
        if(healthIsFullText != null ||  notEnoughCredit != null )
        {
            healthIsFullText.SetActive(false);
            notEnoughCredit.SetActive(false);
        }
    }

    private void UpdateRepairPrice()
    {
        repairPrice.text = PlayerController.Instance.repairCost.ToString();
    }
}
