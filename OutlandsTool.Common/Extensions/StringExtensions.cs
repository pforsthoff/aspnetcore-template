using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OutlandsTool.Common.Extensions
{
    public static class StringExtensions
    {

        public static bool ContainsHtml(this string text)
        {
            return Regex.IsMatch(
                text, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
        }

        public static string EnsureEndsWith(this string str, string value)
        {
            if (!str.EndsWith(value))
            {
                return str + value;
            }

            return str;
        }

        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static string Repeat(this string value, int count)
        {
            return new StringBuilder().Insert(0, value, count).ToString();
        }

        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }
        public static string DomainLess(this string value)
        {
            return value.Substring(value.LastIndexOf("\\") + 1);
        }

        public static string AppendUniqueCode(this string value, int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var ticks = DateTime.UtcNow.Ticks.ToString();
            var code = "";
            for (var i = 0; i < characters.Length; i += 2)
            {
                if ((i + 2) > ticks.Length) continue;
                var number = int.Parse(ticks.Substring(i, 2));
                if (number > characters.Length - 1)
                {
                    var one = double.Parse(number.ToString().Substring(0, 1));
                    var two = double.Parse(number.ToString().Substring(1, 1));
                    code += characters[Convert.ToInt32(one)];
                    code += characters[Convert.ToInt32(two)];
                }
                else
                    code += characters[number];
            }

            var result = value + " " + code;
            return result;
        }
    }
}
