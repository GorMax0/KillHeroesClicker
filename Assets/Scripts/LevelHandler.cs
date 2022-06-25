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
    [SerializeField] private Effect _rewardEffect;

    private readonly int _bossLevel = 5;
    private int _currentLevel;
    private LevelType _currentLevelType;
    private int _levelTypeIndex = 0;
    private Enemy _currentEnemy;
    private int _enemiesPerLevel = 10;
    private int _countEnemiesDied;
    private double _healthMultiplier;
    private bool _previousLevelComplete = true;
    private Spawner _spawner;
    private bool _isLoaded;

    public event UnityAction<int, int, bool> EnemyCountChanged;
    public event UnityAction<string, int> LevelChanged;
    public event UnityAction<float> BossTimerChanged;

    public bool IsBossLevel => _currentLevel % _bossLevel == 0;
    public int CurrentLevel => _currentLevel;
    public Enemy CurrentEnemy => _currentEnemy;

    private void Start()
    {
        if (_isLoaded == false)
        {
            LoadLevel(_currentLevel, _previousLevelComplete);
            _healthMultiplier = 1;
        }
    }

    private void Update()
    {
        if (_currentEnemy != null)
            return;

        if (_previousLevelComplete == true)
        {
            if (_countEnemiesDied == _enemiesPerLevel || IsBossLevel == true)
            {
                ChangeLevel(true);
                CalculateHealthMultiplier();
            }
        }

        if (IsBossLevel == true)
            StartCoroutine(TryKillBoss());

        SpawnEnemy();
        _rewardEffect.Initiate(_currentEnemy, transform.position);
    }

    public void LoadLevel(int numberOfLevel, bool previousLevelComplete, double timeAfterSave = 0)
    {
        if (numberOfLevel == 0)
        {
            ChangeLevel(true);
            return;
        }

        for (int i = 0; i < numberOfLevel - 1; i++)
        {
            ChangeLevel(true);
            CalculateHealthMultiplier();
        }

        _previousLevelComplete = previousLevelComplete;
        _isLoaded = true;
        StartCoroutine(CalculateRewardAbsence(timeAfterSave));
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
        int bossCount = 1;
        int enemyCount = 10;

        if (IsBossLevel == true && _previousLevelComplete == true)
            ChangeLevelType(nextLevel);


        _currentLevel = nextLevel ? ++_currentLevel : --_currentLevel;
        _countEnemiesDied = 0;
        _enemiesPerLevel = IsBossLevel ? bossCount : enemyCount;
        LevelChanged?.Invoke(_currentLevelType.Label, _currentLevel);
        EnemyCountChanged?.Invoke(_countEnemiesDied, _enemiesPerLevel, _previousLevelComplete);
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

    private IEnumerator TryKillBoss()
    {
        float halfMinutes = 30f;
        float bossTimer = halfMinutes;
        float step = 0.1f;
        WaitForSeconds timerUpdateDelay = new WaitForSeconds(step);

        _previousLevelComplete = false;

        while (bossTimer > 0 && _previousLevelComplete == false)
        {
            if (bossTimer > 0)
            {
                bossTimer -= step;
                BossTimerChanged?.Invoke(bossTimer);

                yield return timerUpdateDelay;
            }
        }

        if (_previousLevelComplete == false)
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

    private IEnumerator CalculateRewardAbsence(double timeAfterSave)
    {
        do
        {
            yield return null;

            Debug.Log("Work!");
            if (_currentEnemy == null)
                continue;

            float bonusMultiplier = 0.045f;
            double absenceBonus = (int)(_player.DamagePerSecond * timeAfterSave / _currentEnemy.Health) * bonusMultiplier;
            _player.AddMoney(absenceBonus);
            Debug.Log($"Money added +{absenceBonus}");
        } while (_currentEnemy == null);

    }

    private void OnEnemyDied(Enemy enemy)
    {
        _player.AddMoney(_currentEnemy.Reward);
        UnsubscribeFromEnemy();

        if (IsBossLevel == true)
            _previousLevelComplete = true;

        _countEnemiesDied++;
        EnemyCountChanged?.Invoke(_countEnemiesDied, _enemiesPerLevel, _previousLevelComplete);
    }
}
