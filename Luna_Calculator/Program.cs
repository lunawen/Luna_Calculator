using System;
using System.Text.RegularExpressions;


namespace Luna_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Calculator");
            Console.WriteLine("Please input first number");
            Console.WriteLine("Enter equition and hit enter to get the result");
            Console.WriteLine("E.g., 1+2");
            var equition = Console.ReadLine();
            // Todo: validate equition
            
            // 1. remove all the spaces
            equition = Regex.Replace(equition, " ", "");

            // 2. find the operators and numbers
            string pattern = @"[0-9\(]+[\+\-\*\/]+[0-9\)]+";
            Regex rgx = new Regex(pattern);
            bool isEquitionValid = rgx.IsMatch(equition);
            
            // Do the calculation


            // Return result
            Console.WriteLine(equition);       
            Console.WriteLine(isEquitionValid);       
             
        }
    }

    enum Operators
    {
        add,
        substract,
        multiply,
        divide
    }
}
