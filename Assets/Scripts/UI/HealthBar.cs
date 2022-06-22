using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Image _fill;
    [SerializeField] private Gradient _healthGradient;
    [SerializeField, Space] private WrapButton _wrap;
    [SerializeField] private float _wrappingPositionY;
    [SerializeField] private float _unwrappingPositionY;

    private double _maxHealth;
    private bool _isSetMaxValue;
    private Enemy _currentEnemy;

    private void OnEnable()
    {
        _wrap.IsWrapped += OnIsWrapped;
    }

    private void OnDisable()
    {
        _wrap.IsWrapped -= OnIsWrapped;
    }

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

        _fill.color = _healthGradient.Evaluate(sliderValue);
        _healthText.text = health > 0 ? NumericalFormatter.Format(health) : "!";
    }

    private void OnIsWrapped(bool isWrapped, float duration)
    {
        float currentPositionY = isWrapped == true ? _wrappingPositionY : _unwrappingPositionY;

        transform.DOMoveY(currentPositionY, duration);
    }
}
