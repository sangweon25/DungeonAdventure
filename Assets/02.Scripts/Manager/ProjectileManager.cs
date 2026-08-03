using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager _instance;

    public static ProjectileManager Instance { get { return _instance; } }

    [SerializeField] private GameObject[] _projectilePrefabs;

    [SerializeField] private ParticleSystem _impactParticleSystem;

    private ObjectPoolManager _poolManager;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _poolManager = ObjectPoolManager.Instance;
    }

    public void ShootBullet(RangeWeapon rangeWeapon, Vector2 startPosition, Vector2 dir)
    {
        GameObject prefab = _poolManager.GetObject(rangeWeapon.BulletIndex, startPosition, Quaternion.identity);

        ProjectileController controller = prefab.GetComponent<ProjectileController>();
        controller.Init(dir, rangeWeapon,this);
    }

    public void ShootExplosive(ExplosiveRangeWeapon rangeWeapon, Vector2 startPosition, Vector2 targetPosition)
    {
        GameObject prefab = _poolManager.GetObject(rangeWeapon.BulletIndex, startPosition, Quaternion.identity);

        ExplosiveProjectileController controller = prefab.GetComponent<ExplosiveProjectileController>();
        controller.Init(targetPosition, rangeWeapon, this);
    }
    
    private GameObject CreateProjectile(int bulletIndex,Vector2 startPosition)
    {
        GameObject prefab = _projectilePrefabs[bulletIndex];

        return Instantiate(prefab, startPosition, Quaternion.identity);
    }

    public void CreateImpactParticleAtPosition(Vector3 pos,RangeWeapon rangeWeapon)
    {
        _impactParticleSystem.transform.position = pos;
        ParticleSystem.EmissionModule emissionModule = _impactParticleSystem.emission;
        emissionModule.SetBurst(0,new ParticleSystem.Burst(0,Mathf.Ceil(rangeWeapon.BulletSize * 5)));

        ParticleSystem.MainModule mainModule = _impactParticleSystem.main;
        mainModule.startSpeedMultiplier = rangeWeapon.BulletSize * 10f;
        _impactParticleSystem.Play();
    }
}
