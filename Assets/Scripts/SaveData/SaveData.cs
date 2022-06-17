using System;

[Serializable]
public class SaveData
{
    public double Money { get; private set; }
    public int CurrentLevel { get; private set; }
    public bool PreviousLevelComplete { get; private set; }

    public SaveData(double money, int level, bool previousLevelComplete)
    {
        Money = money;
        CurrentLevel = level;
        PreviousLevelComplete = previousLevelComplete;
    }
}
