namespace Lesson_1_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Task4();
        }

        static void Task1()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine("Numbers");
            foreach (int num in numbers) Console.WriteLine(num);

            Console.WriteLine();
        }
        static void Task2()
        {
            List<int> numbersList = new List<int> { 1, 2, 3, 4, 5 };

            Console.WriteLine("Numbers");
            foreach (int num in numbersList) Console.WriteLine(num);

            Console.WriteLine();

            numbersList.Add(6);
            Console.WriteLine("Added 6. Numbers");
            foreach (int num in numbersList) Console.WriteLine(num);

            Console.WriteLine();

            numbersList.Remove(1);
            Console.WriteLine("Removed 1. Numbers");
            foreach (int num in numbersList) Console.WriteLine(num);

            Console.WriteLine();

            Dictionary<String, String> emailsDict = new Dictionary<string, string> { { "John", "coolJohn@abc.com" }, { "Oleg", "oleg@cba.ru" } };
            foreach (KeyValuePair<String, String> kvp in emailsDict) Console.WriteLine($"key: {kvp.Key}; value: {kvp.Value}");

            Console.WriteLine();
        }

        static void Task3()
        {
            var numbersList = new List<int> { 1, 2, 3, 4, 5 };

            var oddNumbers = numbersList.Where(num => num % 2 == 0);
            Console.WriteLine("Odd numbers in List");
            foreach (int num in oddNumbers) Console.WriteLine(num);

            Console.WriteLine();

            Console.WriteLine("Descending ordered list");
            var orderedNumbers = numbersList.OrderBy( n => n);
            foreach (int num in orderedNumbers) Console.WriteLine(num);

            Console.WriteLine();

            Console.WriteLine($"List sum: {orderedNumbers.Sum()}");

            Console.WriteLine();

            Console.Write("Input integer that exist in list: ");
            var filterNumStr = Console.ReadLine();
            if (!int.TryParse(filterNumStr, out var filterNum))
            {
                Console.WriteLine("Exit...");
            } else if (!(numbersList.Contains(filterNum)))
            {
                Console.WriteLine("Number not in list");
            }

            var customFilterList = numbersList.Where(num => num > filterNum);
            Console.WriteLine("Filtered list:");
            foreach (int num in customFilterList) Console.WriteLine(num);

            Console.WriteLine();

        }
        static void Task4()
        {
            var cities = new List<String> { "Красноярск", "Канск", "Дудинка", "Норильск" };
            var kStartCities = cities.Where(city => city.StartsWith("К"));
            Console.WriteLine("Cities that starts on \"К\" letter:");
            foreach (string city in kStartCities) Console.WriteLine(city);

            Console.WriteLine();

            var alphabeticalSortedCities = cities.OrderBy(city => city);
            Console.WriteLine("Alphabetically sorted cities:");
            foreach (string city in alphabeticalSortedCities) Console.WriteLine(city);

            Console.WriteLine();

            var largerThanFiveCities = cities.Where(city => city.Length > 5);
            Console.WriteLine("Cities with length more than 5 symbols:");
            foreach(string city in largerThanFiveCities) Console.WriteLine(city);

        }
    }
}
