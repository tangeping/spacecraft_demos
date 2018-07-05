# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import SCDefine


class Space(KBEngine.Entity):


    def __init__(self):
        KBEngine.Entity.__init__(self)

        # 包含的实体字典，key=id，value=EntityCall
        self.avatars = {}

        self.cellData["SpaceKeyC"] = self.SpaceKey

        self.createCellEntityInNewSpace(None)

        DEBUG_MSG("Space[%i]::__init__:SpaceKey = %i " % (self.id,self.SpaceKey))


    def onTimer(self,tid,userArg):
        #DEBUG_MSG("%s::onTimer: %i, tid:%i, arg:%i" % (self.getScriptName(), self.id, tid, userArg))
        pass

    def loginToSpace(self,entityCall, position, direction):
        """
        某个Entity请求登录到该场景
        :param entityCall: 要进入本场景的实体entityCall
        """
        DEBUG_MSG("Space[%i].loginToSpace: entityCall.id =%i, SpaceKey =%i" % (self.id,entityCall.id,self.SpaceKey))
        entityCall.createCell(self.cell,self.SpaceKey)
        self.onEnter(entityCall)


    def logoutToSpace(self,entityId):
        """
        某个玩家请求登出该场景
        :param entityId: 登出者的id
        """
        DEBUG_MSG("Space[%i].logoutToSpace: SpaceKey =%i" % (self.id,self.SpaceKey))
        self.onLeave(entityId)

    def onEnter(self,entityCall):
        """
        virtual method.
        进入后的通知
        """
        DEBUG_MSG("Space[%i].onEnter: SpaceKey =%i" % (self.id,self.SpaceKey))
        self.avatars[entityCall.id] = entityCall


    def onLeave(self,entityId):
        """
        virtual method.
        离开后的通知
        """
        DEBUG_MSG("Space[%i].onLeave: SpaceKey =%i" % (self.id,self.SpaceKey))
        if entityId in self.avatars:
            del self.avatars[entityId]


    def onLoseCell(self):
        """
        KBEngine method.
        entity的cell部分实体丢失
        """
        DEBUG_MSG("Space[%i].onLoseCell: SpaceKey =%i" % (self.id,self.SpaceKey))
        KBEngine.globalData["Spaces"].onSpaceLoseCell(self.SpaceKey)

        self.avatars = {}
        self.destroy()

    def onGetCell(self):
        """
        entity的cell部分被创建成功
        """
        DEBUG_MSG("Space[%i]::onGetCell:SpaceKey=%i " % (self.id, self.SpaceKey))
        KBEngine.globalData["Spaces"].onSpaceGetCell(self, self.SpaceKey)

