using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Hero
{
    private double _baseDamage;    
    private float _damageMultiplierModifier;

    public Hero(HeroCreater creater, int id)
    {
        Id = id;
        Name = creater.Name;
        Icon = creater.Icon;
        Cost = creater.Cost;
        _baseDamage = creater.BaseDamage;
        DamageMultiplier = creater.DamageMultiplier;
        _damageMultiplierModifier = DamageMultiplier;
        Damage = _baseDamage;
        IsDamagePerClick = creater.IsDamagePerClick;

        DamageChanged?.Invoke(Damage, DamageMultiplier);        
    }

    public event UnityAction<int> LevelChanged;
    public event UnityAction<double, float> DamageChanged;
    public event UnityAction<float> DamageMultiplierChanged;

    public int Id { get; private set; }
    public int Level { get; private set; }
    public string Name { get; private set; }
    public Sprite Icon { get; private set; }
    public double Cost { get; private set; }
    public double Damage { get; private set; }
    public float DamageMultiplier { get; private set; }
    public bool IsDamagePerClick { get; private set; }
    public bool IsBuyed { get; private set; }

    public void Load(HeroData data)
    {
        Id = data.Id;
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
                    DamageMultiplier += _damageMultiplierModifier;
                    DamageMultiplierChanged?.Invoke(DamageMultiplier);
                }
                else
                {
                    if (Level % levelForMultiplicationSquared == 0)
                    {
                        float ratioDamageMultiplierModifier = 0.3f;

                        _baseDamage *= DamageMultiplier * DamageMultiplier;
                        Damage *= DamageMultiplier * DamageMultiplier;
                        DamageMultiplier += ratioDamageMultiplierModifier;
                    }
                    else if (Level % levelForMultiplication == 0)
                    {
                        float ratioDamageMultiplierModifier = 0.1f;

                        Damage *= DamageMultiplier;
                        DamageMultiplier += ratioDamageMultiplierModifier;
                    }
                }
            }

            Level++;
        }

        LevelChanged?.Invoke(Level);
        DamageChanged?.Invoke(Damage, DamageMultiplier);
    }
}
