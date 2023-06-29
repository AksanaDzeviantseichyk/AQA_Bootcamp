using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ReflectionTaskLibrary
{
    public class CustomConverter
    {
        public string Serialize(object model)
        {
            Type type = model.GetType();
            var serializedString = new StringBuilder();
            if (!IsComplexType(type))
            {
                if (type == typeof(Double))
                {
                    return serializedString.Append(model.ToString().Replace('.', ',')).ToString();
                }
                if (type.IsPrimitive || type == typeof(DayOfWeek))
                {
                    return serializedString.Append(model.ToString()).ToString();
                }
                if (type == typeof(string))
                {
                    return serializedString.Append(model).ToString();
                }
            }
            else
            {
                IList<PropertyInfo> properties = new List<PropertyInfo>(type.GetProperties());
                serializedString.Append("[section.begin]");
                serializedString.Append(Environment.NewLine);
                foreach (PropertyInfo property in properties)
                {
                    string propertyName = property.Name;
                    object propertyValue = property.GetValue(model);
                    if (IsComplexType(property.PropertyType))
                    {
                        
                    }
                    else
                    {
                        serializedString.Append(propertyName);
                        serializedString.Append(" = ");
                        var isNull = propertyValue != null ? serializedString.Append(propertyValue.ToString().Replace('.',',')): serializedString.Append("null");
                        serializedString.Append(Environment.NewLine);
                    }
                }
                serializedString.Append("[section.end]");
                serializedString.Append(Environment.NewLine);
                return serializedString.ToString();
            }
            return serializedString.ToString();
        }

        private static bool IsComplexType(Type type)
        {
            return !type.IsPrimitive && type != typeof(string) && type != typeof(decimal) && type != typeof(DayOfWeek);
        }
    }
}
