using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StatHolder : MonoBehaviour
{
    [SerializeField]
    private Stat[] m_ContainedStats;

    private List<StatModifier> m_Modifiers = new List<StatModifier>();
    public UnityEvent<Stat, float> OnStatChange;

    public void AddModifer(StatModifier statModifier)
    {
        m_Modifiers.Add(statModifier);

        OnStatChange?.Invoke(statModifier.ImpactedStat, ComputeStatValue(statModifier.ImpactedStat));
    }

    public void RemoveModifer(StatModifier statModifier)
    {
        m_Modifiers.Remove(statModifier);

        OnStatChange?.Invoke(statModifier.ImpactedStat, ComputeStatValue(statModifier.ImpactedStat));
    }

    private float ComputeStatValue(Stat stat)
    {
        float value = (m_ContainedStats.Contains(stat) ? stat.BaseValue : 0.0f);
        m_Modifiers.FindAll(x => x.ImpactedStat == stat).ForEach(x => value += x.ModifierValue);
        return value;
    }
}
