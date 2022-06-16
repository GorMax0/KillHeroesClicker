public static class NumericalFormatter
{
    public static string Format(double number)
    {
        string numberToString = number.ToString();
        int indexSubstringChar = numberToString.IndexOf(',');
        numberToString = numberToString.Substring(0, indexSubstringChar < 0 ? numberToString.Length : indexSubstringChar);
        

        if (numberToString.Length < 6)
            return numberToString;
        else
            return number.ToString("#.###e0");
    }
}
