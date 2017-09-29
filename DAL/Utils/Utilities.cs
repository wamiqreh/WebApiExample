using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Utils
{
   public static class Utilities
    {

        //

        public static string GenerateProperTableName(string query) {
            string ReturnQry = query;
            NameValueCollection test = (NameValueCollection)ConfigurationManager.GetSection("CustomTableName/Tablenames");
            foreach (var item in test)
            {

                ReturnQry = ReplaceCaseInsensitive(ReturnQry, item.ToString(), test[item.ToString()]);
            }
            return ReturnQry;
        }
        private static string ReplaceCaseInsensitive(string input, string search, string replacement)
        {
            string result = Regex.Replace(
                input,
                Regex.Escape(search),
                replacement.Replace("$", "$$"),
                RegexOptions.IgnoreCase
            );
            return result;
        }
    }
}
