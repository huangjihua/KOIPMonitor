using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Kernel
{
    public class RandomSession
    {
         //<summary>
        //隨機產生RoomID, 16Byte
         //</summary>      
        public static string createRoomID()
        {
            RNGCryptoServiceProvider rng = null;
            byte[] random = null;
            try
            {
                //...隨機產生RoomID
                random = new Byte[16];
                rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(random);
                return Convert.ToBase64String(random).Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("\"", "");
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>RandomSession>>createRoomID", ex.Message);
                throw new ArgumentNullException();
            }
            finally
            {
                rng = null;
                random = null;
                //GC.Collect(0);
            }
        }
         //<summary>
         //產生ConnectID ,隨機 []Byte  自己帶整數
         //</summary>      
        public static string createSession(int seed)
        {
            RNGCryptoServiceProvider rng = null;
            byte[] random = null;
            try
            {
                //...隨機產生ConnectID 
                random = new Byte[seed];
                rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(random);             
                return Convert.ToBase64String(random).Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("\"", "");
            }
            catch (Exception ex)
            {
                KConsole.Write(ErrorLevel.Serious, "Kernel>>RandomSession>>getSession", ex.Message);
                throw new ArgumentNullException();
            }
            finally
            {
                rng = null;
                random = null;
                //GC.Collect(0);
            }
        }
    }
}
