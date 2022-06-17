using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    private List<HeroData> _heroesData = new List<HeroData>();

    public double Money { get; private set; }
    public int CurrentLevel { get; private set; }
    public bool PreviousLevelComplete { get; private set; }

    public List<HeroData> GetHeroesData()
    {
        List<HeroData> heroesData = new List<HeroData>();

        heroesData.AddRange(_heroesData);
        return heroesData;
    }

    public void SetPlayerData(List<Hero> heroes, double money)
    {
        foreach (var hero in heroes)
        {
            _heroesData.Add(new HeroData(hero.Level, hero.Name, hero.Cost));
        }

        Money = money;
    }

    public void SetLevelData(int level, bool previousLevelComplete)
    {
        CurrentLevel = level;
        PreviousLevelComplete = previousLevelComplete;
    }
}
