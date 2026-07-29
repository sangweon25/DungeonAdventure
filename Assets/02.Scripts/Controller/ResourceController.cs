using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float _healthchangeDelay = .5f;
    private BaseController _controller;
    private StatHandler _statHandler;
    private AnimationHandler _animationHandler;

    private float _timeSinceLastChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => _statHandler.Health;

    private void Awake()
    {
        _controller = GetComponent<BaseController>();
        _statHandler = GetComponent<StatHandler>();
        _animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        CurrentHealth = _statHandler.Health;
    }

    private void Update()
    {
        if (_timeSinceLastChange < _healthchangeDelay)
        {
            _timeSinceLastChange += Time.deltaTime;
            if (_timeSinceLastChange > _healthchangeDelay)
            {
                _animationHandler.InvincibilityEnd();
            }
        }
    }

    public bool ChangeHealth(float health)
    {
        if (health == 0 || _timeSinceLastChange < _healthchangeDelay)
        {
            return false;
        }

        _timeSinceLastChange = 0f;
        CurrentHealth = Mathf.Clamp(CurrentHealth + health, 0f, MaxHealth);
        if (health < 0)
        {
            _animationHandler.Damage();
        }

        if (CurrentHealth <= 0f)
        {
            Death();
        }
        return true;
    }

    private void Death()
    {
        _controller.Death();
    }
}
