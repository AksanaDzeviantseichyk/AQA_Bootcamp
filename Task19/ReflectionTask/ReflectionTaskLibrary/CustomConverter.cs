using ReflectionTask;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ReflectionTaskLibrary
{
    public class CustomConverter
    {
        public string Serialize(object model)
        {
            StringBuilder serializedString = new StringBuilder();
            Type type = model.GetType();
            if (IsSimpleType(type))
            {
                return serializedString
                    .Append(SimpleSerialize(model))
                    .ToString();
            }
            else
            {
                return serializedString
                    .Append(ComplexSerialize(model, 0))
                    .Remove(serializedString.Length - 2, 2)
                    .ToString();                                 
            }
        }

        private string SimpleSerialize(object model)
        {
            Type type = model.GetType();
            if (type == typeof(Double) || type == typeof(float))
            {
                return model.ToString().Replace('.', ',');
            }
            else 
            {
                return model.ToString();
            }
        }

        private string ComplexSerialize(object obj, int nestedLevel)
        {
            Type type = obj.GetType();
            IEnumerable<PropertyInfo> properties = type.GetProperties();
            var serializedString = new StringBuilder();

            string indent = new string(' ', nestedLevel * 10);
            serializedString.AppendLine($"{indent}[section.begin]");
            foreach (PropertyInfo property in properties)
            {
                if (property.IsDefined(typeof(CustomSerializeAttribute)))
                {
                    CustomSerializeAttribute attribute = property.GetCustomAttribute<CustomSerializeAttribute>();
                    string propertyName = attribute.Name ?? property.Name;
                    object propertyValue = property.GetValue(obj);
                    if (propertyValue != null)
                    {
                        if (!IsSimpleType(property.PropertyType))
                        {
                            serializedString
                                .AppendLine($"{indent}{propertyName} = ")
                                .Append(ComplexSerialize(obj, nestedLevel + 1));
                        }
                        else
                        {
                            serializedString
                                .AppendLine($"{indent}{propertyName} = {SimpleSerialize(propertyValue)}");
                        }
                    }
                }
            }
            serializedString.AppendLine($"{indent}[section.end]");
            return serializedString.ToString();
        }
        
        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DayOfWeek);
        }
    }
}
