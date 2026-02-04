using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour
{
    public static StyleManager instance;
    private List<StyleObject> styleObjects = new List<StyleObject>();
    [SerializeField] private int CurrentStyle = 0;
    [SerializeField] private int MaxStyles = 5;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }

    public void Register(StyleObject so)
    {
        if (!styleObjects.Contains(so))
            styleObjects.Add(so);
    }

    public void Unregister(StyleObject so)
    {
        styleObjects.Remove(so);
    }

    public void ChangeObjectStyle()
    {
        CurrentStyle = (CurrentStyle + 1) % MaxStyles;
        foreach (var so in styleObjects)
            so.SetStyle(CurrentStyle);
    }
}
