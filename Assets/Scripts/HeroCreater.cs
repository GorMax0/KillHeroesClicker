using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Gameplay/Hero", order = 51)]
public class HeroCreater : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private double _cost;
    [SerializeField] private double _baseDamage;
    [SerializeField] private float _damageMultiplier;
    [SerializeField] private bool _isDamagePerClick;

    public string Name => _name;
    public Sprite Icon => _icon;
    public double Cost => _cost;
    public double BaseDamage => _baseDamage;
    public float DamageMultiplier => _damageMultiplier;
    public bool IsDamagePerClick => _isDamagePerClick;
}