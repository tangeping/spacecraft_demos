# -*- coding: utf-8 -*-
import KBEngine
import random
import time
from KBEDebug import *
import d_avatar_inittab
from AVATAR_INFOS import TAvatarInfos
from AVATAR_INFOS import TAvatarInfosList
from AVATAR_DATA import TAvatarData

class Account(KBEngine.Proxy):
    """
    账号实体
    客户端登陆到服务端后，服务端将自动创建这个实体，通过这个实体与客户端进行交互
    """
    def __init__(self):
        KBEngine.Proxy.__init__(self)
        self.activeAvatar = None
        self.relogin = time.time()


    def reqAvatarList(self):
        """
        exposed.
        客户端请求查询角色列表
        """
        DEBUG_MSG("Account[%i].reqAvatarList: size=%i." % (self.id, len(self.characters)))
        self.client.onReqAvatarList(self.characters)

    def reqCreateAvatar(self, roleType, name,level):
        """
        exposed.
        客户端请求创建一个角色
        """
        avatarinfo = TAvatarInfos()
        avatarinfo.extend([0, "", 0, 0, TAvatarData().createFromDict({"param1" : 0, "param2" :b''})])


        if len(self.characters) >= 3:
            DEBUG_MSG("Account[%i].size = %i,reqCreateAvatar:%s. character=%s.\n" % (self.id, len(self.characters),name, self.characters))
