using UnityEngine;

public class RangeWeapon : WeaponHandler
{
    [Header("Range Attack")]
    [SerializeField] private Transform _projectileSpawnPos;
    protected Transform ProjectileSpawnPos => _projectileSpawnPos;

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
    protected ProjectileManager ProjectileManagerInstance => _projectileManager;
    protected override void Start()
    {
        base.Start();
        _projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();

        int projectileNumPerShot = ProjectileNumPerShot;
        //발사 시작 각도 (인덱스 0 ~ 1 사이 각)
        float startAngle = CalculateStartAngle(projectileNumPerShot);
        for (int i = 0; i < projectileNumPerShot; i++)
        {
            float angle = CalculateProjectileAngle(startAngle, i);

            FireProjectile(angle);
        }
    }
    /// <summary>
    /// 첫 번째 발사체 시작 각도 계산
    /// n개의 발사체를 균등하게 퍼지도록 
    /// </summary>
    /// <param name="projectileCount">발사체 갯수</param>
    /// <returns></returns>
    private float CalculateStartAngle(int projectileCount)
    {
        return -((projectileCount - 1) * MultipleProjectileAngle) / 2f;
    }

    /// <summary>
    /// 발사체 기본 각도와 무작위 탄퍼짐 각도 계산
    /// </summary>
    /// <param name="startAngle"></param>
    /// <param name="index"> pershot index</param>
    /// <returns></returns>
    private float CalculateProjectileAngle(float startAngle, int index)
    {
        float angle = startAngle + MultipleProjectileAngle * index;
        angle += Random.Range(-Spread, Spread);

        return angle;
    }

    protected virtual void FireProjectile(float angle)
    {
        Vector2 direction = RotateVector2(Controller.LookDirection, angle);

        _projectileManager.ShootBullet(this, _projectileSpawnPos.position, direction);
    }

    /// <summary>
    /// Vector2를 degree만큼 회전
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="degree"></param>
    /// <returns></returns>
    protected static Vector2 RotateVector2(Vector2 vec, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * vec;
    }
}
