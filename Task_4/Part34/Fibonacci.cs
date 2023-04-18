
namespace Task_4.Part34
{
    public class Fibonacci
    {
        private int number;
        public Fibonacci(int number) 
        { 
            this.number = number;
        }
         public void FibonacciSeries()
        {
            int first = 0;
            int second = 1;
            int next = 0;

            Console.Write("Fibonacci numbers up to " + number + ": ");
            Console.Write($"{first} {second} ");

            while (next <= number)
            {
                next = first + second;
                if (next <= number)
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
            if (previousValue <= number)
            {
                Console.Write($"{previousValue} ");
                int currentValue = beforePreviousValue + previousValue;
                FibonacciSeries(previousValue, currentValue);
            }
        }
    }
}
