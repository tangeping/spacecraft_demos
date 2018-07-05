# -*- coding: utf-8 -*-
import KBEngine
import Functor
from KBEDebug import *
import GameConfigs

FIND_SPACE_NOT_FOUND = 0
FIND_SPACE_CREATING = 1

class Spaces(KBEngine.Entity):
    """
    这是一个脚本层封装的空间管理器
    KBEngine的space是一个抽象空间的概念，一个空间可以被脚本层视为游戏场景、游戏房间、甚至是一个宇宙。
    """

    def __init__(self):
        KBEngine.Entity.__init__(self)

        # 向全局共享数据中注册这个管理器的entityCall以便在所有逻辑进程中可以方便的访问
        KBEngine.globalData["Spaces"] = self

        self._spaceAllocs = {}

        self.lastNewSpaceKey = 0


    def findSpace(self,spaceKey,notFoundCreate =False):

        spaceDatas = self._spaceAllocs.get(spaceKey)

        if not spaceDatas:
            if not notFoundCreate:
                return FIND_SPACE_NOT_FOUND

            spaceDatas = self._spaceAllocs.get(self.lastNewSpaceKey)
            if spaceDatas is not None  and  spaceDatas['PlayerCount'] < GameConfigs.SPACE_MAX_PLAYER:
                return spaceDatas

            self.lastNewSpaceKey = KBEngine.genUUID64()

            KBEngine.createEntityAnywhere("Space",\
                                                {
                                                    "SpaceKey":self.lastNewSpaceKey,\
                                                },
                                                Functor.Functor(self.onSpaceCreateCB,self.lastNewSpaceKey))

            spaceDatas = {"spaceEntityCall" : None, "PlayerCount": 0, "enterSpaceReqs" : [], "SpaceKey" : self.lastNewSpaceKey}
            self._spaceAllocs[self.lastNewSpaceKey] = spaceDatas
            return spaceDatas

        return spaceDatas

    def onSpaceCreateCB(self,spaceKey,spaceEntitycall):

        DEBUG_MSG("Spaces::onSpaceCreateCB: space= %i. entityID = %i" % (spaceKey,spaceEntitycall.id))


    def loginToSpace(self, entityCall, position, direction, spaceKey):
        """
        defined method.
        某个玩家请求登陆到某个space中
        """
        DEBUG_MSG('Spaces::loginToSpace: id = %i ,position:%s' % (self.id,str(position)))

        spaceDatas = self.findSpace(spaceKey,True)
        spaceDatas["PlayerCount"] += 1

        spaceEntityCall =  spaceDatas['spaceEntityCall']
        if spaceEntityCall is not None:
            spaceEntityCall.loginToSpace(entityCall, position, direction)
        else:
            DEBUG_MSG("spaces::loginToSpace: space %i creating..., enter entityID=%i" % (spaceDatas["SpaceKey"], entityCall.id))
            spaceDatas["enterSpaceReqs"].append((entityCall, position, direction))


    def logoutSpace(self, avatarID, spaceKey):
        """
        defined method.
        某个玩家请求登出这个space
        """
        spaceDatas = self.findSpace(spaceKey,False)

        if type(spaceDatas) is dict:
            spaceEntityCall =  spaceDatas['spaceEntityCall']
            if spaceEntityCall is not None:
                spaceEntityCall.logoutToSpace(avatarID)
        else:
            # 由于玩家即使是掉线都会缓存至少一局游戏， 因此应该不存在退出房间期间地图正常创建中
            if spaceDatas == FIND_SPACE_CREATING:
                raise Exception("FIND_ROOM_CREATING")

    #--------------------------------------------------------------------------------------------
    #                              Callbacks
    #--------------------------------------------------------------------------------------------
    def onTimer(self, tid, userArg):
        """
        KBEngine method.
        引擎回调timer触发
        """
        #DEBUG_MSG("%s::onTimer: %i, tid:%i, arg:%i" % (self.getScriptName(), self.id, tid, userArg))
        if SCDefine.TIMER_TYPE_CREATE_SPACES == userArg:
            self.createSpaceOnTimer(tid)

        GameObject.onTimer(self, tid, userArg)

    def onSpaceLoseCell(self, spaceKey):
        """
        defined method.
        space的cell创建好了
        """
        DEBUG_MSG("Spaces::onSpaceLoseCell: space %i." % (spaceKey))
        del  self._spaceAllocs[spaceKey]

    def onSpaceGetCell(self, spaceEntityCall, spaceKey):
        """
        defined method.
        space的cell创建好了
        """
        DEBUG_MSG("Spaces::onSpaceGetCell: enterSpaceReqs = %s" % str(self._spaceAllocs[spaceKey]["enterSpaceReqs"]))

        self._spaceAllocs[spaceKey]["spaceEntityCall"] = spaceEntityCall

        for infos in self._spaceAllocs[spaceKey]["enterSpaceReqs"]:
            entityCall = infos[0]
            entityCall.createCell(spaceEntityCall.cell,spaceKey)

        self._spaceAllocs[spaceKey]["enterSpaceReqs"] = []