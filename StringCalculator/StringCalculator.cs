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
        private const string DelimiterDefinitionClosingChar = "\n";
        private readonly IList<string> _delimiters = new List<string> {",", "\n" };

        private const string DelimiterOpeningChar = "[";
        private const string DelimiterClosingChar = "]";

        public int Add(string numbers)
        {
            string cleanedNumbersString = CleanNumbersString(numbers);

            if (string.IsNullOrEmpty(cleanedNumbersString))
                return 0;

            var delimitersAsCharArray = _delimiters.Select(d => d.First()).ToArray();
            var numbersAsStringsList = cleanedNumbersString.Split(delimitersAsCharArray);

            IList<int> numbersList = numbersAsStringsList.Select(int.Parse).ToList();

            if (numbersList.Any(n => n < 0))
            {
                var invalidNumbersList = numbersList.Where(n => n < 0);
                string invalidNumbers = string.Join(",", invalidNumbersList.Select(n => n.ToString()));
                throw new CannotAddNumbersLessThanZeroException(invalidNumbers);
            }

            return numbersList.Where(n => n <= 1000).Sum();
        }

        private string CleanNumbersString(string numbers)
        {
            string cleanedNumbersString = numbers;
            if (numbers.StartsWith(DelimiterDefiner))
            {
                cleanedNumbersString = cleanedNumbersString.Remove(0, DelimiterDefiner.Length);

                cleanedNumbersString = cleanedNumbersString.StartsWith(DelimiterOpeningChar)
                    ? ExtractMultipleDelimiters(cleanedNumbersString)
                    : ExtractSingleDelimiter(cleanedNumbersString);
            }

            if (_delimiters.Any())
            {
                const string newShortDelimiter = "*";
                _delimiters.Add(newShortDelimiter);
                foreach (string delimiter in _delimiters)
                    cleanedNumbersString = cleanedNumbersString.Replace(delimiter, newShortDelimiter);
            }

            return cleanedNumbersString;
        }

        private string ExtractSingleDelimiter(string cleanedNumbersString)
        {
            string newDelimiter;
            if (cleanedNumbersString.StartsWith($"{DelimiterDefinitionClosingChar}{DelimiterDefinitionClosingChar}"))
                newDelimiter = DelimiterDefinitionClosingChar;
            else
            {
                int indexOfDelimiterDefinitionClosingChar = cleanedNumbersString.IndexOf(DelimiterDefinitionClosingChar, StringComparison.Ordinal);
                newDelimiter = cleanedNumbersString.Substring(0, indexOfDelimiterDefinitionClosingChar);
            }

            _delimiters.Add(newDelimiter);

            cleanedNumbersString = cleanedNumbersString.Remove(0, newDelimiter.Length + DelimiterDefinitionClosingChar.Length);
            return cleanedNumbersString;
        }

        private string ExtractMultipleDelimiters(string cleanedNumbersString)
        {
            while (cleanedNumbersString.StartsWith(DelimiterOpeningChar))
            {
                cleanedNumbersString = cleanedNumbersString.Remove(0, DelimiterOpeningChar.Length);
                int indexOfFirstDelimiterClosingChar =
                    cleanedNumbersString.IndexOf(DelimiterClosingChar, StringComparison.Ordinal);
                string definedDelimiter = cleanedNumbersString.Substring(0, indexOfFirstDelimiterClosingChar);
                _delimiters.Add(definedDelimiter);
                cleanedNumbersString = cleanedNumbersString.Remove(0, definedDelimiter.Length + DelimiterClosingChar.Length);
            }

            return cleanedNumbersString.Remove(0, DelimiterDefinitionClosingChar.Length);
        }
    }

    public class CannotAddNumbersLessThanZeroException : Exception
    {
        public CannotAddNumbersLessThanZeroException(string unsupportedValues) 
            : base($"Negative Values Not Supported: {unsupportedValues}")
        {
        }
    }
}
;