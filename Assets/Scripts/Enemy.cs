using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private double _health;

    private double _currentHealth;
    private double _reward;

    public event UnityAction<double> HealthChanged;
    public event UnityAction<Enemy> Died;

    public string Name => _name;
    public double Reward => _reward;

    public void Init(float healthMultiplier, Player player)
    {
        float _rewardMultiplier = 0.09f;

        _currentHealth = _health;
        _currentHealth = Math.Ceiling(_currentHealth * healthMultiplier);
        HealthChanged?.Invoke(_currentHealth);

        _reward = Math.Ceiling(_currentHealth * _rewardMultiplier);
        StartCoroutine(ReceiveDamagePerSecond(player));
    }

    public void ReceiveDamage(double damage)
    {
        _currentHealth -= damage;
        HealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
            Died?.Invoke(this);
    }

    private IEnumerator ReceiveDamagePerSecond(Player player)
    {
        float unitTime = 0.1f;
        double damagePerUnitTime = 0;
        WaitForSeconds waitForSeconds = new WaitForSeconds(unitTime);
        Mathf mathfExtension;

        while (_currentHealth > 0)
        {
            damagePerUnitTime = player.DamagePerSecond * unitTime;
            _currentHealth = mathfExtension.MoveTowards(_currentHealth, 0, damagePerUnitTime);
            HealthChanged?.Invoke(_currentHealth);

            yield return waitForSeconds;
        }

        Died?.Invoke(this);
    }
}
