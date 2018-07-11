using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using System;

public class PlayerData:Singleton<PlayerData> {

  
    private string __account;
    private string __password;
    private Dictionary<UInt64, AVATAR_INFOS> __avatarList;
    private Dictionary<INT32, UINT32> __weaponDestoryTime;
    private INT16 __selectIndex;
    private UInt64 __selAvatarDBID;
    private int __reliveCount;
    private ShipAttri _shipAttri;

    public string Account
    {
        get
        {
            return __account;
        }

        set
        {
            __account = value;
        }
    }

    public string Password
    {
        get
        {
            return __password;
        }

        set
        {
            __password = value;
        }
    }

    public Dictionary<ulong, AVATAR_INFOS> AvatarList
    {
        get
        {
            return __avatarList;
        }

        set
        {
            __avatarList = value;
        }
    }

    public INT16 SelectIndex
    {
        get
        {
            if (__selectIndex < 1)
            {
                __selectIndex = 1;
            }
            else if (__selectIndex > 3)
            {
                __selectIndex = 3;
            }
            return  __selectIndex;
        }

        set
        {
            if (__selectIndex < 1)
            {
                value = 1;
            }
            else if (__selectIndex > 3)
            {
                value = 3;
            }

            __selectIndex = value;
        }
    }

    public ulong SelAvatarDBID
    {
        get
        {
            return __selAvatarDBID;
        }

        set
        {
            __selAvatarDBID = value;
        }
    }

    public int ReliveCount
    {
        get
        {
            return __reliveCount;
        }

        set
        {
            __reliveCount = value;
        }
    }

    public Dictionary<INT32, UINT32> WeaponDestoryTime
    {
        get
        {
            return __weaponDestoryTime;
        }

        set
        {
            __weaponDestoryTime = value;
        }
    }

    public ShipAttri ShipAttri
    {
        get
        {
            return _shipAttri;
        }

        set
        {
            _shipAttri = value;
        }
    }
}
