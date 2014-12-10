using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.IO.Compression;

namespace Kernel
{
    public class Tools
    {
        private static int OSType = 0;//操作系统类型值{0:Linux;1:Windows}

        /// <summary>
        /// 操作系统类型值{0:Linux;1:Windows}
        /// </summary>
        public static int OSTYPE
        {
            get { return OSType;}
            set { OSType = value; }
            
        }
        public static byte[] Serialize(StateObject obj)
        {
            try
            {
                using (MemoryStream ms_save_SerializePackage = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms_save_SerializePackage, obj);
                    return ms_save_SerializePackage.ToArray();
                }                
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>Serialize", ex.Message);
                return null;
            }
        }

        public static StateObject Deserialize(byte[] _serializeBuffe)
        {
            try
            {
                using (MemoryStream ms_open_SerializePackage = new MemoryStream(_serializeBuffe))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (StateObject)formatter.Deserialize(ms_open_SerializePackage);
                }             
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>Deserialize", ex.Message);
                return null;
               
            }
        }

        public static string Createdir(string directoryName)
        {
            try
            {
                if (OSType != 0)
                {
                    directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + directoryName;

                }
                else
                {
                    directoryName = "./" + directoryName;
                }

                DirectoryInfo Createdir = new DirectoryInfo(directoryName);
                if (!Createdir.Exists)
                {
                    Createdir.Create();
                }
                return directoryName;
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>Createdir", ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 將String轉成ASCII bytes 轉成 Base64 Chars
        /// </summary>       
        public static char[] ToBase64Chars(string srcData)
        {
            try
            {    
                char[] charData = Convert.ToBase64String(Encoding.ASCII.GetBytes(srcData)).ToCharArray();
                return charData;
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Response,"", "Kernel>>Tools>>ToBase64Chars>>Exception :"+ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 將String轉成ASCII bytes 轉成 Base64 String
        /// </summary>       
        public static string ToBase64String(string srcData)
        {
            try
            {              
                return Convert.ToBase64String(Encoding.ASCII.GetBytes(srcData));
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Response, "","Kernel>>Tools>>ToBase64String>>Exception:"+ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 將Base64 Chars 還原ASCII bytes  還原 String
        /// </summary>       
        public static string FromBase64Chars(char[] srcData)
        {
            try
            {
                return Encoding.ASCII.GetString(Convert.FromBase64CharArray(srcData, 0, srcData.Length));
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>FromBase64Chars>>", ex.Message);
                return null;
            }
        }


        /// <summary>
        ///將來源字串 轉成 UTF8
        /// </summary>        
        public static Byte[] ToUTF8byte(string srcData, Encoding srcEncoding)
        {
            try
            {
                Byte[] _srcBytes = srcEncoding.GetBytes(srcData);
                Byte[] _dstBytes = Encoding.Convert(srcEncoding, Encoding.UTF8, _srcBytes);
                return _dstBytes;
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>ToUTF8>>", ex.Message);
                return null;
            }
        }

        /// <summary>
        ///將來源字串 轉成 UTF32
        /// </summary>        
        public static Byte[] ToUTF32byte(string srcData, Encoding srcEncoding)
        {
            try
            {
                Byte[] _srcBytes = srcEncoding.GetBytes(srcData);
                Byte[] _dstBytes = Encoding.Convert(srcEncoding, Encoding.UTF32, _srcBytes);
                return _dstBytes;
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>ToUTF32>>", ex.Message);
                return null;
            }
        }

        /// <summary>
        ///將來源字串 轉成 指定編碼
        /// </summary>        
        public static Byte[] StringEncodingConver(string srcData, Encoding srcEncoding, Encoding tgaEncoding)
        {
            try
            {
                Byte[] _srcBytes = srcEncoding.GetBytes(srcData);
                Byte[] _dstBytes = Encoding.Convert(srcEncoding, tgaEncoding, _srcBytes);
                return _dstBytes;
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>StringEncodingConver>>", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gzip壓縮
        /// </summary>        
        public static int GzipCompress(ref byte[] srcBuffer, ref  byte[] cmpBuffer)
        {
            string _CompressSaveFilePath = null;
            try
            {
                string directoryName = "";
                if (OSType != 0)
                {
                     directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\Compress\\";

                }
                else
                {
                     directoryName = "./" + "Compress/";
                }

                DirectoryInfo Createdir = new DirectoryInfo(directoryName);
                if (!Createdir.Exists)
                {
                    Createdir.Create();
                }

                _CompressSaveFilePath = directoryName + RandomSession.createSession(64);

                using (FileStream fs_saveCompress = new FileStream(_CompressSaveFilePath, FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream ZipStream = new GZipStream(fs_saveCompress, CompressionMode.Compress, true))
                    {
                        ZipStream.Write(srcBuffer, 0, srcBuffer.Length);
                    }
                }

                using (FileStream fs_open = new FileStream(_CompressSaveFilePath, FileMode.Open, FileAccess.Read))
                {
                    cmpBuffer = new byte[Convert.ToInt32(fs_open.Length)];
                    fs_open.Read(cmpBuffer, 0, Convert.ToInt32(fs_open.Length));
                }

                return 1;
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>GzipCompress>>", ex.Message);
                return 0;
            }
            finally
            {
                DiskIO.Del(_CompressSaveFilePath);
            }
        }

        /// <summary>
        /// Gzip 解壓縮
        /// </summary>     
        public static int Decompress(byte[] _srcBuffer, ref  byte[] deCmpBuffer)
        {
            string _DecompressSaveFilePath = null;
            try
            {

                string directoryName = "";
                if (OSType != 0)
                {
                     directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\Compress\\";

                }
                else
                {
                    directoryName = "./" + "Compress/";
                }


                DirectoryInfo Createdir = new DirectoryInfo(directoryName);
                if (!Createdir.Exists)
                {
                    Createdir.Create();
                }

                _DecompressSaveFilePath = directoryName + RandomSession.createSession(64);


                using (MemoryStream ms_open = new MemoryStream(_srcBuffer))
                {
                    using (GZipStream zipStream = new GZipStream(ms_open, CompressionMode.Decompress))
                    {
                        using (FileStream fs_saveDecompress = new FileStream(_DecompressSaveFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fs_saveDecompress))
                            {
                                int offset = 0;
                                int BufferSize = 512;
                                int bytesRead = 0;
                                byte[] buffer = new byte[BufferSize];
                                while (true)
                                {
                                    bytesRead = zipStream.Read(buffer, offset, BufferSize);
                                    if (bytesRead == 0)
                                    {
                                        break;
                                    }
                                    bw.Write(buffer, 0, bytesRead);
                                }
                            }
                        }


                        using (FileStream fs_open = new FileStream(_DecompressSaveFilePath, FileMode.Open, FileAccess.Read))
                        {
                            deCmpBuffer = new byte[Convert.ToInt32(fs_open.Length)];
                            fs_open.Read(deCmpBuffer, 0, Convert.ToInt32(fs_open.Length));
                        }
                       
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                ///KConsole.Write(ErrorLevel.Serious, "Kernel>>Tools>>Decompress>>", ex.Message);
                Console.Write("Kernel>>Tools>>Decompress>>" + ex.Message);
                return 0;
            }
            finally
            {           
                DiskIO.Del(_DecompressSaveFilePath);

            }
        }


        //public static int Decompress(byte[] _Sourcebuffer, ref string _returnTargetFilePath)
        //{
            //string strDecompressFilePath = "";
            //try
            //{
            //    //string strDecompressdir = Config.GetAppSettingsValue("DeCompressDirectoryString");
            //    //strDecompressFilePath = strDecompressdir + RandomSession.getSession(64);

            //    //DirectoryInfo Decompressdir = new DirectoryInfo(strDecompressdir);
            //    //if (!Decompressdir.Exists)
            //    //{
            //    //    Decompressdir.Create();
            //    //}

            //    string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\Compress\\";

            //    DirectoryInfo Createdir = new DirectoryInfo(directoryName);
            //    if (!Createdir.Exists)
            //    {
            //        Createdir.Create();
            //    }

            //    strDecompressFilePath = directoryName + RandomSession.createSession(64);


            //    using (MemoryStream ms_open = new MemoryStream(_Sourcebuffer))
            //    {
            //        using (GZipStream zipStream = new GZipStream(ms_open, CompressionMode.Decompress))
            //        {
            //            using (FileStream fs_saveDecompress = new FileStream(strDecompressFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            //            {
            //                using (BinaryWriter bw = new BinaryWriter(fs_saveDecompress))
            //                {
            //                    int offset = 0;
            //                    int BufferSize = 512;
            //                    int bytesRead = 0;
            //                    byte[] buffer = new byte[BufferSize];
            //                    while (true)
            //                    {
            //                        bytesRead = zipStream.Read(buffer, offset, BufferSize);
            //                        if (bytesRead == 0)
            //                        {
            //                            _returnTargetFilePath = strDecompressFilePath;
            //                            break;
            //                        }
            //                        bw.Write(buffer, 0, bytesRead);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    return 1;
            //}
            //catch (Exception ex)
            //{
            //    ConsoleManage.WriteToConsole(0, "KoIPServerLibrary>>Package>>Decompress>>", ex.Message);
            //    if (!string.IsNullOrEmpty(strDecompressFilePath))
            //        ThreadPool.QueueUserWorkItem(new WaitCallback(Package.Delete_File), strDecompressFilePath);
            //    return 0;
            //}
        //}

    }
}
