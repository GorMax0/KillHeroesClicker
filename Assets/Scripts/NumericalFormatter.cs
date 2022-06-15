public static class NumericalFormatter
{
    public static string Format(double number)
    {
        if (number.ToString().Length < 6)
            return number.ToString();
        else
            return number.ToString("#.###e0");
    }
}
