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

    [SerializeField] private float _spread;
    public float Spread { get { return _spread; } }

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
        //발사 최소각도
        float minAngle = (projectileNumPerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < projectileNumPerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;
            float randomSpread = Random.Range(-_spread,_spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection,angle);
        }
    }

    private void CreateProjectile(Vector2 lookDir, float angle)
    {
        _projectileManager.ShootBullet(
            this,
            _projectileSpawnPos.position,
            RotateVector2(lookDir,angle));
    }

    private static Vector2 RotateVector2(Vector2 vec, float degree)
    {
        return Quaternion.Euler(0,0,degree) * vec;
    }
}
