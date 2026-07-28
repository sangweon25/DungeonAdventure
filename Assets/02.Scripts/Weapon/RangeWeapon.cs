using Unity.VisualScripting;
using UnityEngine;

public class RangeWeapon : WeaponHandler
{
    [Header("Range Attack")]
    [SerializeField] private Transform _projectileSpawnPos;

    [SerializeField] private int _bulletIndex;
    public int BulletIndex { get { return _bulletIndex; } }

    [SerializeField] private float _bulletSize = 1f;
    public float BulletSize { get { return _bulletSize; } }

    [SerializeField] private float _duration;
    public float Duration { get { return _duration; } }
    /// <summary>
    /// 탄퍼짐 각
    /// </summary>
    [SerializeField] private float _spread;
    public float Spread { get { return _spread; } }
    /// <summary>
    /// 1회 공격당 발사체 갯수
    /// </summary>
    [SerializeField] private int _projectiledNumPerShot;
    public int ProjectileNumPerShot { get { return _projectiledNumPerShot; } }

    [SerializeField] private float _multipleProjectileAngle;
    public float MultipleProjectileAngle { get { return _multipleProjectileAngle; } }

    [SerializeField] private Color _projectileColor;
    public Color ProjectileColor { get { return _projectileColor; } }

    private ProjectileManager _projectileManager;
    protected override void Start()
    {
        base.Start();
        _projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = MultipleProjectileAngle;
        int projectileNumPerShot = ProjectileNumPerShot;
        //발사 시작 각도 (인덱스 0 ~ 1 사이 각)
        float startAngle = -((projectileNumPerShot - 1) * projectileAngleSpace) / 2f;

        for (int i = 0; i < projectileNumPerShot; i++)
        {
            float angle = startAngle + projectileAngleSpace * i;

            angle += Random.Range(-_spread, _spread);

            CreateProjectile(
                Controller.LookDirection,
                angle);
        }
    }

    private void CreateProjectile(Vector2 lookDir, float angle)
    {
        _projectileManager.ShootBullet(
            this,
            _projectileSpawnPos.position,
            RotateVector2(lookDir, angle));
    }

    private static Vector2 RotateVector2(Vector2 vec, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * vec;
    }
}
