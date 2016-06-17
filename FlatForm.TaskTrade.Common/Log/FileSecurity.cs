using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Peacock.Common.IO.Log
{
    /// <summary>
    /// 对文件字符串进行加密和解密的算法。
    /// </summary>
    public class FileSecurity
    {
        private const string DEFAULTKEY = "SOFTBEY"; //默认加密和解密密钥，系统运行后不能更改该参数的值

        /// <summary>
        /// 使用本系统默认的密钥加密
        /// </summary>
        /// <param name="strToEncrypt">被加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string strToEncrypt)
        {
            return Encrypt(strToEncrypt, DEFAULTKEY);
        }

        /// <summary>   
        /// DEC 加密过程   
        /// </summary>   
        /// <param name="strToEncrypt">被加密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥）</param>   
        /// <returns>加密后的字符串</returns>   
        public static string Encrypt(string strToEncrypt, string key)
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象   
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量   
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法   

            byte[] inputByteArray = Encoding.Default.GetBytes(strToEncrypt);//把字符串放到byte数组中   

            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　   
            //定义将数据流链接到加密转换的流   
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //上面已经完成了把加密后的结果放到内存中去   

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// 使用该系统默认的密钥解密
        /// </summary>
        /// <param name="strToDecrypt">被解密的字符串</param>
        /// <returns>返回被解密的字符串</returns>
        public static string Decrypt(string strToDecrypt)
        {
            return Decrypt(strToDecrypt, DEFAULTKEY);
        }

        /// <summary>   
        /// DEC解密过程
        /// </summary>   
        /// <param name="strToDecrypt">被解密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥，与前面的加密密钥相同）</param>   
        /// <returns>返回被解密的字符串</returns>   
        public static string Decrypt(string strToDecrypt, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[strToDecrypt.Length / 2];
            for (int x = 0; x < strToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(strToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量，此值重要，不能修改   
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象   
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

    }
}
