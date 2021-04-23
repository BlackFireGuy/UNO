using System;
using System.Collections.Generic;
namespace ETModel
{
    public static partial class LandUIType
    {
        public const string LandLogin = "LandLogin";
    }

    public static partial class UIEventType
    {
        public const string LandInitSceneStart = "LandInitSceneStart";
    }

    [Event(UIEventType.LandInitSceneStart)]
    public class InitSceneStart_CreateLandLogin : AEvent
    {
        public override void Run()
        {
            //调用LandLoginFactory创建了登陆注册界面。
            Game.Scene.GetComponent<UIComponent>().Create(LandUIType.LandLogin);
        }
    }
}
