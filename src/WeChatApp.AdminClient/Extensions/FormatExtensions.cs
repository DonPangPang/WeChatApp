using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatApp.AdminClient.Extensions
{
    public static class FormatExtensions
    {
        public static IEnumerable<EnumItem<T>> ToEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                                        .Cast<T>()
                                        .Select(x => new EnumItem<T> { Label = x.ToString(), Value = x })
                                        .ToList();
        }
    }

    public class EnumItem<T>
    {
        public string? Label { get; set; }
        public T? Value { get; set; }
    }
}