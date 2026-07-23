using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float _delay = 1f;
    public float Delay { get => _delay; set => _delay = value; }

    [SerializeField] private float _weaponSize = 1f;
    public float WeaponSize { get => _weaponSize; set => _weaponSize = value; }

    [SerializeField] private float _atkPower = 1f;
    public float AtkPower { get => _atkPower; set => _atkPower = value; }

    [SerializeField] private float _atkSpeed = 1f;
    public float AtkSpeed { get => _atkSpeed; set => _atkSpeed = value; }

    [SerializeField] private float _atkRange = 10f;
    public float AtkRange { get => _atkRange; set => _atkRange = value; }

    public LayerMask target;

    [Header("Knockback")]
    [SerializeField] private bool _isOnKnockback = false;
    public bool IsOnKnockback { get => IsOnKnockback; set => IsOnKnockback = value; }

    [SerializeField] private float _knockbackPower = 0.1f;
    public float KnockbackPower { get => _knockbackPower; set => _knockbackPower = value;}

    [SerializeField] private float _knockbackTIme = 0.5f;

    public float KnockbackTime {  get => _knockbackTIme; set => _knockbackTIme = value;}

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");


    public BaseController Controller { get; private set; }
    private Animator _animator;
    private SpriteRenderer _weaponRenderer;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        _animator = GetComponentInChildren<Animator>();
        _weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        //delay 증가 시 속도 하락
        _animator.speed = 1.0f / _delay;
        transform.localScale = Vector3.one * _weaponSize;
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        Debug.Log("IsAttack");
        AttackAnimation();
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        _weaponRenderer.flipY = isLeft;
    }
}
