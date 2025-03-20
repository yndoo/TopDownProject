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

    [SerializeField] private float spread;  // 탄 퍼짐의 정도
    public float Spread { get { return spread; } }

    [SerializeField] private int numberofProjectilesPerShot; // 몇발 쏠건지
    public float NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

    [SerializeField] private float multipleProjectileAngle; // 각각의 탄의 퍼짐 정도
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

        // 최소치의 각을 구해서 거기서부터 탄을 쭉 쏠거임
        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace; // 발사해야하는 최소 각도 

        for(int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i; //min각도에서 i번째 위치만큼의 각도 더해줌.
            float randomSpread = Random.Range(-spread, spread); // 탄 퍼짐을 랜덤으로 할거임. 다채롭게 나가는 총알이 됨.
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
    /// v를 degree만큼 돌리는 함수
    /// </summary>
    /// <param name="v">돌릴 벡터</param>
    /// <param name="degree">돌릴 각도</param>
    /// <returns></returns>
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        // v를 각도만큼 돌리기. 근데 교환법칙이 성립하지 않기 때문에 벡터 곱하기 쿼터니언은 안된다.
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
