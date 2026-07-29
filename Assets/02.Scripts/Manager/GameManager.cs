using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController playerController { get; private set; }
    private ResourceController _resourceController;

    [SerializeField] private int _currentWaveIndex = 0;

    private EnemyManager _enemyManager;

    private void Awake()    
    {
        Instance = this;
        playerController = FindObjectOfType<PlayerController>();
        playerController.Init(this);

        _enemyManager = GetComponentInChildren<EnemyManager>();
        _enemyManager.Init(this);
    }

    public void StartGame()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        _currentWaveIndex += 1;
        _enemyManager.StartWave(1 + _currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        _enemyManager.StopWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

}
