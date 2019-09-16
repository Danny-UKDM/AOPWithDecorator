using System;

namespace Decorating
{
    class Program
    {
        static void Main()
        {
            var calculator = MethodProxy<ICalculator>.Create(new Calculator());

            calculator.Add(3, 5);
            calculator.ThrowEx("Bob", "Ross");
            calculator.Add(-5, -13);

            Console.ReadKey();
        }
    }
}
