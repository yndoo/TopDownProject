using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // ������ �� �ʿ��� ������
    // [Header] �� �ν����Ϳ� �޼��� ���� ��
    [Header("Attack Info")]
    [SerializeField] private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }
    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get => weaponSize; set=> weaponSize = value; }
    [SerializeField] public float power = 1f;
    public float Power { get => power; set => power = value; }
    [SerializeField] private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }
    [SerializeField] private float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    [Header("Knockback Info")]
    [SerializeField] private bool isOnKnockback = false;
    public bool IsOnKnockback {  get => isOnKnockback; set => isOnKnockback = value;}
    [SerializeField] private float knockbackPower = 0.1f;
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }
    [SerializeField] private float knockbackTime = 0.5f;
    public float KnockbackTime {  get => knockbackTime; set => knockbackTime = value; }

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public BaseController Controller { get; private set; }
    public Animator animator;
    private SpriteRenderer weaponRenderer;

    public AudioClip attackSoundClip;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        animator = GetComponentInChildren<Animator>();  
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay; // delay�� ���������� �ִϸ��̼��� ������ �����ϰ� ������ٰ��� 
        transform.localScale = Vector3.one * weaponSize; // �ν����Ϳ��� ������ ����� ����� ���� �� �ְ� 
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        AttackAnimation();

        if(attackSoundClip != null)
        {
            SoundManager.PlayClip(attackSoundClip);
        }
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        weaponRenderer.flipY = isLeft;
    }
}
