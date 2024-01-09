using System;
using System.Linq;
using System.Drawing;
using IEGOFormationEditor.InazumaEleven.Logic;

namespace IEGOFormationEditor.Tools
{
    public static class EnumHelper
    {
        public static (T Value, string Name)[] GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(value => (Value: value, Name: GetEnumName(value)))
                .ToArray();
        }

        public static string GetEnumName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (NameAttribute)Attribute.GetCustomAttribute(field, typeof(NameAttribute));
            return attribute == null ? value.ToString() : attribute.Name;
        }

        public static string GetShortName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (ShortNameAttribute)Attribute.GetCustomAttribute(field, typeof(ShortNameAttribute));
            return attribute == null ? value.ToString() : attribute.ShortName;
        }

        public static Color GetColorById<T>(int id) where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>();

            foreach (var value in values)
            {
                if ((int)(object)value == id)
                {
                    return GetColor(value);
                }
            }

            throw new ArgumentException($"No enum value with id {id} found.");
        }

        public static Color GetColor<T>(T value) where T : Enum
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (ColorAttribute)Attribute.GetCustomAttribute(field, typeof(ColorAttribute));
            return attribute == null ? Color.Black : attribute.Color;
        }
    }
}
