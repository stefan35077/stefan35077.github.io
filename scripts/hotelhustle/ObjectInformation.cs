using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectInformation : MonoBehaviour
{
    public Style style;
    public objectType type;
}

public enum Style
{
    style1,
    style2,
    style3,
    style4,
    style5
}

public enum objectType
{
    chair,
    lamp,
    bed
}