using System;
using System.Text.RegularExpressions;

namespace PGRating.Crawler.Utilities
{
    public class ParseUtilities
    {
        public static string GetValueFromUrl(string query, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var regex = new Regex($"^.*{key}=(.*?)&");

            var match = regex.Match(query);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        public static string GetTagContent(string value, string tag = null)
        {
            return GetFromTagByRegex(value, new Regex($"^(?:<{tag}.*?>)(.*)(?:</{tag}>)"));
        }

        public static string GetTagAttributes(string value, string tag = null)
        {
            return GetFromTagByRegex(value, new Regex($"^<{tag}\\s+(.*?)>"), tag);
        }

        public static int GetInt(string value)
        {
            // TODO: add logging for exceptions
            return int.Parse(value);
        }

        private static string GetFromTagByRegex(string value, Regex regex, string tag = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            value = Trim(value);

            if (string.IsNullOrEmpty(tag))
            {
                tag = ExtractTag(value);

                if (string.IsNullOrEmpty(tag))
                {
                    return null;
                }
            }

            var match = regex.Match(value);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        private static string Trim(string value)
        {
            return value.Trim().Replace("&nbsp;","");
        }

        private static string ExtractTag(string value)
        {
            var tagRegex = new Regex(@"^<([a-zA-Z]+)\s");
            var tagMatch = tagRegex.Match(value);

            return tagMatch.Success ? tagMatch.Groups[0].Value : null;
        }
    }
}