#            self.client.onCreateAvatarResult(3, avatarinfo)
            return

        avatar_datas = d_avatar_inittab.datas.get(roleType,{})
        avatar_level = level
        exp_max = d_avatar_inittab.upgrade_props.get(avatar_level,{}).get("exp",0)

        props = {
            "name"				: name,
            "roleType"			: roleType,
            "level"				: avatar_level,
            "spaceUType"		: 2, # 玩家登录进入副本进行匹配
            "direction"			: (0, 0, avatar_datas["spawnYaw"]),
            "position"			:  (0,0,0),
            "HP"                : avatar_datas["hp"],
            "MP"                : avatar_datas["mp"],
            "HP_Max"            : avatar_datas["hp_max"],
            "MP_Max"            : avatar_datas["mp_max"],
            "EXP_Max"           : exp_max,
            "uid"               : roleType,

            "component1"		: { "bb" : 1231, "state" : 456},
            "component3"		: { "state" : 888 },
            }

        avatar = KBEngine.createEntityLocally('Avatar', props)
        if avatar:
            avatar.writeToDB(self._onAvatarSaved)


        DEBUG_MSG("Account[%i].reqCreateAvatar:%s. spaceUType=%i, spawnPos=%s.\n" % (self.id, name, avatar.cellData["spaceUType"], (0,0,0)))

    def selectAvatarGame(self, dbid):
        """
        exposed.
        客户端选择某个角色进行游戏
        """
        DEBUG_MSG("Account[%i].selectAvatarGame:%i. self.activeAvatar=%s" % (self.id, dbid, self.activeAvatar))
        # 注意:使用giveClientTo的entity必须是当前baseapp上的entity

        if self.activeAvatar is None:
            if dbid in self.characters:
                self.lastSelCharacter = dbid
                KBEngine.createEntityFromDBID("Avatar",dbid,self.__onAvatarCreate)
            else:
                EROOR_MSG("Account[%i]::selectAvatarGame:not found dbid(%i)"% (self.id,dbid))
        else:
            self.giveClientTo(self.activeAvatar)

    def reqRemoveAvatar(self,name):
        """
        exposed.
        客户端请求删除一个角色
        """
        DEBUG_MSG("Account[%i].reqRemoveAvatar: %s" % (self.id, name))
        found =0
        for key,info in self.characters.items():
            if info[1] == name :
                del self.characters[key]
                found = key
                break
        self.client.onRemoveAvatar(found)

    def reqRemoveAvatarDBID(self,dbid):
        """
        exposed.
        客户端请求删除一个角色
        """
        DEBUG_MSG("Account[%i].reqRemoveAvatar: %s" % (self.id, dbid))
        found =0
        for dbid in self.characters:
                del self.characters[key]
                found = dbid
                break
        self.client.onRemoveAvatar(dbid)

    #--------------------------------------------------------------------------------------------
    #                              Callbacks
    #--------------------------------------------------------------------------------------------
    #
    def onTimer(self, id, userArg):
        """
        KBEngine method.
        使用addTimer后， 当时间到达则该接口被调用
        @param id		: addTimer 的返回值ID
        @param userArg	: addTimer 最后一个参数所给入的数据
        """
        DEBUG_MSG(id, userArg)

    def onClientEnabled(self):
        """
        KBEngine method.
        该entity被正式激活为可使用， 此时entity已经建立了client对应实体， 可以在此创建它的
        cell部分。
        """
        INFO_MSG("account[%i] entities enable. entityCall:%s" % (self.id, self.client))


        for  key,info in  d_avatar_inittab.default_avatars.items():
            DEBUG_MSG("Account[%i]. roleType:%i name:%s \n" % (self.id, info['roleType'], info['name']))
            self.reqCreateAvatar(info['roleType'],self.__ACCOUNT_NAME__+'.'+info['name'],info['level'])


    def onLogOnAttempt(self, ip, port, password):
        """
        KBEngine method.
        客户端登陆失败时会回调到这里
        """
        INFO_MSG("Account[%i]::onLogOnAttempt: ip=%s, port=%i, selfclient=%s" % (self.id, ip, port, self.client))

        if self.activeAvatar :
            if self.activeAvatar.client is not None:
                self.activeAvatar.giveClientTo(self)

            self.relogin = time.time()
            self.activeAvatar.destroySelf()
            self.activeAvatar = None

        return KBEngine.LOG_ON_ACCEPT

    def onClientDeath(self):
        """
        KBEngine method.
        客户端对应实体已经销毁
        """
        DEBUG_MSG("Account[%i].onClientDeath:" % self.id)
        self.destroy()

    def onDestroy(self):
            """
            KBEngine method.
            entity销毁
            """
            DEBUG_MSG("Account::onDestroy: %i." % self.id)

            if self.activeAvatar:
                self.activeAvatar.accountEntity = None

                try:
                    self.activeAvatar.destroySelf()
                except:
                    pass

                self.activeAvatar = None

    def __onAvatarCreate(self,baseRef,dbid,wasActive):
        """
        回调函数带有3个参数：baseRef，databaseID和wasActive。如果操作成功，baseRef会是一个entityCall或者是新创建的Entity实体的直接引用，databaseID会是实体的数据库ID，无论该实体是否已经激活
        wasActive都会有所指示，如果wasActive是True则baseRef是已经存在的实体的引用(已经从数据库检出)。如果操作失败这三个参数的值，baseRef将会是None，databaseID将会是0，wasActive将会是False
        """
        if wasActive:
            ERROR_MSG("Account::__onAvatarCreated:(%i): this character is in world now!" % (self.id))
            return
        if baseRef is None:
            ERROR_MSG("Account::__onAvatarCreated:(%i): the character you wanted to created is not exist!" % (self.id))
            return

        avatar = KBEngine.entities.get(baseRef.id)
        if avatar is None:
            ERROR_MSG("Account::__onAvatarCreated:(%i): when character was created, it died as well!" % (self.id))
            return

        if self.isDestroyed:
            ERROR_MSG("Account::__onAvatarCreated:(%i): i dead, will the destroy of Avatar!" % (self.id))
            avatar.destroy()
            return

        info = self.characters[dbid]
        avatar.cellData["modelID"] = d_avatar_inittab.datas[info[2]]["modelID"]
        avatar.cellData["modelScale"] = d_avatar_inittab.datas[info[2]]["modelScale"]
        avatar.cellData["moveSpeed"] = d_avatar_inittab.datas[info[2]]["moveSpeed"]
        avatar.accountEntity = self
        self.activeAvatar = avatar
        INFO_MSG("__onAvatarCreate::position:%s" % (str(avatar.cellData["position"])))
        self.giveClientTo(avatar)

    def _onAvatarSaved(self,success,avatar):
        """
        这个可选参数是当数据库操作完成后的回调函数。它有两个参数。第一个是boolean类型标志成功或失败，第二个是base实体
        """
        INFO_MSG('Account::_onAvatarSaved:(%i) create avatar state: %i, %s, %i' % (self.id, success, avatar.cellData["name"], avatar.databaseID))

        if self.isDestroyed:
            if avatar:
                avatar.destroy(True)
            return

        avatarinfo = TAvatarInfos()
        avatarinfo.extend([0, "", 0, 0, TAvatarData().createFromDict({"param1" : 0, "param2" :b''})])

        if success:
            info = TAvatarInfos()
            info.extend([avatar.databaseID, avatar.cellData["name"], avatar.roleType, 1, TAvatarData().createFromDict({"param1" : 1, "param2" :b'1'})])
            self.characters[avatar.databaseID] = info
            avatarinfo[0] = avatar.databaseID
            avatarinfo[1] = avatar.cellData["name"]
            avatarinfo[2] = avatar.roleType
            avatarinfo[3] = 1
            self.writeToDB()
        else:
            avatarinfo[1] = "创建失败了"

        avatar.destroy()


