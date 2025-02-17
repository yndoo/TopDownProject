using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    //SerializeField �� �ν����� â�� ����
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } } // �̵� ����
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection {  get { return lookDirection; } } // �ٶ󺸴� ����

    private Vector2 knockback = Vector2.zero; // �˹��� ����
    private float knockbackDuration = 0.0f;

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    [SerializeField] public WeaponHandler weaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>(); 

        if(weaponPrefab != null)
        {
            weaponHandler = Instantiate(weaponPrefab, weaponPivot); // weaponPivot�� weaponPrefab�� �����ؼ� ����� ��
        }
        else
        {
            // �̹� ���⸦ �����ϰ� ���� �� �����ϱ� ã�ƿ��� �ڵ� 
            weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        HandleAttacakDelay();
    }
    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
        if(knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }
    /// <summary>
    /// �Է� ó��, �̵��� �ʿ��� ������ ó�� �Ұ���
    /// </summary>
    protected virtual void HandleAction()
    {

    }
    private void Movement(Vector2 direction)
    {
        direction = direction * statHandler.Speed;
        if(knockbackDuration > 0.0f) // �˹� duration�� �����ִٸ� ���� �����ؾ� ��
        {
            // �˹��� �����ؾ��Ѵٸ� ���� �̵������� ���� �ٿ��ְ� �˹��� ���� �־��ְڴ�. 
            direction *= 0.2f;
            direction += knockback;
        }

        _rigidbody.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction)
    {
        // ��ũź��Ʈ�� x�� y�� �̿��ؼ� ���� ���� ���� -> ���Ȱ����� ���ͼ� ��׸��� �ٲܰ���, Rad2Deg�� 3.14�� 180���� ����� ���ڰ���?
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; // ���̰����� 90���� �Ѿ��ٴ� �� ��и� �� ����(2,3��и�)�̶�� ��

        characterRenderer.flipX = isLeft; 

        // ���⵵ �����ٰ��� 
        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // A���� - B���� => B�� A�� �ٶ󺸴� ���Ͱ� ���� => ����� �Ÿ� ���� �� ����. ���⼭�� normalize�� ���⸸ ������. �׸��� ���̳ʽ�!! �˹��̴ϱ�.
        knockback = -(other.position - transform.position).normalized * power;
    }

    /// <summary>
    /// ���� �ð����� �߻��� �� �ְ� ������ִ� �ڵ�
    /// </summary>
    private void HandleAttacakDelay()
    {
        if (weaponHandler == null) return;

        if(timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if(isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }
    protected virtual void Attack()
    {
        if(lookDirection != Vector2.zero)
        {
            weaponHandler?.Attack();
        }
    }

    public virtual void Death()
    {
        _rigidbody.velocity = Vector3.zero;

        foreach(SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>()) // ĳ���Ͱ� ���� ��� spriterenderer�� ���İ� ����
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // Behaviour�� ������Ʈ�� �޷�����
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
