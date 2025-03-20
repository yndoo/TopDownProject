using System;
using UnityEngine;

public interface IPoolable 
{
    /// <summary>
    /// 처음 생성될 때 초기화
    /// </summary>
    /// <param name="returnAction"></param>
    void Initialize(Action<GameObject> returnAction);
    /// <summary>
    /// 스폰될 때 초기화
    /// </summary>
    void OnSpawn();
    void OnDespawn();
}
