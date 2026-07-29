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

    public override void Attack()
    {
        AttackAnimation();

        int projectileNumPerShot = ProjectileNumPerShot;
        float projectileAngleSpace = MultipleProjectileAngle;
        float startAngle = -((projectileNumPerShot - 1) * projectileAngleSpace) / 2f;

        for (int i = 0; i < projectileNumPerShot; i++)
        {
            Vector2 targetPosition = GetTargetPosition();
            float angle = startAngle + projectileAngleSpace * i;
            angle += Random.Range(-Spread, Spread);

            Vector2 startPosition = ProjectileSpawnPos.position;
            Vector2 direction = (targetPosition - startPosition).normalized;
            Vector2 adjustedTargetPosition = startPosition + RotateVector2(direction, angle) * Vector2.Distance(startPosition, targetPosition);

            ProjectileManagerInstance.ShootExplosive(this, startPosition, adjustedTargetPosition);
        }
    }

    private Vector2 GetTargetPosition()
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
