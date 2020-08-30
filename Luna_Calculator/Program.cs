using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Luna_Calculator {
    class Program {

         static void Main (string[] args) {
            Console.WriteLine ("Welcome to Calculator");
            Console.WriteLine ("Enter equition and hit enter to get the result");
            Console.WriteLine ("E.g., 1+2");
            var equition = Console.ReadLine ();

            // 1. remove all the spaces
            equition = Regex.Replace (equition, "[ =\t\n]", "");
            string invalidCharacters = @"[^\d\+\-\*\^\%\/\(\)]";

            // 2. validate equition
            Regex regex = new Regex (invalidCharacters);
            bool isValid = !regex.IsMatch (equition);

            // 3. determine calculation priority
            // separate the equition by ()
            Regex rgx = new Regex (@"\([^\(\)]*\)");

            foreach (Match match in rgx.Matches (equition)) {
                string e = Regex.Replace(match.Value, @"[\(\)]", "");
                double intermediateResult = GetEquitionResult (e);
                Console.WriteLine (intermediateResult);
            }

            // ToDo：unwrap more ()
            // e.g., (1+2+(3+4+(5+6))) + 1 

            // ToDo: without ()
            if(!rgx.IsMatch(equition)){
                double finalResult = GetEquitionResult (equition);
                Console.WriteLine (finalResult);
            }

            // Todo: allow user to continue calculation (current result = x)
        }

        private static double GetEquitionResult (string e) {
            string operatorsRegex = @"[\+\-\*\^\%\/]";
            Regex rgx = new Regex (operatorsRegex);
            List<string> numbers = new List<string> (rgx.Split (e).Where(s => s != String.Empty));
            foreach (string s in numbers) {
                Console.WriteLine (s);
            }
            string numberRegex = @"\d+?\.?\d*?";
            Regex numRgx = new Regex (numberRegex);
            List<string> operators = new List<string> (numRgx.Split (e).Where(s => s != String.Empty));
            foreach (string s in operators) {
                Console.WriteLine (s);
            }
            if (numbers.Count == operators.Count + 1){
                return Calculate (numbers, operators);
            }
            else return -1;

        }

        public static double Calculate (List<string> numbers, List<string> operators) {
            double result = 0;
            // calculate highest priority
            while (operators.Contains ("^")) {
                int i = operators.IndexOf ("^");
                double leftNum;
                double rightNum;
                double.TryParse (numbers[i], out leftNum);
                double.TryParse (numbers[i + 1], out rightNum);
                result += Math.Pow (leftNum, rightNum);
                operators.RemoveAt (i); // remove this operator
                numbers.RemoveRange (i, 2); //remove the calculated number
                numbers.Insert (i, result.ToString ()); // add result in the numbers list
            }

            while (operators.Contains ("*") || operators.Contains ("/")) {
                int i = operators.IndexOf ("*");
                int j = operators.IndexOf ("/");
                if ((i < j && i != -1) || j == -1) { // multiply first
                    double leftNum;
                    double rightNum;
                    double.TryParse (numbers[i], out leftNum);
                    double.TryParse (numbers[i + 1], out rightNum);
                    result += leftNum * rightNum;
                    operators.RemoveAt (i); // remove this operator
                    numbers.RemoveRange (i, 2); //remove the calculated number
                    numbers.Insert (i, result.ToString ()); // add result in the numbers list
                } else if ((j < i && j != -1) || i == -1)  { // divide first
                    double leftNum;
                    double rightNum;
                    double.TryParse (numbers[j], out leftNum);
                    double.TryParse (numbers[j + 1], out rightNum);
                    result += leftNum / rightNum;
                    operators.RemoveAt (j); // remove this operator
                    numbers.RemoveRange (j, 2); //remove the calculated number
                    numbers.Insert (j, result.ToString ()); // add result in the numbers list
                }
            }

            while (operators.Contains ("+") || operators.Contains ("-")) {
                int i = operators.IndexOf ("+");
                int j = operators.IndexOf ("-");
                if ((i < j && i != -1) || j == -1) { // Add first
                    double leftNum;
                    double rightNum;
                    double.TryParse (numbers[i], out leftNum);
                    double.TryParse (numbers[i + 1], out rightNum);
                    result += leftNum + rightNum;
                    operators.RemoveAt (i); // remove this operator
                    numbers.RemoveRange (i, 2); //remove the calculated number
                    numbers.Insert (i, result.ToString ()); // add result in the numbers list
                } else if ((j < i && j != -1) || i == -1) { // Substract first
                    double leftNum;
                    double rightNum;
                    double.TryParse (numbers[j], out leftNum);
                    double.TryParse (numbers[j + 1], out rightNum);
                    result += leftNum - rightNum;
                    operators.RemoveAt (j); // remove this operator
                    numbers.RemoveRange (j, 2); //remove the calculated number
                    numbers.Insert (j, result.ToString ()); // add result in the numbers list
                }
            }

            // result should be equal to the number in List now
            if (numbers.Count == 1 && operators.Count == 0) {
                Console.WriteLine (numbers[0]);
                Console.WriteLine (result);
            }

            return result;
        }
    }
}