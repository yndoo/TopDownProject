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
        // layer�� ���������� �Ǿ��ִ� �����ϸ� ��. ex ) 3���� �����ִ� : 0 0 1 0 0 0 
        // 3�� �������� �� 1���� layer�� ����Ʈ ������ �ϸ� �Ȱ��� 1 0 0 0 �� ������ ����. ���� �浹�� layer�� ���� ������ ����. 
        if(levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer))) // ���� ����� �浹���� ��
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f /*�ٷ� �پ��ִ� ���� ��ƼŬ ������ ����ؼ� ����*/, fxOnDestroy);
        }
        // target�� �浹���� ��
        else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            // ������ ó��
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

        // transform.right�� ��¥ transform��ġ�� �������� ����Ű�� ���� ������. �װ� �������ָ� �������� ȸ���� �ڵ����� ��.
        transform.right = this.direction; 

        // ���Ϲ���?
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
