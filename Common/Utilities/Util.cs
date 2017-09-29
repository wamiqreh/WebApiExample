#region Modificaiton Hisotory
// Created By: Mirza Fahad Ali Baig
// Created On: July3, 2012
// ********************************* Modification History *********************
// Mirza Fahad Ali Baig             July3, 2012                 GetEnumDescription, GetEnumCategory & EnumToList
// Mirza Fahad Ali Baig             September19, 2012           GetEnumDisplayName
// ****************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Data;
using System.ComponentModel;

namespace Common.Utilities
{
    public static class Util
    {
        #region Functions
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo _fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])_fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetEnumCategory(Enum value)
        {
            FieldInfo _fieldInfo = value.GetType().GetField(value.ToString());

            CategoryAttribute[] attributes =
                (CategoryAttribute[])_fieldInfo.GetCustomAttributes(
                typeof(CategoryAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Category;
            else
                return value.ToString();
        }

        public static string GetEnumDisplayName(Enum value)
        {
            FieldInfo _fieldInfo = value.GetType().GetField(value.ToString());

            DisplayNameAttribute[] attributes =
                (DisplayNameAttribute[])_fieldInfo.GetCustomAttributes(
                typeof(DisplayNameAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].DisplayName;
            else
                return value.ToString();
        }

        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;

            // TODO: TO USE ABOVE FUNCTION
            //foreach (States state in EnumToList<States>())
            //{
            //    stateDropDown.Items.Add(GetEnumDescription(state));
            //}
        }

        #endregion
    }
}
