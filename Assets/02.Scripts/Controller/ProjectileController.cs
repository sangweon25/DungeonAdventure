using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileController : MonoBehaviour , IPoolable
{
    [SerializeField] private LayerMask _levelCollisionLayer;

    private RangeWeapon _rangeWeapon;

    private float _currentDuration;
    private Vector2 _direction;
    private bool _isReady;
    private Transform _pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    public bool fxOnDestroy = true;

    private ProjectileManager _projectileManager;

    private Action<GameObject> returnToPool;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!_isReady) return;

        _currentDuration += Time.deltaTime;

        if (_currentDuration > _rangeWeapon.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        _rigidbody.velocity = _direction * _rangeWeapon.AtkSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_levelCollisionLayer.value == (_levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f, fxOnDestroy);
        }
        else if (_rangeWeapon.target.value == (_rangeWeapon.target.value | (1 << collision.gameObject.layer)))
        {
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(-_rangeWeapon.AtkPower);
                if (_rangeWeapon.IsOnKnockback)
                {
                    BaseController baseController = collision.GetComponent<BaseController>();
                    if (baseController != null)
                    {
                        baseController.ApplyKnockback(transform,_rangeWeapon.KnockbackPower, _rangeWeapon.KnockbackTime);
                    }
                }
            }

            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void Init(Vector2 dir, RangeWeapon rangeWeapon, ProjectileManager projectileManager)
    {
        this._projectileManager = projectileManager;
        _rangeWeapon = rangeWeapon;

        this._direction = dir;
        _currentDuration = 0;
        transform.localScale = Vector3.one * rangeWeapon.BulletSize;
        _spriteRenderer.color = rangeWeapon.ProjectileColor;

        transform.right = this._direction;

        if (this._direction.x < 0)
            _pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            _pivot.localRotation = Quaternion.Euler(0, 0, 0);

        _isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            _projectileManager.CreateImpactParticleAtPosition(position, _rangeWeapon);
        }
        OnDeSpawn();
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void OnSpawn()
    {

    }

    public void OnDeSpawn()
    {
        returnToPool?.Invoke(gameObject);
    }
}
