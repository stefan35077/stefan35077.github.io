using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Storagemanager : MonoBehaviour
{
    public static Storagemanager instance;

    private Dictionary<StyleObject, int> storageData = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void RegisterStyleObject(StyleObject obj, int initialAmount)
    {
        if (!storageData.ContainsKey(obj))
        {
            storageData[obj] = initialAmount;
            UpdateUI(obj);
        }
    }

    public void RemoveStorage(StyleObject obj)
    {
        if (storageData.ContainsKey(obj))
        {
            if (storageData[obj] > 0)
            {
                storageData[obj]--;
                UpdateUI(obj);
            }
            else
            {
                Debug.LogWarning("Tried to remove item from empty storage!");
                return;
            }
        }
    }

    public void AddStorage(StyleObject obj)
    {
        if (storageData.ContainsKey(obj))
        {
            storageData[obj]++;
            UpdateUI(obj);
        }
    }

    public int GetAmountLeft(StyleObject obj)
    {
        return storageData.ContainsKey(obj) ? storageData[obj] : 0;
    }

    private void UpdateUI(StyleObject obj)
    {
        obj.UpdateAmountUIText(GetAmountLeft(obj));
    }
}
