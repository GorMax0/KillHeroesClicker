using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private double _damagePerClick = 1d;
    private float _damagePerClickMultiplier;
    [SerializeField] private double _damagePerSecond = 0d;
    private List<Hero> _heroes = new List<Hero>();

    public event UnityAction<double> MoneyChanged;
    public event UnityAction DamageChanged;

    [field: SerializeField] public double Money { get; private set; }
    public double DamagePerClick => _damagePerClick;
    public double DamagePerSecond => _damagePerSecond;

    private void OnDisable()
    {
        foreach (var hero in _heroes)
        {
            hero.DamageChanged -= AddDamage;
            hero.DamageMultiplierChanged -= OnDamageMultiplierChanged;
        }
    }

    public void LoadData(List<Hero> loadedHeroes, double money)
    {
        foreach (var loadedHero in loadedHeroes)
        {
            AddHero(loadedHero);
        }

        AddMoney(money);

        Debug.Log($"Number of heroes of the list: {_heroes.Count}");
    }

    public List<Hero> GetHeroes()
    {
        List<Hero> newHeroesCollection = new List<Hero>();

        newHeroesCollection.AddRange(_heroes);

        return newHeroesCollection;
    }

    public void AddMoney(double money)
    {
        if (money > 0)
        {
            Money += money;
            MoneyChanged?.Invoke(Money);
        }
    }

    public void BuyHero(Hero buyedHero, double sumPrice)
    {
        if (Money >= sumPrice)
        {
            Money -= sumPrice;
            MoneyChanged?.Invoke(Money);

            if (buyedHero.IsBuyed == false)
            {
                AddHero(buyedHero);
            }
        }
    }

    private void AddHero(Hero heroForAdd)
    {
        heroForAdd.DamageChanged += AddDamage;
        heroForAdd.DamageMultiplierChanged += OnDamageMultiplierChanged;
        _heroes.Add(heroForAdd);
    }

    private void AddDamage(double damage, float multiplier)
    {
        double tempDamagePerClick = 0;
        double tempDamagePerSecond = 0;

        foreach (var hero in _heroes)
        {
            if (hero.IsDamagePerClick == true)
                tempDamagePerClick += hero.Damage;
            else
                tempDamagePerSecond += hero.Damage;
        }

        _damagePerClick -= _damagePerSecond * _damagePerClickMultiplier;
        _damagePerSecond = tempDamagePerSecond;
        _damagePerClick = tempDamagePerClick + _damagePerSecond * _damagePerClickMultiplier;
        DamageChanged?.Invoke();
    }

    private void OnDamageMultiplierChanged(float multiplier)
    {
        _damagePerClickMultiplier = multiplier;
    }
}
