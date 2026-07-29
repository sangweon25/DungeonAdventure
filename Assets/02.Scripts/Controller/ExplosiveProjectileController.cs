using UnityEngine;

public class ExplosiveProjectileController : MonoBehaviour
{
    private ExplosiveRangeWeapon _explosiveRangeWeapon;
    private ProjectileManager _projectileManager;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Transform _pivot;

    private Vector2 _targetPosition;
    private Vector2 _direction;
    private float _currentDelay;
    private bool _isReady;
    private bool _hasArrived;

    private Animator _animator;

    [SerializeField] private float rotationSpeed = 360f;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
        _pivot = transform.GetChild(0);
        _animator = GetComponentInChildren<Animator>();
        
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
        //¿©±â ÆøÅº ¾Ö´Ï¸ÞÀÌ¼Ç
        if (_currentDelay >= _explosiveRangeWeapon.ExplosionDelay)
        {
            Explode();
        }
    }

    public void Init(Vector2 targetPosition, ExplosiveRangeWeapon rangeWeapon, ProjectileManager projectileManager)
    {
        _explosiveRangeWeapon = rangeWeapon;
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
        _animator.speed = 1.0f / _explosiveRangeWeapon.ExplosionDelay;
    }

    private void MoveToTarget()
    {
        Vector2 currentPosition = transform.position;
        float moveDistance = _explosiveRangeWeapon.AtkSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (Vector2.Distance(currentPosition, _targetPosition) <= moveDistance)
        {
            ArriveAtTarget();
            
            return;
        }

        _rigidbody.velocity = _direction * _explosiveRangeWeapon.AtkSpeed;
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosiveRangeWeapon.ExplosionRadius, _explosiveRangeWeapon.target);

        foreach (Collider2D hit in hits)
        {
            ResourceController resourceController = hit.GetComponent<ResourceController>();
            if (resourceController == null) continue;

            resourceController.ChangeHealth(-_explosiveRangeWeapon.AtkPower);

            if (_explosiveRangeWeapon.IsOnKnockback)
            {
                BaseController baseController = hit.GetComponent<BaseController>();
                if (baseController != null)
                {
                    baseController.ApplyKnockback(transform, _explosiveRangeWeapon.KnockbackPower, _explosiveRangeWeapon.KnockbackTime);
                }
            }
        }

        _projectileManager.CreateImpactParticleAtPosition(transform.position, _explosiveRangeWeapon);
        Destroy(gameObject);
    }

    private void ArriveAtTarget()
    {
        transform.position = _targetPosition;
        _rigidbody.velocity = Vector2.zero;
        _hasArrived = true;
        _animator.SetTrigger("Fuse");
    }
}
