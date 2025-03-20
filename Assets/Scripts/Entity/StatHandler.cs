using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatHandler에 있는 값들은 정보. ex) Health가 실시간 체력이 아님.
/// </summary>
public class StatHandler : MonoBehaviour
{
    public StatData statData;
    private Dictionary<StatType, float> currentStats = new Dictionary<StatType, float>();

    private void Awake()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        foreach (StatEntry entry in statData.stats)
        {
            currentStats[entry.statType] = entry.baseValue;
        }
    }

    public float GetStat(StatType statType)
    {
        return currentStats.ContainsKey(statType) ? currentStats[statType] : 0f;
    }

    public void ModifyStat(StatType statType, float amount, bool isPermanent = true, float duration = 0)
    {
        if (!currentStats.ContainsKey(statType)) return;

        currentStats[statType] += amount;

        if(!isPermanent)
        {
            StartCoroutine(RemoveStatAfterDutation(statType, amount, duration));
        }
    }

    private IEnumerator RemoveStatAfterDutation(StatType statType, float amount, float duration)
    {
        yield return new WaitForSeconds(duration);
        currentStats[statType] -= amount;
    }
}