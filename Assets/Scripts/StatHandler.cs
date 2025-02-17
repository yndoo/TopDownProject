using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatHandler에 있는 값들은 정보. ex) Health가 실시간 체력이 아님.
/// </summary>
public class StatHandler : MonoBehaviour
{
    // Range : min과 max로 값 제한할 수 있음
    [Range(1, 100)][SerializeField] private int health = 10;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }
    [Range(1f, 20f)][SerializeField] private float speed = 3;
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }
}
