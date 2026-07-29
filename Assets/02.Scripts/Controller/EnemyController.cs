using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager _enemyManager;
    private Transform _target;

    [SerializeField] private float _followRange = 15f;

    public void Init(EnemyManager enemyManager, Transform target)
    {
        this._enemyManager = enemyManager;
        this._target = target;
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (_target.position - transform.position).normalized;
    }

    protected override void HandlerAction()
    {
        base.HandlerAction();

        if (weaponHandler == null || _target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        if (distance <= _followRange)
        {
            lookDirection = direction;
            if (distance < weaponHandler.AtkRange)
            {
                int layerMaskTarget = weaponHandler.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position,
                    direction,
                    weaponHandler.AtkRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level") | layerMaskTarget));

                //충돌 Layer가 공격 대상인지
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero;
                return;
            }
            movementDirection = direction;
        }
    }
    public override void Death()
    {
        base.Death();
        _enemyManager.RemoveEnemyOnDeath(this);
    }

}
