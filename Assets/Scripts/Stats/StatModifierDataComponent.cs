using System;
using UnityEngine;

[Serializable]
public class StatModifierDataComponent : DataComponent
{
    [SerializeField]
    private StatModifier m_Modifier;

    public StatModifier Modifier => m_Modifier;
}