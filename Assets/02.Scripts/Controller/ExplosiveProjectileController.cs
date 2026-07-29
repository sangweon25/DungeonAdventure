using UnityEngine;

public class ExplosiveProjectileController : MonoBehaviour
{
    private ExplosiveRangeWeapon _rangeWeapon;
    private ProjectileManager _projectileManager;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Transform _pivot;

    private Vector2 _targetPosition;
    private Vector2 _direction;
    private float _currentDelay;
    private bool _isReady;
    private bool _hasArrived;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
        _pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!_isReady) return;

        if (!_hasArrived)
        {
            MoveToTarget();
            return;
        }

        _currentDelay += Time.deltaTime;
        if (_currentDelay >= _rangeWeapon.ExplosionDelay)
        {
            Explode();
        }
    }

    public void Init(Vector2 targetPosition, ExplosiveRangeWeapon rangeWeapon, ProjectileManager projectileManager)
    {
        _rangeWeapon = rangeWeapon;
        _projectileManager = projectileManager;
        _targetPosition = targetPosition;
        _direction = (_targetPosition - (Vector2)transform.position).normalized;
        _currentDelay = 0f;
        _hasArrived = false;

        transform.localScale = Vector3.one * rangeWeapon.BulletSize;
        _spriteRenderer.color = rangeWeapon.ProjectileColor;
        transform.right = _direction;

        if (_direction.x < 0)
            _pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            _pivot.localRotation = Quaternion.Euler(0, 0, 0);

        _isReady = true;
    }

    private void MoveToTarget()
    {
        Vector2 currentPosition = transform.position;
        float moveDistance = _rangeWeapon.AtkSpeed * Time.deltaTime;

        if (Vector2.Distance(currentPosition, _targetPosition) <= moveDistance)
        {
            transform.position = _targetPosition;
            _rigidbody.velocity = Vector2.zero;
            _hasArrived = true;
            return;
        }

        _rigidbody.velocity = _direction * _rangeWeapon.AtkSpeed;
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _rangeWeapon.ExplosionRadius, _rangeWeapon.target);

        foreach (Collider2D hit in hits)
        {
            ResourceController resourceController = hit.GetComponent<ResourceController>();
            if (resourceController == null) continue;

            resourceController.ChangeHealth(-_rangeWeapon.AtkPower);

            if (_rangeWeapon.IsOnKnockback)
            {
                BaseController baseController = hit.GetComponent<BaseController>();
                if (baseController != null)
                {
                    baseController.ApplyKnockback(transform, _rangeWeapon.KnockbackPower, _rangeWeapon.KnockbackTime);
                }
            }
        }

        _projectileManager.CreateImpactParticleAtPosition(transform.position, _rangeWeapon);
        Destroy(gameObject);
    }
}
