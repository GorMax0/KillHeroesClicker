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
        CreateData();
        StartCoroutine(SaveOverTime());
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            CreateData();
            _saveSystem.Save(_progressData);
        }
        else if (_saveSystem != null)
        {
            _progressData = _saveSystem.Load();
        }
    }

    private void CreateData()
    {
        double money = _player.Money;
        List<Hero> heroes = _player.GetHeroes();
        List<HeroView> views = _heroShop.GetViews();
        int level = _levelHandler.CurrentLevel;

        _progressData = new SaveData(money, heroes, views, level);
    }

    private IEnumerator SaveOverTime()
    {
        float timeDelayInSeconds = 60;
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeDelayInSeconds);

        while (true)
        {
            _saveSystem.Save(_progressData);

            yield return waitForSeconds;
        }
    }
}

//public static class ProgressSaver
//{
//    public static void Save<T>(string key, T saveData)
//    {
//        string jsonDataString = JsonUtility.ToJson(saveData);
//        PlayerPrefs.SetString(key, jsonDataString);
//    }

//    public static T Load<T>(string key) where T: new()
//    {
//        if (PlayerPrefs.HasKey(key))
//        {
//            string loadedString = PlayerPrefs.GetString(key);

//            return JsonUtility.FromJson<T>(loadedString);
//        }
//        else
//        {
//            return new T();
//        }
//    }
//}