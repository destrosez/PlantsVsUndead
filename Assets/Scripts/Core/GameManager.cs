using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _heartsText;
    
    [SerializeField] private EnemyData[] _enemyTypes;
    [SerializeField] private int[] _enemyCounts;
    
    [SerializeField] private GameObject[] _waves;
    
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    
    private float _money = 100f;
    private int _hearts = 3;
    private float _initialDelay = 3f;
    private float _spawnInterval = 3f;
    private float _panelDuration = 2f;

    private int _currentWave;
    private int _spawnedInWave;
    private float _spawnTimer;
    private bool _spawning;
    private int _lives;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _lives = _hearts;
        UpdateMoneyUI();
        UpdateLivesUI();
        StartCoroutine(ShowWavePanelAndStart(0, _initialDelay));
    }

    private void Update()
    {
        if (_spawning && _currentWave < _enemyTypes.Length)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                SpawnEnemy();
                _spawnedInWave++;

                if (_spawnedInWave >= _enemyCounts[_currentWave])
                {
                    _spawning = false;
                    StartCoroutine(ShowWavePanelAndStart(_currentWave + 1, _panelDuration));
                }
                else
                {
                    _spawnTimer = _spawnInterval;
                }
            }
        }

        if (!_spawning && _currentWave >= _enemyTypes.Length)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                _winPanel.SetActive(true);
                Time.timeScale = 0f;
                _spawning = false;
            }
        }
    }

    private IEnumerator ShowWavePanelAndStart(int nextWaveIndex, float delay)
    {
        GameObject panel = null;
        if (nextWaveIndex == 0) panel = _waves[0];
        else if (nextWaveIndex < _enemyTypes.Length - 1) panel = _waves[1];
        else if (nextWaveIndex == _enemyTypes.Length - 1) panel = _waves[2];

        if (panel != null)
        {
            panel.SetActive(true);
            yield return new WaitForSeconds(delay);
            panel.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        _currentWave = nextWaveIndex;
        _spawnedInWave = 0;
        _spawnTimer = _spawnInterval;
        _spawning = (_currentWave < _enemyTypes.Length);
    }

    private void SpawnEnemy()
    {
        var data = _enemyTypes[_currentWave];
        int col = Random.Range(-2, 3);
        float spawnZ = 6f;
        Vector3 spawnPos = new Vector3(col, 1f, spawnZ);
        
        var go = Instantiate(data.prefab, spawnPos, Quaternion.identity);
        
        var unit = go.GetComponent<Unit>();
        var enemyController = go.GetComponent<EnemyController>();

        if (unit != null)
        {
            unit.Health = data.health;
        }

        if (enemyController != null)
        {
            enemyController.MovementSpeed  = data.movementSpeed;
            enemyController.Damage         = data.damage;
            enemyController.DamageCooldown = data.damageCooldown;
            enemyController.Worth          = data.worth;
        }
    }
    
    public void LoseLife()
    {
        _lives--;
        UpdateLivesUI();
        if (_lives <= 0)
        {
            _spawning = false;
            Time.timeScale = 0f;
            _losePanel.SetActive(true);
        }
    }
    
    private void UpdateMoneyUI()
    {
        if (_moneyText != null)
        {
            _moneyText.text = $"{Mathf.FloorToInt(_money)}";
        }
    }

    private void UpdateLivesUI()
    {
        if (_heartsText != null)
        {
            _heartsText.text = $"{_lives}/3";
        }
    }
    
    public bool TrySpendMoney(int amount)
    {
        if (_money < amount)
        {
            return false;
        }
        _money -= amount;
        UpdateMoneyUI();
        return true;
    }

    public void AddMoney(float amount)
    {
        _money += amount;
        UpdateMoneyUI();
    }
}