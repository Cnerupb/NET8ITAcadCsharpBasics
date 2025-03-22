using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lesson_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input user name: ");
            var userName = Console.ReadLine();
            Console.WriteLine($"Hello, {userName}!");
        }
    }
}
