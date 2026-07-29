using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Coroutine _waveRoutine;

    [SerializeField] private List<GameObject> _enemyPrefabs;

    [SerializeField] private List<Rect> _spawnAreas;
    [SerializeField] private Color _gizmoColor = new Color(1, 0, 0, .3f);
    [SerializeField] private List<EnemyController> _activeEnemies = new List<EnemyController>();

    private bool _enemySpawnComplete;

    [SerializeField] private float _timeBetweenSpawns = 0.2f;
    [SerializeField] private float _timeBeteweenWaves = 1f;

    private GameManager _gameManager;

    public void Init(GameManager gameManager)
    {
        this._gameManager = gameManager;
    }

    public void StartWave(int waveCount)
    {
        if (waveCount <= 0)
        {
            _gameManager.EndOfWave();
            return;
        }

        if (_waveRoutine != null)
            StopCoroutine(_waveRoutine);
        _waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        _enemySpawnComplete = false;
        yield return new WaitForSeconds(_timeBeteweenWaves);

        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            SpawnRandomEnemy();
        }

        _enemySpawnComplete = true;
    }

    private void SpawnRandomEnemy()
    {
        if (_enemyPrefabs.Count == 0 || _spawnAreas.Count == 0)
        {
            Debug.Log("Enemy Prfeabs or spawnAreas count zero");
            return;
        }
        GameObject randomPrefab = _enemyPrefabs[Random.Range(0,_enemyPrefabs.Count)];

        Rect randomArea = _spawnAreas[Random.Range(0, _spawnAreas.Count)];

        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax));

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y),Quaternion.identity);
        EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();

        _activeEnemies.Add(enemyController);
    }
    private void OnDrawGizmosSelected()
    {
        if (_spawnAreas == null) return;

        Gizmos.color = _gizmoColor;
        foreach (var area in _spawnAreas)
        {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width,area.height);
            Debug.Log($"center {center} size : {size}");
            Gizmos.DrawCube(center,size);
        }
    }

}
