using FluentAssertions;
using NUnit.Framework;

namespace StringCalculator
{
    public class StringCalculatorTests
    {
        private readonly IStringCalculator _stringCalculator;

        public StringCalculatorTests()
        {
            _stringCalculator = new StringCalculator();
        }

        [Test]
        public void AddMethodReturnsZeroForEmptyString()
        {
            int result = _stringCalculator.Add("");

            result.Should().Be(0);
        }

        [Test]
        public void AddMethodReturnsValueOfNumber()
        {
            int result = _stringCalculator.Add("7");

            result.Should().Be(7);
        }

        [Test]
        public void AddMethodReturnsValueOfSummedNumber()
        {
            int result = _stringCalculator.Add("7,1");

            result.Should().Be(8);
        }

        [Test]
        public void AddMethodReturnsSumOfUnknownAmountOfNumbers()
        {
            int result = _stringCalculator.Add("5,48,5,18,1,5,7");

            result.Should().Be(89);
        }

        [Test]
        public void AddMethodReturnsSumOfNumbersWithNewLineDelimiter()
        {
            int result = _stringCalculator.Add("7\n1");

            result.Should().Be(8);
        }

        [Test]
        public void AddMethodReturnsSumOfNumbersWithNewLineOrCommaDelimiter()
        {
            int result = _stringCalculator.Add("1,7\n1,8");

            result.Should().Be(17);
        }

        [Test]
        public void AddMethodShouldDeclareDelimiter()
        {
            int result = _stringCalculator.Add("//;\n1;2");

            result.Should().Be(3);
        }

        [Test]
        public void AddMethodShouldThrowExceptionOnNegatives()
        {
            _stringCalculator.Invoking(y => y.Add("-1,-2"))
                .Should().Throw<CannotAddNumbersLessThanZeroException>()
                .WithMessage("Negative Values Not Supported: -1,-2");
        }

        [Test]
        public void AddMethodShouldIgnoreNumbersGreaterThan1000()
        {
            int result = _stringCalculator.Add("1001,2");

            result.Should().Be(2);
        }

        [Test]
        public void AddMethodShouldDeclareDelimiterOfAnyLength()
        {
            int result = _stringCalculator.Add("//THISISTOTESANORMALTHING\n1THISISTOTESANORMALTHING2");

            result.Should().Be(3);
        }

        [Test]
        public void AddMethodShouldDeclareMultipleDelimiterOfAnyLength()
        {
            int result = _stringCalculator.Add("//[THISISTOTESANORMALTHING][***]\n1THISISTOTESANORMALTHING2***45");

            result.Should().Be(48);
        }
    }
}
