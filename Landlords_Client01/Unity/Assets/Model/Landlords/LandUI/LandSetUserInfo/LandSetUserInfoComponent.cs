using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class LandSetUserInfoComponentAwakeSystem : AwakeSystem<LandSetUserInfoComponent>
    {
        public override void Awake(LandSetUserInfoComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 修改设置用户信息界面组件
    /// </summary>
    public class LandSetUserInfoComponent : Component
    {
        //电话
        public InputField phone;
        //邮箱
        public InputField email;
        //性别
        public InputField sex;

        public async void  Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            phone = rc.Get<GameObject>("Phone").GetComponent<InputField>();
            email = rc.Get<GameObject>("Email").GetComponent<InputField>();
            sex = rc.Get<GameObject>("Sex").GetComponent<InputField>();

            rc.Get<GameObject>("Confirm").GetComponent<Button>().onClick.Add(OnConfirmUserInfo);

            //获取玩家数据
            A1001_GetUserInfo_C2G GetUserInfo_Req = new A1001_GetUserInfo_C2G();
            A1001_GetUserInfo_G2C GetUserInfo_Ack = (A1001_GetUserInfo_G2C)await SessionComponent.Instance.Session.Call(GetUserInfo_Req);

            //显示用户名和用户等级
            phone.text = GetUserInfo_Ack.Phone.ToString();
            email.text = GetUserInfo_Ack.Email;
            sex.text = GetUserInfo_Ack.Sex;
        }

        public async void OnConfirmUserInfo()
        {
            try
            {
                //发送设置用户信息消息
                A1002_SetUserInfo_C2G SetUserInfo_Req = new A1002_SetUserInfo_C2G()
                {
                    Phone = Int64.Parse(phone.text),
                    Email = email.text,
                    Sex = sex.text
                };
                A1002_SetUserInfo_G2C SetUserInfo_Ack = (A1002_SetUserInfo_G2C)await SessionComponent.Instance.Session.Call(SetUserInfo_Req);

                //更新大厅界面上的用户信息
                LandLobbyComponent lobbyComponent = Game.Scene.GetComponent<UIComponent>().Get(LandUIType.LandLobby).GetComponent<LandLobbyComponent>();
                lobbyComponent.UpdateUserInfo(SetUserInfo_Ack);

                //移除用户信息设置界面
                Game.EventSystem.Run(UIEventType.LandSetUserInfoFinish);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}