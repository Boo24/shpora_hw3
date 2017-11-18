using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace ControlDigit
{
	public static class ControlDigitExtensions
	{
		public static int ControlDigit(this long number)
		{
			int sum = 0;
			int factor = 1;
			do
			{
				int digit = (int)(number % 10);
				sum += factor * digit;
				factor = 4 - factor;
				number /= 10;

			}
			while (number > 0);

			int result = sum % 11;
			if (result == 10)
				result = 1;
			return result;
		}

		public static int ControlDigit2(this long number)
		{
		    var sum= number.GetAllDigits().GetElemsOnOddPositions().Sum()+
	            number.GetAllDigits().GetElemsOnEvenPositions().Sum() * 3;
		    return sum%11==10? 1 : sum%11;
        }

	    
	}

    public static class Extensions
    {
        public static IEnumerable<int> GetAllDigits(this long number)
        {
            do
            {
                yield return GetLastDigit(number);
                number /= 10;
            }
            while (number > 0);
        }

        public static int GetLastDigit(this long num) => (int) (num % 10);

        public static IEnumerable<T> GetEverySecondDigit<T>(this IEnumerable<T> collection, bool isOdd)
        {
            foreach (var elem in collection)
            {
                if (isOdd)
                {
                    yield return elem;
                    isOdd = false;
                }
                else
                    isOdd = true;
            }
        }
        public static IEnumerable<T> GetElemsOnEvenPositions<T>(this IEnumerable<T> collection)
        {
            return GetEverySecondDigit(collection, false);
        }
        public static IEnumerable<T> GetElemsOnOddPositions<T>(this IEnumerable<T> collection)
        {
            return GetEverySecondDigit(collection, true);
        }


    }

	[TestFixture]
	public class ControlDigitExtensions_Tests
	{
		[TestCase(0, ExpectedResult = 0)]
		[TestCase(1, ExpectedResult = 1)]
		[TestCase(2, ExpectedResult = 2)]
		[TestCase(9, ExpectedResult = 9)]
		[TestCase(10, ExpectedResult = 3)]
		[TestCase(15, ExpectedResult = 8)]
		[TestCase(17, ExpectedResult = 1)]
		[TestCase(18, ExpectedResult = 0)]
		public int TestControlDigit(long x)
		{
			return x.ControlDigit();
		}

		[Test]
		public void CompareImplementations()
		{
			for (long i = 0; i < 100000; i++)
				Assert.AreEqual(i.ControlDigit(), i.ControlDigit2());
		}
	}

	[TestFixture]
	public class ControlDigit_PerformanceTests
	{
		[Test]
		public void TestControlDigitSpeed()
		{
			var count = 10000000;
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < count; i++)
				12345678L.ControlDigit();
			Console.WriteLine("Old " + sw.Elapsed);
			sw.Restart();
			for (int i = 0; i < count; i++)
				12345678L.ControlDigit2();
			Console.WriteLine("New " + sw.Elapsed);
		}
	}
}
