using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace WeChatApp.Shared.Extensions
{
    /// <summary>
    /// </summary>
    public static class FormatExtensions
    {
        // public static bool IsEmpty(this string str)
        // {
        //     return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        // }

        public static bool IsEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public static bool IsEmpty(this DateTime? dt)
        {
            return dt is null || !dt.HasValue || dt.Equals(default(DateTime));
        }

        public static bool IsEmpty(this DateTime dt)
        {
            return dt.Equals(default(DateTime));
        }

        public static bool IsEmpty<T>(this T obj) where T : class, new()
        {
            return obj is null || obj.Equals(default(T));
        }

        public static bool IsEmpty<T>(this IEnumerable<T> lst)
        {
            return lst is null || !lst.Any();
        }

        public static bool IsEmpty<T>(this List<T> lst)
        {
            return lst is null || !lst.Any();
        }

        public static bool IsEmpty(this Guid id)
        {
            return id.Equals(Guid.Empty);
        }

        public static bool IsEmpty(this Guid? id)
        {
            return id is null || id.IsEmpty();
        }

        public static bool IsEmpty(this bool? value)
        {
            return value is null;
        }

        public static int ToInt(this string value)
        {
            try
            {
                Int32.TryParse(value, out var result);
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static int ToInt<T>(this T value) where T : Enum
        {
            return value.GetHashCode();
        }

        public static bool ToBool(this int value)
        {
            return value != 0;
        }

        public static Guid ToGuid(this string value)
        {
            Guid.TryParse(value, out var result);
            return result;
        }

        public static Decimal ToDecimal(this string value)
        {
            Decimal.TryParse(value, out var result);
            return result;
        }

        public static long ToLong(this string value)
        {
            long.TryParse(value, out var result);
            return result;
        }

        public static DateTime ToDateTime(this string value)
        {
            DateTime.TryParse(value, out var result);
            return result;
        }

        public static DateTime? ToDateTimeOrNull(this string value)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch { return null; }
        }

        public static int DateDiff(this DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.Days;
        }

        // 两个日期相差的小时数
        public static int HourDiff(this DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.Hours;
        }

        // 两个日期相差的分钟数
        public static int MinuteDiff(this DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.Minutes;
        }

        // 两个日期相差的秒数
        public static int SecondDiff(this DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.Seconds;
        }

        //将字符串转为指定类型的值
        public static T ConvertTo<T>(this object value, T defaultValue = default(T))
        {
            if (value is null)
                return defaultValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                if (typeof(T) == typeof(Guid))
                    return (T)Convert.ChangeType(Guid.Parse(value.ToString() ?? ""), typeof(T));

                if (typeof(T) == typeof(DateTime))
                    return (T)Convert.ChangeType(DateTime.Parse(value.ToString() ?? ""), typeof(T));

                if (typeof(T).IsEnum)
                    return (T)Convert.ChangeType(Enum.Parse(typeof(T), value.ToString() ?? ""), typeof(T));

                return defaultValue;
            }
        }
    }
}

#pragma warning restore 1591