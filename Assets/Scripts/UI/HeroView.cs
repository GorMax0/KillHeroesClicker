using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class HeroView : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private TMP_Text _numberOfLevelsSell;
    [SerializeField] private Button _sellButton;

    private Hero _hero;
    private MultiplierSell _multiplierLevelForSell;

    public event UnityAction<Hero, HeroView> SellButtonClick;

    public Hero Hero => _hero;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(OnButtonClick);
        _hero.LevelChanged -= OnLevelChanged;
        _hero.DamageChanged -= OnDamageChanged;
        _multiplierLevelForSell.MultiplierChanged -= OnMultiplierChanged;
    }

    public void Init(Hero hero, MultiplierSell multiplierLevelForSell)
    {
        _multiplierLevelForSell = multiplierLevelForSell;

        _hero = hero;
        _name.text = hero.Name;
        _icon.sprite = hero.Icon;
        PriceDisplay(_hero.Cost);
        OnDamageChanged(_hero.Damage);
        gameObject.SetActive(false);       

        _hero.LevelChanged += OnLevelChanged;
        _hero.DamageChanged += OnDamageChanged;
        _multiplierLevelForSell.MultiplierChanged += OnMultiplierChanged;
    }

    public void PriceDisplay(double price)
    {
        if (price > 0)
            _price.text = NumericalFormatter.Format(price);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void ChangeInteractableButton(bool isEnabled)
    {
        if (_sellButton.interactable != isEnabled)
            _sellButton.interactable = isEnabled;
    }

    private void OnButtonClick()
    {
        SellButtonClick?.Invoke(_hero, this);
    }

    private void OnLevelChanged(int level)
    {
        if (_level.gameObject.activeSelf == false)
            _level.gameObject.SetActive(true);

        _level.text = "óð. " + level.ToString();
    }

    private void OnDamageChanged(double damage, float multiplier = 0)
    {
        string damageText = NumericalFormatter.Format(damage);

        if (_hero.IsDamagePerClick == true)
            _damage.text = _level.text.Length == 0 ? $"({damageText} ÓÂÍ)" : $"{damageText} ÓÂÍ + {multiplier:P0} îò ÓÂÑ";
        else
            _damage.text = _level.text.Length == 0 ? $"({damageText} ÓÂÑ)" : $"{damageText} ÓÂÑ";
    }

    private void OnMultiplierChanged(int multiplier)
    {
        int defaultMultiplier = 1;

        _numberOfLevelsSell.text = multiplier == defaultMultiplier ? "Óðîâåíü +" : $"x{multiplier}";
    }
}