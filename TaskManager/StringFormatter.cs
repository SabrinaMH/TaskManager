using System;

namespace TaskManager
{
    public static class StringFormatter
    {
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("input cannot be null or empty", "input");
            
            string lower = input.ToLower();
            char capitalizedFirstLetter = char.ToUpper(lower[0]);
            string result = string.Concat(capitalizedFirstLetter, lower.Substring(1));
            return result;
        }
    }
}