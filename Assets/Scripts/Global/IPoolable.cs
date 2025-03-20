using System;
using UnityEngine;

public interface IPoolable 
{
    /// <summary>
    /// ó�� ������ �� �ʱ�ȭ
    /// </summary>
    /// <param name="returnAction"></param>
    void Initialize(Action<GameObject> returnAction);
    /// <summary>
    /// ������ �� �ʱ�ȭ
    /// </summary>
    void OnSpawn();
    void OnDespawn();
}
