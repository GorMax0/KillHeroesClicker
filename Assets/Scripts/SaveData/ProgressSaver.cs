using System.Collections;
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

        _progressData = new SaveData(money, level, previousLevelComplete);
        _saveSystem.Save(_progressData);
        Debug.Log("Save!");
    }

    private void LoadDate()
    {
        if ((_progressData = _saveSystem.Load()) == null)
            return;

        _player.AddMoney(_progressData.Money);
        _levelHandler.LoadLevel(_progressData.CurrentLevel, _progressData.PreviousLevelComplete);
        Debug.Log("Load!");
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