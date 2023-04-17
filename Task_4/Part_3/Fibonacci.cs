using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4.Part_3
{
    public class Fibonacci
    {
        private int _number;
        public Fibonacci(int number) 
        { 
            _number = number;
        }
         public void FibonacciSeries()
        {
            int first = 0;
            int second = 1;
            int next = 0;

            Console.Write("Fibonacci numbers up to " + _number + ": ");
            Console.Write($"{first} {second} ");

            while (next <= _number)
            {
                next = first + second;
                if (next <= _number)
                {
                    Console.Write(next + " ");
                }
                first = second;
                second = next;
            }
            Console.WriteLine();
        }

        public void FibonacciSeries(int beforePreviousValue, int previousValue)
        {
            if (previousValue <= _number)
            {
                Console.Write($"{previousValue} ");
                int currentValue = beforePreviousValue + previousValue;
                FibonacciSeries(previousValue, currentValue);
            }
        }
    }
}
