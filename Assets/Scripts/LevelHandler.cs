using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<LevelType> _levelTypes;
    [SerializeField] private float _difficulty;
    [SerializeField] private float _enemyHealthMultiplier;
    [SerializeField] private float _bossHealthMultiplier;

    private readonly int _bossLevel = 5;
    private int _currentLevel;
    private LevelType _currentLevelType;
    private int _levelTypeIndex = 0;
    private Enemy _currentEnemy;
    private int _enemiesPerLevel = 10;
    private int _countEnemiesDied;
    private float _healthMultiplier = 1;
    private bool _bossKilled = true;
    private Spawner _spawner;

    public event UnityAction<int, int, bool> EnemyCountChanged;
    public event UnityAction<string, int> LevelChanged;
    public event UnityAction<float> BossTimerChanged;

    private bool IsBossLevel => _currentLevel % _bossLevel == 0;
    public int CurrentLevel => _currentLevel;
    public Enemy CurrentEnemy => _currentEnemy;

    private void Start()
    {
        if (_currentLevelType == null)
        {
            ChangeLevelType(true);
            _currentLevel++;
            LevelChanged?.Invoke(_currentLevelType.Label, _currentLevel);
            EnemyCountChanged?.Invoke(_countEnemiesDied, _enemiesPerLevel, _bossKilled);
        }

        SpawnEnemy();
    }

    private void Update()
    {
        if (_currentEnemy != null)
            return;

        if (_bossKilled == true)
        {
            if (_countEnemiesDied == _enemiesPerLevel || IsBossLevel == true)
            {
                ChangeLevel(true);
                CalculateHealthMultiplier();
            }
        }

         if (IsBossLevel == true)
            StartCoroutine(TryKillBoss(_currentEnemy));

        SpawnEnemy();
    }

    public void BackToBossLevel()
    {
        UnsubscribeFromEnemy();
        ChangeLevel(true);
        CalculateHealthMultiplier();
    }

    private void SpawnEnemy()
    {
        _currentEnemy = _currentLevelType.GetRandomEnemy();
        _currentEnemy = _spawner.InvokeEnemy(_currentEnemy, _healthMultiplier, _player);
        _currentEnemy.Died += OnEnemyDied;
    }

    private void ChangeLevel(bool nextLevel)
    {
        if (IsBossLevel == true && _bossKilled == true)
            ChangeLevelType(nextLevel);

        _countEnemiesDied = 0;
        EnemyCountChanged?.Invoke(_countEnemiesDied, _enemiesPerLevel, _bossKilled);
        _currentLevel = nextLevel ? ++_currentLevel : --_currentLevel;
        LevelChanged?.Invoke(_currentLevelType.Label, _currentLevel);
    }

    private void ChangeLevelType(bool nextLevelType)
    {
        if (_currentLevelType != null)
        {
            _currentLevelType.SetActiveDecoration(false);
            _levelTypeIndex = nextLevelType ? ++_levelTypeIndex : --_levelTypeIndex;
        }

        if (_levelTypeIndex == _levelTypes.Count)
            _levelTypeIndex = 0;

        if (_levelTypeIndex < 0)
            _levelTypeIndex = _levelTypes.Count - 1;

        _currentLevelType = _levelTypes[_levelTypeIndex];
        _currentLevelType.FillSpawner();
        _currentLevelType.SetActiveDecoration(true);
        _spawner = _currentLevelType.SpawnerPoint;
    }

    private void CalculateHealthMultiplier()
    {
        switch (_currentLevel % _bossLevel)
        {
            case 0:
                _healthMultiplier *= _bossHealthMultiplier;
                break;
            case 1:
                int multiplierCorrector = 30;
                int percentCorrector = 100;
                _healthMultiplier *= (multiplierCorrector - _bossHealthMultiplier) / percentCorrector;
                break;
            default:
                _healthMultiplier *= _enemyHealthMultiplier;
                break;
        }
    }

    private void CancelHealthMultiplier()
    {
        _healthMultiplier /= _bossHealthMultiplier;
    }

    private IEnumerator TryKillBoss(Enemy enemy)
    {
        float halfMinutes = 30f;
        float bossTimer = halfMinutes;
        float step = 0.1f;
        WaitForSeconds timerUpdateDelay = new WaitForSeconds(step);

        _bossKilled = false;

        while (bossTimer > 0 && _bossKilled == false)
        {
            if (bossTimer > 0)
            {
                bossTimer -= step;
                BossTimerChanged?.Invoke(bossTimer);

                yield return timerUpdateDelay;
            }
        }

        if (_bossKilled == false)
        {
            UnsubscribeFromEnemy();
            SetPreviousLevel();
        }
    }

    private void SetPreviousLevel()
    {
        ChangeLevel(false);
        CancelHealthMultiplier();
    }

    private void UnsubscribeFromEnemy()
    {
        _currentEnemy.Died -= OnEnemyDied;
        _currentEnemy.gameObject.SetActive(false);
        _currentEnemy = null;
    }

    private void OnEnemyDied(Enemy enemy)
    {        
        _player.AddMoney(_currentEnemy.Reward);
        UnsubscribeFromEnemy();

        if (IsBossLevel == true)        
            _bossKilled = true;        

        _countEnemiesDied++;
        EnemyCountChanged?.Invoke(_countEnemiesDied, _enemiesPerLevel, _bossKilled);        
    }
}