using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DataComponent
{
}

[Serializable]
public struct DataComponentList
{
    [SerializeReference]
    public List<DataComponent> m_DataComponentsList;
}
