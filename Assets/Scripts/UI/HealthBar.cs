using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _healthText;

    private double _maxHealth;
    private bool _isSetMaxValue;
    private Enemy _currentEnemy;

    public void SetSubscription(Enemy invokedEnemy)
    {
        if (_currentEnemy != null)
            _currentEnemy.HealthChanged -= OnHealthChanded;

        _currentEnemy = invokedEnemy;
        _currentEnemy.HealthChanged += OnHealthChanded;
        _isSetMaxValue = false;
    }

    private void OnHealthChanded(double health)
    {
        float sliderValue;

        if (_isSetMaxValue == false)
        {
            _maxHealth = health;
            _isSetMaxValue = true;
        }

        sliderValue = (float)(health / _maxHealth);
        _slider.value = sliderValue;
        _healthText.text = health > 0 ? NumericalFormatter.Format(health) : "!";
    }
}
