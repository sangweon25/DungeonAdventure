using UnityEngine;

public class ExplosiveRangeWeapon : RangeWeapon
{
    [Header("Explosive Projectile")]
    [SerializeField] private Vector2 _targetPoint = Vector2.one;
    public Vector2 TargetPoint => _targetPoint;

    [SerializeField] private float _explosionDelay = 1.5f;
    public float ExplosionDelay => _explosionDelay;

    [SerializeField] private float _explosionRadius = 2f;
    public float ExplosionRadius => _explosionRadius;

    protected override void FireProjectile(float angle)
    {
        Vector2 startPosition = ProjectileSpawnPos.position;
        Vector2 targetPoint = GetTargetPoint();

        Vector2 direction = (targetPoint - startPosition).normalized;
        Vector2 targetPosition = startPosition + RotateVector2(direction, angle) * Vector2.Distance(startPosition, targetPoint);
        ProjectileManagerInstance.ShootExplosive(this, startPosition, targetPosition);
    }

    /// <summary>
    /// 폭발형 발사체 목표 위치 계산
    /// </summary>
    /// <returns></returns>
    private Vector2 GetTargetPoint()
    {
        Vector2 targetPosition = (Vector2)transform.position + Controller.LookDirection * AtkRange;

        if (Controller is EnemyController enemyController && enemyController.Target != null)
        {
            targetPosition = enemyController.Target.position;
        }

        Vector2 randomOffset = new Vector2(
            Random.Range(-_targetPoint.x, _targetPoint.x),
            Random.Range(-_targetPoint.y, _targetPoint.y));

        return targetPosition + randomOffset;
    }
}
