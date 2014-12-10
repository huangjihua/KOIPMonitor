using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Commonality
{
    public static class Encrypt
    {
        public static string key;
        public static string Key
        {
            get { return key; }
            set
            {
                key = value;
            }
        }
        ///MD5加密
        ///
        //const string sKey = "lxw88888";
        /// <summary>
        /// 加密//public static string MD5Encrypt(string pToEncrypt, string sKey)
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string pToEncrypt)
        {
            try
            {
                string sKey = "is988888";
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
            }
            catch (Exception ex)
            {
                return pToEncrypt;
            }
        }

        /// <summary>
        /// 解密//public static string MD5Decrypt(string pToDecrypt, string sKey)
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <returns></returns>
        public static string MD5Decrypt(string pToDecrypt)
        {
            try
            {
                string sKey = "is988888";

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return pToDecrypt;
            }
        }

    }


}
