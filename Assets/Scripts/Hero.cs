using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Hero
{
    private double _baseDamage;
    private float _damageMultiplier;
    private float _damageMultiplierModifier;

    public Hero(HeroCreater creater)
    {
        Name = creater.Name;
        Icon = creater.Icon;
        Cost = creater.Cost;
        _baseDamage = creater.BaseDamage;
        _damageMultiplier = creater.DamageMultiplier;
        _damageMultiplierModifier = _damageMultiplier;
        Damage = _baseDamage;
        IsDamagePerClick = creater.IsDamagePerClick;

        DamageChanged?.Invoke(Damage, _damageMultiplier);
    }

    public event UnityAction<int> LevelChanged;
    public event UnityAction<double, float> DamageChanged;
    public event UnityAction<float> DamageMultiplierChanged;

    public int Level { get; private set; }
    public string Name { get; private set; }
    public Sprite Icon { get; private set; }
    public double Cost { get; private set; }
    public double Damage { get; private set; }
    public bool IsDamagePerClick { get; private set; }
    public bool IsBuyed { get; private set; }

    public void Load(HeroData data)
    {
        Buy(data.Level, data.Cost);
    }

    public void Buy(int _numberOfLevelsForBuy, double newPrice)
    {
        if (IsBuyed == false)
            IsBuyed = true;

        LevelUp(_numberOfLevelsForBuy);
        Cost = newPrice;
    }

    private void LevelUp(int _numberOfLevelsForBuy)
    {
        int levelForMultiplication = 25;
        int levelForMultiplicationSquared = 50;

        for (int i = 0; i < _numberOfLevelsForBuy; i++)
        {
            if (Level > 0)
            {
                Damage += _baseDamage;

                if (IsDamagePerClick == true)
                {
                    _damageMultiplier += _damageMultiplierModifier;
                    DamageMultiplierChanged?.Invoke(_damageMultiplier);
                }
                else
                {
                    if (Level % levelForMultiplicationSquared == 0)
                        Damage *= _damageMultiplier * _damageMultiplier;
                    else if (Level % levelForMultiplication == 0)
                        Damage *= _damageMultiplier;
                }
            }

            Level++;
        }

        LevelChanged?.Invoke(Level);
        DamageChanged?.Invoke(Damage, _damageMultiplier);
    }
}
