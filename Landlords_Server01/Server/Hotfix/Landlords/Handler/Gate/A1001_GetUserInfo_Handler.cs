﻿using System;
using ETModel;

namespace ETHotfix
{
    //已知玩家UserID 尝试读取玩家资料
    [MessageHandler(AppType.Gate)]
    public class A1001_GetUserInfo_Handler : AMRpcHandler<A1001_GetUserInfo_C2G, A1001_GetUserInfo_G2C>
    {
        protected override async ETTask Run(Session session, A1001_GetUserInfo_C2G request, A1001_GetUserInfo_G2C response, Action reply)
        {
            try
            {
                //验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_UserNotOnline;
                    reply();
                    return;
                }

                //获取玩家对象
                User user = session.GetComponent<SessionUserComponent>().User;
                DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
                UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(user.UserID);

                response.UserName = userInfo.UserName;
                response.Money = userInfo.Money;
                response.Level = userInfo.Level;
                response.Phone = userInfo.Phone;
                response.Email = userInfo.Email;
                response.Sex = userInfo.Sex;
                response.Title = userInfo.Title;

                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}