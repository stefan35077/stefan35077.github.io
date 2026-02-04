using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StyleObject : MonoBehaviour
{
    [SerializeField] private GameObject ObjectPrefab;
    [SerializeField] private TextMeshProUGUI TextObject_AmountLeft;

    [SerializeField] private List<GameObject> PossibleStyles = new();
    private int _currentStyleIndex = 0;
    [SerializeField] private int maxAmount;
    [SerializeField] private GameObject lastSpawnedObject;

    [SerializeField] StyleIdentifier styleId;

    private int increment = 0;

    [SerializeField] private bool canGrab;


    private void OnEnable()
    {
        if (StyleManager.instance != null)
            StyleManager.instance.Register(this);
    }

    private void OnDisable()
    {
        StyleManager.instance?.Unregister(this);
    }

    private void Start()
    {
        StyleManager.instance.Register(this);
        BuildStyleList();
        SetStyle(0);
        Storagemanager.instance.RegisterStyleObject(this, initialAmount: maxAmount);
        SetStyleIdentifier();
    }

    private void Update()
    {
        checkStorage();
    }

    private void BuildStyleList()
    {
        Storagemanager.instance.RemoveStorage(this);
        var container = Instantiate(ObjectPrefab, transform.position, Quaternion.identity, transform);
        container.name = container.name + increment++;

        var objectStyleList = container.GetComponent<ObjectStyleList>();
        if (objectStyleList == null)
        {
            Debug.LogError($"ObjectStyleList component not found on prefab {ObjectPrefab.name}");
            return;
        }

        PossibleStyles.Clear();
        PossibleStyles.AddRange(objectStyleList.objectStyles);

        foreach (var style in PossibleStyles)
        {
            style.SetActive(false);
        }

        lastSpawnedObject = container;
    }

    public void UpdateAmountUIText(int newAmount)
    {
        TextObject_AmountLeft.text = newAmount.ToString();
    }

    public void SetStyle(int idx)
    {
        for (int i = 0; i < PossibleStyles.Count; i++)
            PossibleStyles[i].SetActive(i == idx);

        _currentStyleIndex = idx;
        SetStyleIdentifier();
    }

    public void OnGrabbed(GameObject obj)
    {
        checkStorage();

        if (obj == lastSpawnedObject && canGrab)
        {
            var id = GetComponent<StyleIdentifier>();
            if (id != null)
                id.style = (ObjectStyle)_currentStyleIndex;
            StartCoroutine(InstantiateObj());
        }
    }

    IEnumerator InstantiateObj()
    {
        yield return new WaitForSeconds(2);

        var replacementGO = ObjectPrefab;
        var replacementSO = gameObject.GetComponentInParent<StyleObject>();
        replacementSO.BuildStyleList();
        replacementSO.SetStyle(_currentStyleIndex);
    }


    public void SetStyleIdentifier()
    {
        styleId = null;

        foreach (Transform child in transform)
        {
            styleId = child.GetComponent<StyleIdentifier>();
        }

        switch (_currentStyleIndex)
        {
            case 0:
                styleId.style = ObjectStyle.Realistic;
                break;
            case 1:
                styleId.style = ObjectStyle.Cartoon;
                break;
            case 2:
                styleId.style = ObjectStyle.Shibby;
                break;
            case 3:
                styleId.style = ObjectStyle.BlackWhite;
                break;
            case 4:
                styleId.style = ObjectStyle.Stylised;
                break;

        }
    }

    public void checkStorage()
    {
        if (Storagemanager.instance.GetAmountLeft(this) > 0)
        {
            canGrab = true;
        }
        else
        {
            canGrab = false;
        }

        if (!canGrab)
        {
            lastSpawnedObject = null;
            PossibleStyles.Clear();
        }
    }
}
