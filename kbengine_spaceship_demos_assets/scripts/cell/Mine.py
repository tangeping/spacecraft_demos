# -*- coding: utf-8 -*-
import KBEngine
import SCDefine
from KBEDebug import *
from interfaces.GameObject import GameObject


class Mine(KBEngine.Entity,GameObject):

    def  __init__(self):
        """

        """
        KBEngine.Entity.__init__(self)
        GameObject.__init__(self)

        self.territoryControllerID = 0
        self.addTimer(0.01,0,SCDefine.TIMER_TYPE_PROP_IN_WORLD)#只需要觸發一次

        DEBUG_MSG('Mine::__init__: id = %i ,position:(%f,%f,%f),direction(%f,%f,%f)'
        % (self.id,self.position.x,self.position.y,self.position.z,self.direction.x,self.direction.y,self.direction.z,))

    def initEntity(self):
        """
        virtual method.
        """
        pass

    def isMine(self):
        """
        virtual method.
        """
        return True

    def onTimer(self,tid,userArg):

        if userArg == SCDefine.TIMER_TYPE_PROP_IN_WORLD:
            self.onEnterWorld()

        GameObject.onTimer(self, tid, userArg)


    def onEnterWorld(self):
        """
        KBEngine method.
        这个entity已经进入世界了
        """
        DEBUG_MSG("%s.%i::onEnterWorld." % (self.getScriptName(), self.id))

        self.addTerritory()


    def addTerritory(self):
        """
        添加触发范围

        """

        assert self.territoryControllerID == 0 and "territoryControllerID != 0"

        trange = self.getDatas()["attackScale"]/2.0

        self.territoryControllerID = self.addProximity(trange, 0, 0)

        if self.territoryControllerID <= 0:
            ERROR_MSG("%s::addTerritory: %i, range=%i, is error!" % (self.getScriptName(), self.id, trange))
        else:
            INFO_MSG("%s::addTerritory: %i range=%i, id=%i." % (self.getScriptName(), self.id, trange, self.territoryControllerID))

    def delTerritory(self):
        """
        删除触发范围
        """
        if self.territoryControllerID > 0:
            self.cancelController(self.territoryControllerID)
            self.territoryControllerID = 0
            INFO_MSG("%s::delTerritory: %i" % (self.getScriptName(), self.id))


    def startDestroyTimer(self):
        """
        virtual method.

        启动销毁entitytimer
        """
        self.addTimer(4, 0, SCDefine.TIMER_TYPE_DESTROY)

        DEBUG_MSG("%s::startDestroyTimer: %i running." % (self.getScriptName(), self.id))


    def onEnterTrap(self, entityEntering, range_xz, range_y, controllerID, userarg):
        """
        KBEngine method.
        有entity进入trap
        """

        if entityEntering.isDestroyed or entityEntering.getScriptName() != "Avatar" or entityEntering.isDead() or entityEntering.id == self.id:
            return

        entityEntering.recvPropEffect(self.uid,self.utype)

        DEBUG_MSG("%s::onEnterTrap: %i entityEntering=(%s)%i, range_xz=%s, range_y=%s, controllerID=%i, userarg=%i" % \
                        (self.getScriptName(), self.id, entityEntering.getScriptName(), entityEntering.id, \
                        range_xz, range_y, controllerID, userarg))

        explodeTime = self.getDatas().get("explodeTime",0)
        self.allClients.onMineDestroy(entityEntering.id, explodeTime)

        self.startDestroyTimer()

    def onLeaveTrap(self, entityLeaving, range_xz, range_y, controllerID, userarg):
        """
        KBEngine method.
        有entity离开trap
        """

        if entityLeaving.isDestroyed or entityLeaving.getScriptName() != "Avatar" or entityLeaving.isDead()or entityLeaving.id == self.id:
            return

        INFO_MSG("%s::onLeaveTrap: %i entityLeaving=(%s)%i." % (self.getScriptName(), self.id, \
                entityLeaving.getScriptName(), entityLeaving.id))


    #--------------------------------------------------------------------------------------------
    #                              Callbacks
    #--------------------------------------------------------------------------------------------

    def onDestroy(self):
        """
        entity销毁
        """

        space = self.getCurrSpace()

        if space == None:
            return

        space.onMineDestroy(self.id)