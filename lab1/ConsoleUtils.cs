using System;
using System.Text; 

namespace Utils
{
    static class ConsoleInput
    {

        public static string GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }

        public static string GetInputOnText(string str) {
            Console.WriteLine(str);
            return Console.ReadLine();
        }
    }
}