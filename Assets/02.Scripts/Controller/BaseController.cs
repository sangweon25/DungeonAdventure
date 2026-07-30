using System.Collections;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rigidbody2D;

    [SerializeField] private SpriteRenderer _characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 _knockbackDirection = Vector2.zero;
    private float _knockbackDuration = 0f;

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;
    protected WeaponHandler weaponHandler;

    [SerializeField] private WeaponHandler _weaponPrefab;
    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 15f;
    [SerializeField] private float _dashDuration = 0.15f;
    [SerializeField] private float _dashCooldown = 0.5f;

    private bool _isDashing;
    private bool _canDash = true;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();

        if (_weaponPrefab != null)
        {
            weaponHandler = Instantiate(_weaponPrefab,weaponPivot);
        }
        else
        {
            weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandlerAction();
        Rotate(lookDirection);
        AttackDelay();
    }

    protected virtual void FixedUpdate()
    {
        if (_isDashing) return;
        Movement(movementDirection);
        if (_knockbackDuration > 0f) _knockbackDuration -= Time.fixedDeltaTime;
    }


    protected virtual void HandlerAction()
    {

    }

    private void Movement(Vector2 dir)
    {
        if (rigidbody2D == null) return;

        dir = dir * statHandler.MoveSpeed;
        //Apply Knockback
        if (_knockbackDuration > 0.0f)
        {
            dir *= 0.2f;
            dir += _knockbackDirection;
        }

        rigidbody2D.velocity = dir;
        animationHandler.Move(dir);
    }

    private void Rotate(Vector2 dir)
    {
        float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotationZ) > 90f;

        _characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }
        weaponHandler?.FlipWeapon(isLeft);

    }
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        _knockbackDuration = duration;
        _knockbackDirection = -(other.position - _characterRenderer.transform.position).normalized * power;
    }

    private void AttackDelay()
    {
        if (weaponHandler == null) return;

        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
        {
            weaponHandler.Attack();
        }
    }

    protected void Dash()
    {
        if (!_canDash || _isDashing || rigidbody2D == null) return;

        Vector2 dashDir = movementDirection;
        //입력이 없으면 lookDirection 방향을 dash방향으로
        if (dashDir.sqrMagnitude <= Mathf.Epsilon)
        {
            dashDir = lookDirection;
        }
        //바라보는 방향도 없을 경우 dash x 
        if (dashDir.sqrMagnitude <= Mathf.Epsilon) return;

        StartCoroutine(DashRoutine(dashDir.normalized));
    }

    private IEnumerator DashRoutine(Vector2 dashDir)
    {
        _canDash = false;
        _isDashing = true;

        rigidbody2D.velocity = dashDir * _dashSpeed;

        yield return new WaitForSeconds(_dashDuration);

        _isDashing = false;
        rigidbody2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(_dashCooldown);

        _canDash = true;
    }

    public virtual void Death()
    {
        rigidbody2D.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour item in transform.GetComponentsInChildren<Behaviour>())
        {
            item.enabled = false;
        }
        Destroy(gameObject,2f);
    }

}
