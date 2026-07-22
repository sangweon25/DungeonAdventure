using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [Range(1, 100f)][SerializeField] private int _health = 10;
    public int Health
    {
        get => _health;
        set => _health = Mathf.Clamp(value,0,100);
    }

    [Range(1, 20f)][SerializeField] private float _moveSpeed = 3;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = Mathf.Clamp(value, 0, 20);
    }

}
