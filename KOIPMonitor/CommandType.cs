using System;
using System.Collections.Generic;
using System.Text;

namespace KOIPMonitor
{
    #region 服务器类型
    /// <summary>
    /// 服务器类型
    /// </summary>
    public class ServType
    {
        /// <summary>
        /// KMS类型{1}
        /// </summary>
        public const int KMS = 1;
        /// <summary>
        /// KIS类型{2}
        /// </summary>
        public const int KIS = 2;
        /// <summary>
        /// KNS类型{3}
        /// </summary>
        public const int KNS = 3;
        /// <summary>
        /// KDS类型{4}
        /// </summary>
        public const int KDS = 4;
    }
    #endregion

    #region 服务器通用指令
    /// <summary>
    /// 服务器通用指令
    /// </summary>
    public class CommCmd
    {
        /// <summary>
        /// 协议版本
        /// </summary>
        public const byte MsgVer = 1;

        /// <summary>
        /// 服务器注册请求
        /// </summary>
        public const short RegServer = 11;
        /// <summary>
        /// 握手请求
        /// </summary>
        public const short Handshake = 1018;

        /// <summary>
        /// 网速测试cmd=1000
        /// </summary>
        public const short wirespeed = 1000;

        /// <summary>
        /// 退出系统{CS-->KIS;CS-->KNS}
        /// </summary>
        public const short UserExit = 10;

        /// <summary>
        /// 时间同步
        /// </summary>
        public const short TimeSyn = 4501;

 

    }
    #endregion

    #region KMS服务指令
    /// <summary>
    /// KMS服务指令
    /// </summary>
    public class KMSCmd
    {
        /// <summary>
        /// 获取KIS列表信息 CMD=1;
        /// [1]
        /// </summary>
        public const short Getkislist = 01;
    }
    #endregion

    #region KIS服务指令
    /// <summary>
    /// KIS服务指令
    /// </summary>
    public class KISCmd
    {
        /// <summary>
        /// 获取KNS列表信息 CMD=2;{CS-->KIS}
        /// [02]
        /// </summary>
        public const short GetkNslist = 02;

        /// <summary>
        /// 获取用户个人信息{CS-->KIS}
        /// [500]
        /// </summary>
        public const short GetPersonInfo = 500;

        /// <summary>
        /// 获取好友分组信息{CS-->KIS}
        /// [501]
        /// </summary>
        public const short GetFriendTypeInfo = 501;

        /// <summary>
        /// 获取用户好友列表{CS-->KIS}
        /// [502]
        /// </summary>
        public const short GetFriendList = 502;

        /// <summary>
        /// 更新个人信息{CS-->KIS}
        /// [503]
        /// </summary>
        public const short UpdatePersonInfo = 503;

        /// <summary>
        /// 删除好友{CS-->KIS}
        /// [505]
        /// </summary>
        public const short DelFriend = 505;

        /// <summary>
        /// 更新好友分组
        /// [509]
        /// </summary>
        public const short UpdateFriendTypeID = 509;

        /// <summary>
        /// 用户动态注册{CS-->KIS}
        /// [529]
        /// </summary>
        public const short UserReg = 529;

        /// <summary>
        /// 查看好友信息{CS-->KIS}
        /// [530]
        /// </summary>
        public const short ViewFriendInfo = 530;

        /// <summary>
        /// 获取本机用户列表
        /// [532]
        /// </summary>
        public const short getMactionUser = 532;
        /// <summary>
        /// 用户注册时取消删除预注册帐号信息
        /// [533]
        /// </summary>
        public const short DeleteUserByRegUser = 533;
        /// <summary>
        /// 用户阻止
        /// [534]
        /// </summary>
        public const short StopUser = 534;
        /// <summary>
        /// 删除被阻止的用户
        /// [535]
        /// </summary>
        public const short DeleteStopUser = 535;
        /// <summary>
        /// 登录KIS密码验证{CS-->KIS}
        /// [1001]
        /// </summary>
        public const short UserLogin = 1001;

        /// <summary>
        /// 查找好友
        /// [1004]
        /// </summary>
        public const  short FindFriend = 1004;

