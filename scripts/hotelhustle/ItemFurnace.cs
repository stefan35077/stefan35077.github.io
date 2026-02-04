using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFurnace : MonoBehaviour
{
    StyleObject styleOBJ;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Item"))
        {
            styleOBJ = other.gameObject.GetComponentInParent<StyleObject>();
            Storagemanager.instance.AddStorage(styleOBJ);
            Destroy(other.gameObject);
        }
    }
}
