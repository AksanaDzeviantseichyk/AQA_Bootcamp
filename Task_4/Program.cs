using System;
using System.Globalization;
using Task_4.Part_1;

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
                Console.WriteLine("2 - Animals");
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
                        
                        break;
                    case "3":
                        
                        break;
                    case "4":
                        
                        break;
                    case "5":

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
    }

}
