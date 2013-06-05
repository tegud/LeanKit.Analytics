using System.Collections;

namespace LeanKit.Utilities.Collections
{
    public static class HastableExtensions
    {
        public static string GetValue(this Hashtable configurationSection, string key)
        {
            return (string)configurationSection[key];
        }
    }
}