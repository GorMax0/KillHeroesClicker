using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitcherLevel : MonoBehaviour
{
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private Button _nextLevel;
    [SerializeField] private TMP_Text _labelLevel;
    [SerializeField] private TMP_Text _countEnemies;

    private void OnEnable()
    {
        _levelHandler.LevelChanged += OnLevelChanged;
        _levelHandler.EnemyCountChanged += OnEnemyCountChanged;
        _levelHandler.BossTimerChanged += OnBossTimerChanged;
        _nextLevel.onClick.AddListener(_levelHandler.BackToBossLevel);
    }

    private void OnDisable()
    {
        _levelHandler.LevelChanged -= OnLevelChanged;
        _levelHandler.EnemyCountChanged -= OnEnemyCountChanged;
        _levelHandler.BossTimerChanged -= OnBossTimerChanged;
        _nextLevel.onClick.RemoveListener(_levelHandler.BackToBossLevel);
    }

    private void OnLevelChanged(string label, int numberLevel)
    {
        if (_labelLevel != null)
            _labelLevel.text = $"{label} ур. {numberLevel}";
    }

    private void OnEnemyCountChanged(int enemyCount, int enemyPerLevel, bool previousLevelComplete)
    {
        if (previousLevelComplete == true)
        {
            _countEnemies.text = $"{enemyCount}/{enemyPerLevel}";
        }
        else
        {
            if (_countEnemies.text != string.Empty)
            {
                _nextLevel.gameObject.SetActive(true);
                _countEnemies.text = string.Empty;
            }
        }
    }

    private void OnBossTimerChanged(float timer)
    {
        if (timer > 0)
            _countEnemies.text = $"{timer:N1} секунд";
    }
}
