using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    //SerializeField 로 인스펙터 창에 공개
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } } // 이동 방향
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection {  get { return lookDirection; } } // 바라보는 방향

    private Vector2 knockback = Vector2.zero; // 넉백의 방향
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
            weaponHandler = Instantiate(weaponPrefab, weaponPivot); // weaponPivot에 weaponPrefab을 복제해서 만들게 됨
        }
        else
        {
            // 이미 무기를 장착하고 있을 수 있으니까 찾아오는 코드 
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
    /// 입력 처리, 이동에 필요한 데이터 처리 할거임
    /// </summary>
    protected virtual void HandleAction()
    {

    }
    private void Movement(Vector2 direction)
    {
        direction = direction * statHandler.Speed;
        if(knockbackDuration > 0.0f) // 넉백 duration이 남아있다면 아직 적용해야 함
        {
            // 넉백을 적용해야한다면 기존 이동방향의 힘을 줄여주고 넉백의 힘을 넣어주겠다. 
            direction *= 0.2f;
            direction += knockback;
        }

        _rigidbody.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction)
    {
        // 아크탄젠트로 x와 y값 이용해서 사이 각도 구함 -> 라디안값으로 나와서 디그리로 바꿀거임, Rad2Deg는 3.14를 180으로 만드는 숫자겠지?
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; // 사이각도가 90도를 넘었다는 건 사분면 중 왼쪽(2,3사분면)이라는 뜻

        characterRenderer.flipX = isLeft; 

        // 무기도 돌려줄거임 
        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // A벡터 - B벡터 => B가 A를 바라보는 벡터가 나옴 => 방향과 거리 구할 수 있음. 여기서는 normalize로 방향만 가져옴. 그리고 마이너스!! 넉백이니까.
        knockback = -(other.position - transform.position).normalized * power;
    }

    /// <summary>
    /// 일정 시간마다 발사할 수 있게 만들어주는 코드
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

        foreach(SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>()) // 캐릭터가 가진 모든 spriterenderer에 알파값 적용
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // Behaviour는 컴포넌트에 달려있음
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
