# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
from interfaces.GameObject import GameObject
from interfaces.Motion import Motion
from interfaces.Combat import Combat
from interfaces.Arsenal import Arsenal
from interfaces.State import State
from interfaces.Settle import Settle

import GameConfigs
import GameUtils
import SCDefine

class Avatar(KBEngine.Entity,GameObject,Motion,Combat,Arsenal,State,Settle):

    def  __init__(self):
        KBEngine.Entity.__init__(self)
        GameObject.__init__(self)
        Motion.__init__(self)
        Combat.__init__(self)
        Arsenal.__init__(self)
        State.__init__(self)
        Settle.__init__(self)

        #self.topSpeed = self.moveSpeed + 5.0
        self.topSpeed = 0.0

                # 默认开始的尺寸
        self.modelRadius = 0.5

        self.changeState(GameConfigs.ENTITY_STATE_FREE)



        # 随机的初始化一个出生位置
#        self.position =  GameUtils.randomPosition3D(self.modelRadius)

        # self.topSpeedY = 10.0
        self.getCurrSpace().onEnter(self)

        # 可视范围起始位30米，后期根据长大尺寸调整
 #       self.setViewRadius(30.0, 1.0)

        DEBUG_MSG('Avatar::__init__: id = %i ,position:(%f,%f,%f)' % (self.id,self.position.x,self.position.y,self.position.z))


    def  isPlayer(self):
        """
        virtual method.
        """
        return True

    def startDestroyTimer(self):
        """
        virtual method.

        启动销毁entitytimer
        """
        pass


    #--------------------------------------------------------------------------------------------
    #                              Callbacks
    #--------------------------------------------------------------------------------------------
    def onTimer(self, tid, userArg):
        """
        KBEngine method.
        引擎回调timer触发
        """
        #DEBUG_MSG("%s::onTimer: %i, tid:%i, arg:%i" % (self.getScriptName(), self.id, tid, userArg))
        GameObject.onTimer(self, tid, userArg)
        Arsenal.onTimer(self, tid, userArg)

    def onDestroy(self):
        """
        KBEngine method.
        entity销毁
        """
        DEBUG_MSG("Avatar::onDestroy: %i." % self.id)


    def relive(self, exposed, type):
        """
        defined.
        复活
        """
        if exposed != self.id:
            return

        DEBUG_MSG("Avatar::relive: %i, type=%i." % (self.id, type))


    def jump(self, exposed):
        """
        defined.
        玩家跳跃 我们广播这个行为
        """
        if exposed != self.id:
            return

        self.otherClients.onJump()

    def onAddEnemy(self, entityID):
        """
        virtual method.
        有敌人进入列表
        """
        pass

    def onEnemyEmpty(self):
        """
        virtual method.
        敌人列表空了
        """
        pass