        /// <summary>
        /// 随机搓和
        /// [1033]
        /// </summary>
        public const  short RandomRuband = 1033;

        /// <summary>
        /// 添加好友
        /// [531]
        /// </summary>
        public const short AddFriend = 531;
        /// <summary>
        /// 通用密码验证
        /// [2501]
        /// </summary>
        public const short checkpassword = 2501;

        /// <summary>
        /// 增加我的歌本信息
        /// [2502]
        /// </summary>
        public const short AddMySongList = 2502;
        /// <summary>
        /// 删除我的歌本信息
        /// [2503]
        /// </summary>
        public const short DelMySongList = 2503;
        /// <summary>
        /// 获取我的歌本信息
        /// [2504]
        /// </summary>
        public const short GetMySongList = 2504;
        /// <summary>
        /// 增加历史歌单信息
        /// [2505]
        /// </summary>
        public const short AddHistorySongList = 2505;
        /// <summary>
        /// 删除历史歌单信息
        /// [2506]
        /// </summary>
        public const short DelHistorySongList = 2506;
        /// <summary>
        /// 获取历史歌单信息
        /// [2507]
        /// </summary>
        public const short GetHistorySongList = 2507;

    }
    #endregion

    #region KNS服务指令
    /// <summary>
    /// KNS服务指令
    /// </summary>
    public class KNSCmd
    {
        /// <summary>
        /// 用户注册至KNS{CS-->KNS}
        /// [3]
        /// </summary>
        public const short UserRegKNS = 3;

        /// <summary>
        /// 更新用户登录位置信息{KNS-->KIS}
        /// [4]
        /// </summary>
        public const short UpdateUserLoginLoc = 4;

        /// <summary>
        /// 状态信息通知请求{CS-->KNS-->CS}
        /// [5]
        /// </summary>
        public const short UserStateUpdate = 5;
        /// <summary>
        /// 退出包厢
        /// [9]
        /// </summary>
        public const short ExitRoom = 9;
        /// <summary>
        /// 获取当前节点在线用户ID{KNS-->KIS}
        /// [12]
        /// </summary>
        public const short getusernodeonline = 12;
        /// <summary>
        /// 邀请加为好友请求{CS-->KNS-->CS}
        /// [1005]
        /// </summary>
        public const short RequestFriend = 1005;

        /// <summary>
        /// 邀请加为好友回应{CS-->KNS-->CS}
        /// [1035]
        /// </summary>
        public const short RspFriend = 1035;

        /// <summary>
        /// 点对点音频传输请求
        /// [1008]
        /// </summary>
        public const short RequestAUDIO = 1008;

        /// <summary>
        /// 点对点音频传输回应
        /// [1036]
        /// </summary>
        public const short RspAUDIO = 1036;

        /// <summary>
        /// 点对点音频传输通知请求
        /// [1009]
        /// </summary>
        public const short RequestNoticeAUDIO = 1009;

        /// <summary>
        /// 点对点音频传输通知回应
        /// [1037]
        /// </summary>
        public const short RspNoticeAUDIO = 1037;

        /// <summary>
        /// 点对点视频频传输请求
        /// [1010]
        /// </summary>
        public const short RequestVIDEO = 1010;

        /// <summary>
        /// 点对点视频频传输回应
        /// [1038]
        /// </summary>
        public const short RspVIDEO = 1038;

        /// <summary>
        /// 点对点视频传输通知请求
        /// [1011]
        /// </summary>
        public const short RequestNoticeVIDEO = 1011;

        /// <summary>
        /// 点对点视频传输通知回应
        /// [1039]
        /// </summary>
        public const short RspNoticeVIDEO = 1039;

        /// <summary>
        /// Video port 心跳包
        /// [1014]
        /// </summary>
        public const short VideoPortHand = 1014;

        /// <summary>
        /// Audio port 心跳包
        /// [1015]
        /// </summary>
        public const short AudioPortHand = 1015;

        /// <summary>
        /// 发送道具
        /// [1016]
        /// </summary>
        public const short Sendprop = 1016;

