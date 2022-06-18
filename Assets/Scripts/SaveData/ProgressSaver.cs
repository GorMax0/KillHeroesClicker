using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressSaver : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private HeroShop _heroShop;
    [SerializeField] private LevelHandler _levelHandler;

    private BinarySave _saveSystem;
    private SaveData _progressData;

    private void Start()
    {
        _saveSystem = new BinarySave();
        LoadDate();
        StartCoroutine(SaveOverTime());
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
            SaveData();
    }

    private void SaveData()
    {
        double money = _player.Money;
        int level = _levelHandler.CurrentLevel;
        bool previousLevelComplete = _levelHandler.IsBossLevel ? false : true;

        _progressData = new SaveData();
        _progressData.SetPlayerData(_player.GetHeroes(), money);
        _progressData.SetLevelData(level, previousLevelComplete);
        _saveSystem.Save(_progressData);
        Debug.Log("Save!");
    }

    private void LoadDate()
    {
        if ((_progressData = _saveSystem.Load()) == null)
            return;

        LoadPlayerData();
        _levelHandler.LoadLevel(_progressData.CurrentLevel, _progressData.PreviousLevelComplete);

        Debug.Log("Load!");
    }

    private void LoadPlayerData()
    {
        List<HeroCreater> heroCreaters = _heroShop.GetHeroCreaters();
        List<HeroData> data = _progressData.GetHeroesData();
        List<Hero> heroes = new List<Hero>();

        foreach (HeroData hero in data)
        {
            for (int i = 0; i < heroCreaters.Count; i++)
            {
                if (hero.Name.Equals(heroCreaters[i].Name))
                {
                    heroes.Add(new Hero(heroCreaters[i], hero.Id));
                    continue;
                }
            }
        }

        _player.LoadData(heroes, _progressData.Money);

        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].Load(data[i]);
        }

        _heroShop.LoadBuyedHeroes(heroes);
    }

    private IEnumerator SaveOverTime()
    {
        float timeDelayInSeconds = 180;
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeDelayInSeconds);

        while (true)
        {
            yield return waitForSeconds;
            SaveData();
        }
    }
}