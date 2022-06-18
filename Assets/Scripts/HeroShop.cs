using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroShop : MonoBehaviour
{
    [SerializeField] private List<HeroCreater> _heroCreaters = new List<HeroCreater>();
    [SerializeField] private Player _player;
    [SerializeField] private HeroView _template;
    [SerializeField] private ItemContainer _container;
    [SerializeField] private MultiplierSell _multiplierSell;

    private double[] _sumPrices;
    private List<HeroView> _views = new List<HeroView>();
    private int _index = 0;
    private float _multiplierPrice = 1.1f;
    private int _numberOfLevelsForSale;

    private void Awake()
    {
        int indexFirstView = 0;

        _sumPrices = new double[_heroCreaters.Count];

        for (_index = 0; _index < _heroCreaters.Count; _index++)
        {
            AddHero(new Hero(_heroCreaters[_index], _index));
            CalculateSumPrice(_views[_index]);
        }

        _index = indexFirstView;
        _views[_index].Enable();
    }

    private void OnEnable()
    {
        _player.MoneyChanged += OnMoneyChanged;
        _multiplierSell.MultiplierChanged += OnMultiplierChanged;
    }

    private void OnDisable()
    {
        _player.MoneyChanged -= OnMoneyChanged;
        _multiplierSell.MultiplierChanged -= OnMultiplierChanged;

        foreach (var view in _views)
        {
            view.SellButtonClick -= OnSellButtonClick;
        }
    }

    public List<HeroCreater> GetHeroCreaters()
    {
        List<HeroCreater> heroCreaters = new List<HeroCreater>();

        heroCreaters.AddRange(_heroCreaters);
        return heroCreaters;
    }

    public void LoadBuyedHeroes(List<Hero> loadedHeroes)
    {
        foreach (var loadedHero in loadedHeroes)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                if (loadedHero.Id.Equals(_views[i].Hero.Id))
                {
                    
                    _views[i].Refresh(loadedHero);
                    _views[i+1].Enable();
                    continue;
                }
            }
        }

        CalculateSumPrices();
        DisplayNextHero(_player.Money);
    }

    private void AddHero(Hero hero)
    {
        bool isEnable = false;

        HeroView view = Instantiate(_template, _container.transform);
        view.SellButtonClick += OnSellButtonClick;

        view.Init(hero, _multiplierSell);
        view.ChangeInteractableButton(isEnable);
        _views.Add(view);
    }

    private bool CanSell(double price)
    {
        return _player.Money >= price;
    }

    private void CalculateSumPrice(HeroView view)
    {
        double tempPrice = view.Hero.Cost;
        int index = _views.IndexOf(view);

        for (int number = _numberOfLevelsForSale; number > 0; number--)
        {
            if (number == _numberOfLevelsForSale)
            {
                _sumPrices[index] = tempPrice;
            }
            else
            {
                tempPrice = Math.Ceiling(tempPrice * _multiplierPrice);
                _sumPrices[index] += Math.Ceiling(tempPrice);
            }
        }
    }

    private void CalculateSumPrices()
    {
        foreach (var view in _views)
        {
            CalculateSumPrice(view);
        }
    }

    private void ChangeInteractableSellButtons()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            _views[i].ChangeInteractableButton(CanSell(_sumPrices[i]));
        }
    }

    private bool IsEnoughMoney(double money)
    {
        return money >= _heroCreaters[_index].Cost;
    }

    private void DisplayNextHero(double money)
    {
        if (_index + 1 < _heroCreaters.Count && IsEnoughMoney(money) == true)
        {
            _index++;

            if (_index < _views.Count)
                _views[_index].Enable();
        }
        Debug.Log(_index);
    }

    private double GetNewCost(HeroView view)
    {
        double newCost = view.Hero.Cost;

        for (int i = 0; i < _numberOfLevelsForSale; i++)
        {
            newCost = Math.Ceiling(newCost * _multiplierPrice);
        }

        return newCost;
    }

    private void OnMultiplierChanged(int multiplier)
    {
        _numberOfLevelsForSale = multiplier;
        CalculateSumPrices();

        for (int i = 0; i < _views.Count; i++)
        {
            _views[i].PriceDisplay(_sumPrices[i]);
        }

        ChangeInteractableSellButtons();
    }

    private void OnMoneyChanged(double playerMoney)
    {
        DisplayNextHero(playerMoney);
        ChangeInteractableSellButtons();
    }

    private void OnSellButtonClick(Hero hero, HeroView heroView)
    {
        int index = _views.IndexOf(heroView);
        double newCost = GetNewCost(heroView);

        if (CanSell(_sumPrices[index]))
        {
            _player.BuyHero(hero, _sumPrices[index]);
            hero.Buy(_numberOfLevelsForSale, newCost);
            CalculateSumPrice(heroView);
            heroView.PriceDisplay(_sumPrices[index]);
            heroView.ChangeInteractableButton(CanSell(_sumPrices[index]));
        }
    }
}
