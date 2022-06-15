using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    private double _money;    
    private List<Hero> _heroes;
    private List<HeroView> _views;
    private int _currentLevel;

    public SaveData(double money, List<Hero> heroes, List<HeroView> views, int level)
    {
        _money = money;
        _heroes = heroes;   
        _views = views;
        _currentLevel = level;
    }
}
