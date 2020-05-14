using System;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculator
{
    public interface IStringCalculator
    {
        public int Add(string numbers);
    }

    public class StringCalculator : IStringCalculator
    {
        private const string DelimiterDefiner = "//";
        private readonly IList<string> _delimiters = new List<string> {",", "\n" };

        public int Add(string numbers)
        {
            string cleanedNumbersString = numbers;
            if (numbers.StartsWith(DelimiterDefiner))
            {
                cleanedNumbersString = cleanedNumbersString.Remove(0, DelimiterDefiner.Length);

                string newDelimiter;
                if (cleanedNumbersString.StartsWith("\n\n"))
                    newDelimiter = "\n";
                else
                {
                    int indexOfFirstNewLine = cleanedNumbersString.IndexOf("\n", StringComparison.Ordinal);
                    newDelimiter = cleanedNumbersString.Substring(0, indexOfFirstNewLine);
                }

                int indexOfCommaDelimiter = _delimiters.IndexOf(",");
                _delimiters.RemoveAt(indexOfCommaDelimiter);
                _delimiters.Add(newDelimiter);

                cleanedNumbersString = cleanedNumbersString.Remove(0, newDelimiter.Length + 1);
            }


            if (string.IsNullOrEmpty(cleanedNumbersString))
                return 0;

            var delimitersAsCharArray = _delimiters.Select(d => d.First()).ToArray();
            var numbersAsStringsList = cleanedNumbersString.Split(delimitersAsCharArray);

            return numbersAsStringsList.Select(int.Parse).Sum();
        }
    }
}
;