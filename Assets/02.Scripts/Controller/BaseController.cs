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
    private float moveSpeed = 5f;

    protected AnimationHandler animationHandler;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        Rotate(lookDirection);
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
        if (_knockbackDuration > 0f) _knockbackDuration -= Time.fixedDeltaTime;
    }

    private void Movement(Vector2 dir)
    {
        if (rigidbody2D == null) return;

        dir = dir * moveSpeed;
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
    }
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        _knockbackDuration = duration;
        _knockbackDirection = -(other.position - _characterRenderer.transform.position).normalized * power;
    }

}
