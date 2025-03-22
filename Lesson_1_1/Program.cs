namespace Lesson_1_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
        }

        static void Task1()
        {
            Console.Write("Input two numbers through space: ");
            var strNumbers = Console.ReadLine();

            if (strNumbers == null)
            {
                Console.WriteLine("Exit...");
                return;
            }

            var strNumbersArr = strNumbers.Split(" ");
            var firstNumber = int.Parse(strNumbersArr[0]);
            var secondNumber = int.Parse(strNumbersArr[1]);
            Console.WriteLine($"{firstNumber} + {secondNumber} = {firstNumber + secondNumber}");
            Console.WriteLine($"{firstNumber} - {secondNumber} = {firstNumber - secondNumber}");
            Console.WriteLine($"{firstNumber} * {secondNumber} = {firstNumber * secondNumber}");
            Console.WriteLine($"{firstNumber} / {secondNumber} = {(float) firstNumber / secondNumber}");
        }

        static void Task2()
        {
            Console.Write("Enter an integer: ");
            var strNumber = Console.ReadLine();

            if (strNumber == null)
            {
                Console.WriteLine("Exit...");
                return;
            }

            var number = int.Parse(strNumber);

            if (number % 2 == 0) Console.WriteLine("Number is odd");
            else Console.WriteLine("Number is even");
        }

        static void Task3()
        {
            Console.Write("Enter an integer (num > 0): ");
            var strNumber = Console.ReadLine();

            if (strNumber == null)
            {
                Console.WriteLine("Exit...");
                return;
            }

            var number = int.Parse(strNumber);

            if (number < 1) 
            {
                Console.WriteLine("Bad number");
                return;
            }


            for (int i = 1; i < 11; i++)
            {
                Console.WriteLine($"{number} * {i} = {number * i}");
            }
        }
    }
}
