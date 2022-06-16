using System;

[Serializable]
public class SaveData
{
    public double Money { get; private set; }
    public int CurrentLevel { get; private set; }
    public bool BossKilled { get; private set; }

    public SaveData(double money, int level, bool bossKilled)
    {
        Money = money;
        CurrentLevel = level;
        BossKilled = bossKilled;
    }
}
