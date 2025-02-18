using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테미나, 마나포인트 등의 자원들을 관리
/// </summary>
public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    private float timeSinceLastChange = float.MaxValue; // 변화를 가진 시간을 저장. 그래야 일정 시간 후에 다시 변화를 갖기 때문.

    public float CurrentHealth { get; private set; }
    public float MaxHealth => statHandler.Health;

    public AudioClip damageClip;

    private Action<float, float> OnChangeHealth;

    private void Awake()
    {
        baseController = GetComponent<BaseController>();
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    private void Update()
    {
        if(timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if(timeSinceLastChange > healthChangeDelay )
            {
                animationHandler.InvincibilityEnd();
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if(change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0;
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);

        if(change < 0)
        {
            animationHandler.Damage();
            if (damageClip != null)
            {
                SoundManager.PlayClip(damageClip);
            }
        }

        if(CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    private void Death()
    {
        baseController.Death();
    }

    public void AddHealthChangeEvent(Action<float,  float> action)
    {
        OnChangeHealth += action;
    }

    public void RemoveHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth -= action;
    }
}
