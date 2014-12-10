using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Kernel
{
    public class DiskIO
    {
        public static void Save(string filePath,byte[] buffer,int count)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    //KConsole.Write(ErrorLevel.Warn, "Kernel>>DiskIO>>Save", "filePath is null or empty");
                    Console.WriteLine("Kernel>>DiskIO>>Save"+"filePath is null or empty");
                    throw new ArgumentNullException();
                }

                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(buffer, 0, count);
                        bw.Flush();                    
                    }

                }               
            }
            catch (Exception ex)
            {
                //KConsole.Write(ErrorLevel.Serious, "Kernel>>DiskIO>>Save", ex.Message);
                Console.WriteLine("Kernel>>DiskIO>>Save"+ex.Message);
                throw new ArgumentNullException();
            }
        }
       
        public static void SaveToMemory(char[] srcBuffer,ref byte[] targetBuffer, Encoding encoding)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(ms, encoding))
                    {
                        sw.Write(srcBuffer);
                        sw.Flush();
                    }
                    targetBuffer = ms.ToArray();
                }                
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>DiskIO>>SaveToMemory", ex.Message);
            }
        }

        public static void SaveSerialize(string filePath, byte[] buffer, int count)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    KConsole.Write(ErrorLevel.Warn, "Kernel>>DiskIO>>SaveSerialize", "filePath is null or empty");
                    throw new ArgumentNullException();
                }

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(buffer, 0, count);
                        bw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>DiskIO>>SaveSerialize", ex.Message);
                throw new ArgumentNullException();
            }
        }

        public static void Del(object _obj)
        {
            try
            {
                if (_obj == null)
                {
                    KConsole.Write(ErrorLevel.Serious, "Kernel>>DiskIO>>Del", "_obj無效");
                    return;
                }

                string _filepath = (string)_obj;
                FileInfo file = new FileInfo(_filepath);
                if (file.Exists)
                {
                    file.Delete();                    
                }
                file = null;
                //GC.Collect(0);
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>DiskIO>>Save", ex.Message);
                //throw new ArgumentNullException();
            }
        }
    }
}
