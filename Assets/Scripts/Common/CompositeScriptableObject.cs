using System;
using System.Linq;
using UnityEngine;

public class CompositeScriptableObject : ScriptableObject
{
    [SerializeField]
    private DataComponentList m_DataComponents;

    public bool HasDataComponent<T>() where T : DataComponent
    {
        return m_DataComponents.m_DataComponentsList.Any(x => x is T);
    }

    public bool HasDataComponent(Type type)
    {
        return m_DataComponents.m_DataComponentsList.Any(x => x.GetType() == type);
    }

    public T GetDataComponent<T>() where T : DataComponent
    {
        return m_DataComponents.m_DataComponentsList.FirstOrDefault(x => x is T) as T;
    }
}
