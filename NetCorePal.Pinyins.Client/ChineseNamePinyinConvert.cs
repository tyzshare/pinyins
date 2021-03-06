﻿using System;
using System.IO;
using System.Net;
using System.Text;

namespace NetCorePal.Pinyins.Client
{
    /// <summary>
    /// 中文名称拼音转换
    /// </summary>
    public class ChineseNamePinyinConvert
    {
        //请求的地址
        private string _url;
        public ChineseNamePinyinConvert(string url)
        {
            _url = url;
        }
        /// <summary>
        /// 获取姓名拼音
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns>转换后的拼音</returns>
        public string GetChineseNamePinYin(string name)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(_url + "/api/PinYin?name=" + name);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
