using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize { get { return bulletSize; } }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private float spread;  // ź ������ ����
    public float Spread { get { return spread; } }

    [SerializeField] private int numberofProjectilesPerShot; // ��� �����
    public float NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

    [SerializeField] private float multipleProjectileAngle; // ������ ź�� ���� ����
    public float MultipleProjectileAngle { get { return multipleProjectileAngle; } }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    private ProjectileManager projectileManager;

    private StatHandler statHandler;
    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
        statHandler = GetComponentInParent<StatHandler>();
    }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberofProjectilesPerShot + (int)statHandler.GetStat(StatType.ProjectileCount);

        // �ּ�ġ�� ���� ���ؼ� �ű⼭���� ź�� �� �����
        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace; // �߻��ؾ��ϴ� �ּ� ���� 

        for(int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i; //min�������� i��° ��ġ��ŭ�� ���� ������.
            float randomSpread = Random.Range(-spread, spread); // ź ������ �������� �Ұ���. ��ä�Ӱ� ������ �Ѿ��� ��.
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
        }
    }
    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        projectileManager.ShootBullet(
            this,
            projectileSpawnPosition.position,
            RotateVector2(_lookDirection, angle)
            );
    }
    /// <summary>
    /// v�� degree��ŭ ������ �Լ�
    /// </summary>
    /// <param name="v">���� ����</param>
    /// <param name="degree">���� ����</param>
    /// <returns></returns>
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        // v�� ������ŭ ������. �ٵ� ��ȯ��Ģ�� �������� �ʱ� ������ ���� ���ϱ� ���ʹϾ��� �ȵȴ�.
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
