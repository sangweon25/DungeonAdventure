using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager _instance;

    public static ProjectileManager Instance { get { return _instance; } }

    [SerializeField] private GameObject[] _projectilePrefabs;

    [SerializeField] private ParticleSystem _impactParticleSystem;

    private void Awake()
    {
        _instance = this;
    }

    public void ShootBullet(RangeWeapon rangeWeapon, Vector2 startPos, Vector2 dir)
    {
        GameObject prefab = _projectilePrefabs[rangeWeapon.BulletIndex];
        GameObject obj = Instantiate(prefab,startPos,Quaternion.identity);

        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(dir, rangeWeapon,this);

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
