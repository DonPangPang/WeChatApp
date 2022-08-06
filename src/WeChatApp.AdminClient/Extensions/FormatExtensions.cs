using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WeChatApp.AdminClient.Extensions
{
    public static class FormatExtensions
    {
        public static IEnumerable<EnumItem<T>> ToEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                                        .Cast<T>()
                                        .Select(x => new EnumItem<T> { Label = x.GetDescription(), Value = x })
                                        .ToList();
        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        public static string GetDescription(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo? fd = type.GetField(em.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Description;
            }
            return name;
        }
    }

    public class EnumItem<T>
    {
        public string? Label { get; set; }
        public T? Value { get; set; }
    }
}