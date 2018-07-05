namespace KBEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using System.Linq;

    public class Account : AccountBase
    {
        public Dictionary<UInt64, AVATAR_INFOS> avatars = new Dictionary<UInt64, AVATAR_INFOS>();
        // Use this for initialization

        public Account():base()
        {

        }

        public override void __init__()
        {
            //注册事件
            Event.registerIn("reqAvatarList", this, "reqAvatarList");
            Event.registerIn("reqCreateAvatar", this, "reqCreateAvatar");
            Event.registerIn("reqRemoveAvatar", this, "reqRemoveAvatar");
            Event.registerIn("selectAvatarGame", this, "selectAvatarGame");

            Event.fireOut("onLoginSuccessfully",new object[] { KBEngineApp.app.entity_uuid,id,this});

//            baseEntityCall.reqAvatarList();

        }

        public override void onDestroy()
        {
            KBEngine.Event.deregisterIn(this);
        }

        public override void onCreateAvatarResult(Byte retcode, AVATAR_INFOS info)
        {
            if (retcode == 0)
            {
                avatars.Add(info.dbid, info);
                Dbg.DEBUG_MSG("Account::onCreateAvatarResult: name=" + info.name);
            }
            else
            {
                Dbg.DEBUG_MSG("Account::onCreateAvatarResult: retcode=" + retcode);
            }
            // ui event
            Event.fireOut("onCreateAvatarResult", new object[] { retcode, info, avatars });
        }
        public override void onRemoveAvatar(UInt64 dbid)
        {
            Dbg.DEBUG_MSG("Account::onRemoveAvatar: dbid=" + dbid);
            avatars.Remove(dbid);

            Event.fireOut("onRemoveAvatar", new object[] { dbid, avatars });
        }
        public override void onReqAvatarList(AVATAR_INFOS_LIST infos)
        {
            avatars.Clear();
            Dbg.DEBUG_MSG("Account::onReqAvatarList: avatarsize=" + infos.values.Count);
            for (int i = 0; i < infos.values.Count; i++)
            {
                AVATAR_INFOS info = infos.values[i];
                Dbg.DEBUG_MSG("Account::onReqAvatarList: name" + i + "=" + info.name);
                avatars.Add(info.dbid, info);
            }

            // ui event
            Dictionary<UInt64, AVATAR_INFOS> avatarList = new Dictionary<UInt64, AVATAR_INFOS>(avatars);
            Event.fireOut("onReqAvatarList", new object[] { avatarList });

            if (infos.values.Count == 0)
                return;
        }
        public void reqAvatarList()
        {
            Dbg.DEBUG_MSG("Account::reqAvatarList");
            baseEntityCall.reqAvatarList();
        }
        public void reqCreateAvatar(Byte roleType, string name, UInt16 level)
        {
            Dbg.DEBUG_MSG("Account::reqCreateAvatar: roleType=" + roleType);
            baseEntityCall.reqCreateAvatar(roleType, name, level);
        }

        public void reqRemoveAvatar(string name)
        {
            Dbg.DEBUG_MSG("Account::reqRemoveAvatar: name=" + name);
            baseEntityCall.reqRemoveAvatar(name);
        }
        public void selectAvatarGame(UInt64 dbid)
        {
            Dbg.DEBUG_MSG("Account::selectAvatarGame: dbid=" + dbid);
            baseEntityCall.selectAvatarGame(dbid);
        }
    }
}


