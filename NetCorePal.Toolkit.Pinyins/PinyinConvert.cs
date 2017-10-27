﻿using System;
using Microsoft.International.Converters.PinYinConverter;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
namespace NetCorePal.Toolkit.Pinyins
{
    /// <summary>
    /// 拼音转换器
    /// </summary>
    public class PinyinConvert
    {
        /// <summary>
        /// 将字符串转为拼音，多音字会转成多个组合
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="lower">输出小写</param>
        /// <returns></returns>
        public static string[] ToPinyins(string str, bool lower = false)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new string[] { str };
            }
            var results = new List<StringBuilder>() { new StringBuilder() };
            foreach (var chr in str)
            {
                if (CanSkip(chr)) { continue; }

                if (ChineseChar.IsValidChar(chr))
                {
                    var cc = GetChart(chr);

                    AppendPinyin(results, cc.Pinyins);
                }
                else
                {
                    results.ForEach(p => p.Append(chr));
                }
            }
            return results.Select(p => lower ? p.ToString().ToLower() : p.ToString()).ToArray();
        }
        /// <summary>
        /// 转为拼音首字母，存在多音字则返回各种组合
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="lower">是否转为全小写</param>
        /// <returns></returns>
        public static string[] ToPinyinInitials(string str, bool lower = false)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new string[] { str };
            }
            var results = new List<StringBuilder>() { new StringBuilder() };
            foreach (var chr in str)
            {
                if (CanSkip(chr)) { continue; }

                if (ChineseChar.IsValidChar(chr))
                {
                    var cc = GetChart(chr);

                    AppendPinyin(results, cc.Initials);
                }
                else
                {
                    results.ForEach(p => p.Append(chr));
                }
            }
            return results.Select(p => lower ? p.ToString().ToLower() : p.ToString()).ToArray();
        }


        /// <summary>
        /// 获取格式化过的拼音字符串，由 首字母拼音和全拼拼音组成，由<paramref name="separator"/>隔开
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="separator"></param>
        /// <param name="lower">是否转为全小写</param>
        /// <param name="maxLength">输出结果最大长度，默认0,表示不截取;否则将根据maxLength截取数据结果</param>
        /// <returns></returns>
        public static string ToPinyinSearchFomat(string str, string separator = ";", bool lower = false, int maxLength = 0)
        {
            var r = string.Join(separator, ToPinyinInitials(str, lower).Union(ToPinyins(str, lower)));
            if (maxLength <= 0 || r.Length < maxLength)
            {
                return r;
            }
            else
            {
                return r.Substring(0, maxLength);
            }
        }


        #region 私有方法
        static ConcurrentDictionary<char, CachedChar> cache = new ConcurrentDictionary<char, CachedChar>();
        static CachedChar GetChart(char chr)
        {
            if (cache.TryGetValue(chr, out var value))
            {
                return value;
            }
            var v = new CachedChar(chr);
            cache.TryAdd(chr, v);
            return v;
        }


        static bool CanSkip(char chr)
        {
            return false;
        }

        static void AppendPinyin<T>(List<StringBuilder> sb, T[] array)
        {
            var len = sb.Count;


            for (int i = 1; i < array.Length; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    sb.Add(new StringBuilder(sb[j].ToString()));
                }
            }

            for (int j = 0; j < len; j++)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    sb[i * len + j].Append(array[i]);
                }
            }
        }

        #endregion
    }
}