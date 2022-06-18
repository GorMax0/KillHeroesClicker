[System.Serializable]
public class HeroData
{
    public int Id { get; private set; }
    public int Level { get; private set; }
    public string Name { get; private set; }
    public double Cost { get; private set; }

    public HeroData(int id, int level, string name, double cost)
    {
        Id = id;
        Level = level;
        Name = name;
        Cost = cost;
    }
}
