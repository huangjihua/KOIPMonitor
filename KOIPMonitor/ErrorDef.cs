using System;
using System.Collections.Generic;
using System.Text;

namespace KOIPMonitor
{
    public class ErrorDef
    {


        #region 系统类
        /// <summary>
        /// 无KIS 服务器
        /// </summary>
        public const short NoKIS = -101;
        /// <summary>
        /// 无KNS 服务器
        /// </summary>
        public const short NoKNS = -102;
        /// <summary>
        /// 会话ID 已存在。
        /// </summary>
        public const short SessionExist = -103;

        /// <summary>
        /// 与KMS 连接断开，找不到KMS
        /// </summary>
        public const short KMSDiscon = -104;
        /// <summary>
        /// 与KIS 连接断开，找不到KIS
        /// </summary>
        public const short KISDiscon = -105;

        /// <summary>
        /// 与KNS 连接断开，找不到KNS
        /// </summary>
        public const short KNSDiscon = -106;
        /// <summary>
        /// 与KDS 连接断开，找不到KDS
        /// </summary>
        public const short KDSDiscon = -107;
        /// <summary>
        /// 与数据库服务器连接断开，找不到数据库
        /// </summary>
        public const short DBDiscon = -108;




        #endregion

        #region 基础数据类


        #endregion

        #region 业务处理类

        /// <summary>
        /// 用户名错误
        /// </summary>
        public const short ErrUserID = -10001;
        /// <summary>
        /// 密码错误
        /// </summary>
        public const short ErrPassword = -10002;
        /// <summary>
        /// 加入群组，密码验证错误
        /// </summary>
        public const short ErrGroupPassword = -10003;

        /// <summary>
        /// 加入群组，被拒绝
        /// </summary>
        public const short ErrGroupDenial = -10004;
        /// <summary>
        /// 进入包厢，密码验证错误
        /// </summary>
        public const short ErrRoomPassWord = -105;


        #endregion


    }

    public class ErrKNS
    {
        /// <summary>
        /// 联系人不在线
        /// </summary>
        public const short UserUnOnLine = -105;
    }

    public class ErrCommon
    {
        #region 通用公共
        /// <summary>
        /// 成功
        /// </summary>
        public const short Success = 0;
        /// <summary>
        /// 未知
        /// </summary>
        public const short Unknown = -1;
        /// <summary>
        /// 文件太大无法传送
        /// </summary>
        public const short FileLong = -2;


        #endregion
    }

    public class ErrSongPlay
    {
        /// <summary>
        /// 未找到MIC文件
        /// </summary>
        public const short ErrNoMIC = -5001;

        /// <summary>
        /// 查无歌曲记录
        /// </summary>
        public const short ErrNoSongRecord = -5002;

        /// <summary>
        /// 没有MIDI属性
        /// </summary>
        public const short ErrNoMICProperty = -5003;

        /// <summary>
        /// 没有找到歌手记录
        /// </summary>
        public const short ErrNoSingerRecord = -5004;


    }
}
