using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Source.UtilitClasses
{
    public class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            string str = phrase.ToLowerInvariant();

            // Remove invalid characters
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one space
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // Cut and trim if too long
            str = str[..Math.Min(100, str.Length)].Trim();

            // Replace spaces with hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }
    }
}
