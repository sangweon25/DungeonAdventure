using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager _instance;

    public static ProjectileManager Instance { get { return _instance; } }

    [SerializeField] private GameObject[] _projectilePrefabs;

    private void Awake()
    {
        _instance = this;
    }

    public void ShootBullet(RangeWeapon rangeWeapon, Vector2 startPos, Vector2 dir)
    {
        GameObject prefab = _projectilePrefabs[rangeWeapon.BulletIndex];
        GameObject obj = Instantiate(prefab,startPos,Quaternion.identity);

        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(dir, rangeWeapon);
    }
}