        /// <summary>
        /// 追加歌曲至排歌单
        /// [1027]
        /// </summary>
        public const short SuperaddSingSongList = 1027;

        /// <summary>
        /// 通知唱歌
        /// [1030]
        /// </summary>
        public const short NoticeSingSong = 1030;

        /// <summary>
        /// 切歌
        /// [1031]
        /// </summary>
        public const short ChangeSong = 1031;

        /// <summary>
        /// 邀请唱歌请求
        /// [1034]
        /// </summary>
        public const short RequestSingSong = 1034;

        /// <summary>
        /// 邀请唱歌回应
        /// [1040]
        /// </summary>
        public const short RspSingSong = 1040;

        /// <summary>
        /// 包厢ID通知
        /// [1041]
        /// </summary>
        public const short RequestRoomID = 1041;

        /// <summary>
        /// 获取点对点音视频端口请求
        /// [1042]
        /// </summary>
        public const short ReqGetAVPort = 1042;

        /// <summary>
        /// 获取点对点音视频端口回应
        /// [1043]
        /// </summary>
        public const short RspGetAVPort = 1043;

        /// <summary>
        /// 点对点音视频端口通知
        /// [1044]
        /// </summary>
        public const short NoticeAVPort = 1044;

        /// <summary>
        /// 点对点音视频请求取消
        /// [3501]
        /// </summary>
        public const short AudioVideoReqCancel = 3501;

        /// <summary>
        /// 点对点音视频结束
        /// [3502]
        /// </summary>
        public const short AudioVideoStop = 3502;

        /// <summary>
        /// 用户退出上传至KIS
        /// [3503]
        /// </summary>
        public const short UserExitToKIS = 3503;

        /// <summary>
        /// 包厢销毁通知KNSCS
        /// [3504]
        /// </summary>
        public const short RoomLogout = 3504;
        /// <summary>
        /// 用户退出或非正常断线服务器通知相同包厢用户，此人已离线。
        /// [3505]
        /// </summary>
        public const short RoomUserLogout = 3505;
        
        /// <summary>
        /// 互动对唱等待询问请求
        /// [3506]
        /// </summary>
        public const short ReqWaitSingSong = 3506;
        
        /// <summary>
        /// 互动对唱等待询问回应
        /// [3507]
        /// </summary>
        public const short RspWaitSingSong = 3507;

        /// <summary>
        /// 加入黑名单通知
        /// [3508]
        /// </summary>
        public const short NoticeBlack = 3508;

        /// <summary>
        /// 获取排麦列表
        /// [3509]
        /// </summary>
        public const short GetSongList = 3509;

        /// <summary>
        /// 摄像机开启关闭状态通知
        /// [3510]
        /// </summary>
        public const short CamState = 3510;

        /// <summary>
        /// 包厢用户测时发送
        /// [3511]
        /// </summary>
        public const short ReqRoomUserTestTime = 3511;

        /// <summary>
        /// 包厢用户测时回应
        /// [3512]
        /// </summary>
        public const short RspRoomUserTestTime = 3512;

        /// <summary>
        /// 互动对唱等待询问请求（只转发不通知唱歌）
        /// [3513]
        /// </summary>
        public const short ReqWaitNoSingSong = 3513;

        /// <summary>
        /// 互动对唱等待询问回应（只转发不通知唱歌）
        /// [3514]
        /// </summary>
        public const short RspWaitNoSingSong = 3514;

        /// <summary>
        /// UDP端口注册成功通知
        /// [3515]
        /// </summary>
        public const short RegUDPNotice = 3515;

        /// <summary>
        /// 追加歌曲至排歌单(排麦信息含所有歌曲属性)
        /// [3516]
        /// </summary>
        public const short SuperaddSingSongListFull = 3516;

        /// <summary>
        /// 获取排麦列表（排麦信息含所有歌曲属性）
        /// [3517]
        /// </summary>
        public const short GetSongListFull = 3517;

    }
    #endregion

