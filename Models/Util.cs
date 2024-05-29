using System.Reflection;

namespace Docteur.Models
{
    public static class Util
    {


        public static string upperedName(string input)
        {
            char firstChar = input[0];
            string restOfString = input.Substring(1);

            return char.ToUpper(firstChar) + restOfString;
        }

        public static String scriptInsertColumns(List<string> columnNames, string automatic)
        {
            String ans = "";

            for (int i = 0; i < columnNames.Count; i++)
            {
                if (String.Compare(columnNames[i], automatic, StringComparison.OrdinalIgnoreCase) == 0)
                {

                }
                else
                {
                    ans += columnNames[i];
                    if (i != columnNames.Count - 1)
                    {
                        ans += ",";
                    }
                }
            }

            return ans;
        }
    }
}
