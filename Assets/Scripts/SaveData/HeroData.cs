[System.Serializable]
public class HeroData
{
    public int Level { get; private set; }
    public string Name { get; private set; }
    public double Cost { get; private set; }

    public HeroData(int level, string name, double cost)
    {
        Level = level;
        Name = name;
        Cost = cost;
    }
}
