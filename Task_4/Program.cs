using Task_4.Part1;
using Task_4.Part2;
using Task_4.Part_3_4;
using Task_4.Part_5;

namespace Task_4
{
    class Program
    {
        static void Main(string[] args)
        {
            String answer;
            do
            {
                Console.WriteLine("Choose:");
                Console.WriteLine("1 - Shapes");
                Console.WriteLine("2 - Alive");
                Console.WriteLine("3 - Fibonacci numbers using a loop");
                Console.WriteLine("4 - Fibonacci numbers using recursion");
                Console.WriteLine("5 - Polynomial");
                Console.WriteLine("6 - Lightstring");
                Console.WriteLine("Enter the number of the operation you want to execute:");

                var numberOfOperation = Console.ReadLine().Trim();
                switch (numberOfOperation)
                {
                    case "1":
                        WorkWithShapes();
                        break;
                    case "2":
                        WorkWithAlive();
                        break;
                    case "3":
                        FibonacciUsingLoop();
                        break;
                    case "4":
                        FibonacciUsingRecursion();
                        break;
                    case "5":
                        WorkWithPolynomial();
                        break;
                    case "6":

                        break;
                    default:
                        Console.WriteLine("Unknown operation");
                        break;
                }
                Console.WriteLine("Do you want to repeat (YES/NO)? If YES enter Y:");
                answer = Console.ReadLine().ToUpper();

            } while (answer == "Y");
        }

        #region case1
        public static void WorkWithShapes()
        {
            Console.WriteLine("Choose a shape:");
            Console.WriteLine("1 - Circle");
            Console.WriteLine("2 - Square");
            Console.WriteLine("3 - Triangle");
            Console.WriteLine("Enter the number of the shape:");

            var numberOfShape = Console.ReadLine().Trim();
            try
            {
                switch (numberOfShape)
                {
                    case "1":
                        Console.WriteLine("Enter the radius of the circle:");
                        var radius = Double.Parse(Console.ReadLine());
                        Circle circle = new Circle(radius);
                        PrintShape(circle);
                        break;
                    case "2":
                        Console.WriteLine("Enter the side length of the square:");
                        var sideLength = Double.Parse(Console.ReadLine());
                        Square square = new Square(sideLength);
                        PrintShape(square);
                        break;
                    case "3":
                        Console.WriteLine("Enter the AB length of the triangle:");
                        var lengthAB = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the BC length of the triangle:");
                        var lengthBC = Double.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the AC length of the triangle:");
                        var lengthAC = Double.Parse(Console.ReadLine());
                        Triangle triangle = new Triangle(lengthAB,lengthBC, lengthAC);
                        if (triangle.IsExists())
                            PrintShape(triangle);
                        else Console.WriteLine("This triangle IS NOT Exists ");
                        break;
                    default:
                        Console.WriteLine("Unknown shape");
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }
        public static void PrintShape(Shape shape)
        {
            Console.WriteLine($"Perimeter: {Math.Round(shape.GetPerimeter(),3)}\nArea: {Math.Round(shape.GetArea(),3)}");
        }
        #endregion

        #region case2
        public static void WorkWithAlive()
        {
            Wolf wolf = new Wolf("Alpha");
            Rabbit rabbit = new Rabbit("Peter");
            Bear bear = new Bear("Yogi");
            Rose rose = new Rose("Red Rose");
            Grass grass = new Grass("Green Grass");

            Console.WriteLine(wolf.GetName());
            Console.WriteLine(rabbit.GetName());
            Console.WriteLine(bear.GetName());
            Console.WriteLine(rose.GetName());
            Console.WriteLine(grass.GetName());

            Console.WriteLine(wolf.GetIndividualName());
            Console.WriteLine(rabbit.GetIndividualName());
            Console.WriteLine(bear.GetIndividualName());

            rabbit.Eat(rose);
            bear.Eat(grass);

            wolf.Eat(rabbit);
            bear.Eat(wolf);
        }
        #endregion

        #region case3
        public static void FibonacciUsingLoop()
        {
            Console.Write("Enter a natural number: ");
            try
            {
                int number = int.Parse(Console.ReadLine());
                Fibonacci fibonacci = new Fibonacci(number);
                fibonacci.FibonacciSeries();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        #endregion

        #region case4
        public static void FibonacciUsingRecursion()
        {
            Console.Write("Enter a natural number: ");
            try
            {
                int number1 = int.Parse(Console.ReadLine());
                Fibonacci fibonacci1 = new Fibonacci(number1);
                Console.Write($"Fibonacci numbers up to {number1}: ");
                fibonacci1.FibonacciSeries(0, 1);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            Console.WriteLine();
        }
        #endregion

        #region case5
        public static void WorkWithPolynomial()
        {
            Polynomial p1 = new Polynomial(-3, 0, 4, 0, 0, 5, 0, -12);
            Polynomial p2 = new Polynomial(2, -1, 3, 0, 4);

            Console.WriteLine($"First polynomial:\np1 = {p1}");
            Console.WriteLine($"Second polynomial:\np2 = {p2}");
            Console.WriteLine($"An example of adding two polynomials:\np1 + p2 = {p1 + p2}");
            Console.WriteLine($"An example of subtracting two polynomials:\np1 - p2 = {p1 - p2}");
            Console.WriteLine($"An example of multiplying two polynomials:\np1 * p2 = {p1 * p2}");
        }
        #endregion
    }

}
