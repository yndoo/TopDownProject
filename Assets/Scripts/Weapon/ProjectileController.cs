using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangeWeaponHandler rangeWeaponHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestroy = true;

    ProjectileManager projectileManager;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady) return;

        currentDuration += Time.deltaTime;

        if(currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        _rigidbody.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // layer가 이진법으로 되어있다 생각하면 됨. ex ) 3번이 켜져있다 : 0 0 1 0 0 0 
        // 3번 켜져있을 때 1에다 layer값 시프트 연산을 하면 똑같이 1 0 0 0 이 나오는 거임. 지금 충돌한 layer의 값이 나오는 거임. 
        if(levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer))) // 레벨 벽면과 충돌했을 때
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f /*바로 붙어있는 데서 파티클 나오면 어색해서 빼줌*/, fxOnDestroy);
        }
        // target과 충돌했을 때
        else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            // 데미지 처리
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if(resourceController != null)
            {
                resourceController.ChangeHealth(-rangeWeaponHandler.Power);
                if(rangeWeaponHandler.IsOnKnockback)
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if(controller != null)
                    {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }
            }

            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager)
    {
        this.projectileManager = projectileManager;

        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        // transform.right는 진짜 transform위치의 오른쪽을 가리키는 방향 가져옴. 그걸 설정해주면 나머지의 회전이 자동으로 됨.
        transform.right = this.direction; 

        // 상하반전?
        if(direction.x < 0)
        {
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            pivot.localRotation = Quaternion.Euler(0, 0, 0);
        }

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if(createFx)
        {
            projectileManager.CreateImpactParticlesAtPosition(position, rangeWeaponHandler);
        }

        Destroy(this.gameObject);
    }
}