    #region 点歌服务指令
    /// <summary>
    /// 点歌服务指令
    /// </summary>
    public class SongCmd
    {
        /// <summary>
        /// 获取歌曲列表
        /// </summary>
        public const short SongList = 510;
        /// <summary>
        /// 获取歌曲菜单
        /// </summary>
        public const short SongType = 511;
        /// <summary>
        /// 获取歌曲属性
        /// </summary>
        public const short SongProperty = 1025;
        /// <summary>
        /// 获取歌曲文件
        /// </summary>
        public const short SongFile = 1026;
        /// <summary>
        /// 获取歌首名称
        /// </summary>
        public const short SingerName = 528;

    }
    #endregion

    #region 运维管理服务[OMS]服务器指令
    /// <summary>
    /// 运维管理服务[OMS]服务器指令
    /// </summary>
    public class OMSCmd
    {
        /// <summary>
        /// 获取服务器列表请求
        /// [5801]
        /// </summary>
        public const short ReqGetServList = 5801;

        /// <summary>
        /// 获取服务器列表回应
        /// [5802]
        /// </summary>
        public const short RspGetServList = 5802;

        /// <summary>
        /// 获取服务器状态请求
        /// [5803]
        /// </summary>
        public const short ReqGetServState = 5803;

        /// <summary>
        /// 获取服务器状态回应
        /// [5804]
        /// </summary>
        public const short RspGetServState = 5804;


        /// <summary>
        /// 启动服务请求
        /// </summary>
        public const short ReqServOpt = 5805;

        /// <summary>
        /// 启动服务回应
        /// [5806]
        /// </summary>
        public const short RspServOpt = 5806;

        /// <summary>
        /// 用户登录请求
        /// [5807]
        /// </summary>
        public const short ReqUserLogin = 5807;

        /// <summary>
        /// 用户登录回应
        /// [5808]
        /// </summary>
        public const short RspUserLogin = 5808;

        /// <summary>
        /// 设备类型添加
        /// [5809]
        /// </summary>
        public const short DevTypeAdd = 5809;

        /// <summary>
        /// 设备类型删除
        /// [5810]
        /// </summary>
        public const short DevTypeDelete = 5810;

        /// <summary>
        /// 设备类型修改
        /// [5811]
        /// </summary>
        public const short DevTypeModify = 5811;

        /// <summary>
        /// 设备类型查询
        /// [5812]
        /// </summary>
        public const short DevTypeQuery = 5812;

        /// <summary>
        /// 设备信息添加
        /// [5813]
        /// </summary>
        public const short DevInfoAdd = 5813;

        /// <summary>
        /// 设备信息删除
        /// [5814]
        /// </summary>
        public const short DevInfoDelete = 5814;

        /// <summary>
        /// 设备信息修改
        /// [5815]
        /// </summary>
        public const short DevInfoModify = 5815;

        /// <summary>
        /// 设备信息查询
        /// [5816]
        /// </summary>
        public const short DevInfoQuery = 5816;

        /// <summary>
        /// 用户添加
        /// [5817]
        /// </summary>
        public const short UserInfoAdd = 5817;

        /// <summary>
        /// 用户删除
        /// [5819]
        /// </summary>
        public const short UserInfoDelete = 5819;

        /// <summary>
        /// 用户修改
        /// [5820]
        /// </summary>
        public const short UserInfoModify = 5820;

        /// <summary>
        /// 用户查询
        /// [5821]
        /// </summary>
        public const short UserInfoQuery = 5821;

        /// <summary>
        /// 用户密码修改
        /// [5822]
        /// </summary>
        public const short ChangePwd = 5822;

        /// <summary>
        /// 故障日志查询
        /// [5823]
        /// </summary>
        public const short AlarmLogQuery = 5823;

        /// <summary>
        /// 故障日志清除
        /// [5824]
        /// </summary>
        public const short AlarmLogClear = 5824;

        /// <summary>
        /// KOIP挂线查询
        /// [5825]
        /// </summary>
        public const short KOIPOnline = 5825;

        /// <summary>
        /// 设备状态上传
        /// [5826]
        /// </summary>
        public const short UpDevState = 5826;



    }
    #endregion




}
