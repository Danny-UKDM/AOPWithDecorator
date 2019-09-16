using System;

namespace Decorating
{
    public class Calculator : ICalculator
    {
        public int Add(int a, int b) => a + b;

        public void ThrowEx(string a, string b)
        {
            throw new Exception($"Error when handling {a} and {b}", new Exception($"I am an inner {a} Exception"));
        }
    }
}
