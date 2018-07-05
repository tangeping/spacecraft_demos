# -*- coding: utf-8 -*-
import random
import math
import Math
import time
import KBEngine
import SCDefine
import GameConfigs
from KBEDebug import *

from interfaces.GameObject import GameObject
from interfaces.Motion import Motion
from interfaces.State import State

class Weapon(KBEngine.Entity,GameObject,Motion,State):
    """
    docstring for Weapon
    """
    def __init__(self):
        KBEngine.Entity.__init__(self)
        GameObject.__init__(self)
        Motion.__init__(self)
        State.__init__(self)

        self.territoryControllerID = 0

        self.destroyTime = self.getDatas()["destroyTime"]
        self.explodeTime = self.getDatas().get("explodeTime",0)

        self.changeState(GameConfigs.ENTITY_STATE_FREE)

        self.addTimer(0.01,0,SCDefine.TIMER_TYPE_WEAPON_IN_WORLD)#只需要觸發一次

        DEBUG_MSG("Weapon::__init__: %i. attackerID:%i,ownerID:%i,destroyTime:%i" % (self.id,self.attackerID,self.ownerID,self.destroyTime))

        DEBUG_MSG('Weapon::__init__: id = %i ,position:(%f,%f,%f),direction(%f,%f,%f),forward(%f,%f,%f)'
            % (self.id,
                self.position.x,self.position.y,self.position.z,
                self.direction.x,self.direction.y,self.direction.z,
                self.destForward.x,self.destForward.y,self.destForward.z
                ))


    def isWeapon(self):

        return True

    def resetSpeed(self):
        walkSpeed = self.getDatas()["moveSpeed"]

        DEBUG_MSG("Weapon::resetSpeed: %i. walkSpeed:%i" % (self.id,walkSpeed))

        if walkSpeed != self.moveSpeed:
            self.moveSpeed = walkSpeed


    def onTimer(self,tid,userArg):

        if userArg == SCDefine.TIMER_TYPE_WEAPON_DURATION:
            self.startDestroyTimer()

        if userArg == SCDefine.TIMER_TYPE_WEAPON_IN_WORLD:
            self.onEnterWorld()

        GameObject.onTimer(self, tid, userArg)

    def getOwnner(self):

        return KBEngine.entities.get(self.ownerID)

    def onWeaponFly(self):

        attackDistant = self.getDatas()["attackDis"]/10.0
        runSpeed = self.getDatas()["runSpeed"]
        duration = self.getDatas()["destroyTime"]

        self.changeState(GameConfigs.ENTITY_STATE_FIGHT)

        if runSpeed != self.moveSpeed:
            self.moveSpeed = runSpeed


        entity = self.getOwnner()
        cruiseSpeed = 0 if entity is None else entity.cruiseSpeed
        speed = self.moveSpeed * 0.1 + cruiseSpeed * 0.1


        destPosition =  Math.Vector3(self.destForward) * duration *(self.moveSpeed + cruiseSpeed) + self.position

        DEBUG_MSG("Weapon::onWeaponFly.%i:position(%f,%f,%f),destForward(%f,%f,%f),duration=%i,moveSpeed=%f, cruiseSpeed=%f " %
            (self.id,self.position.x,self.position.y,self.position.z,self.destForward.x,self.destForward.y,self.destForward.z,duration,self.moveSpeed,cruiseSpeed))

        if destPosition.distTo(self.position) > attackDistant:
            self.gotoPosition(destPosition,speed)
        else:
            self.resetSpeed()
            self.stopMotion()


    def startDestroyTimer(self):
        """
        virtual method.

        启动销毁entitytimer
        """
        self.changeState(GameConfigs.ENTITY_STATE_DEAD)

        self.allClients.onWeaponDestroy(self.explodeTime)

        self.addTimer(5, 0, SCDefine.TIMER_TYPE_DESTROY)

        DEBUG_MSG("%s::startDestroyTimer: %i running." % (self.getScriptName(), self.id))


    def onEnterWorld(self):
        """
        KBEngine method.
        这个entity已经进入世界了
        """
        DEBUG_MSG("%s.%i::onEnterWorld." % (self.getScriptName(), self.id))

        self.addTimer(self.destroyTime,0, SCDefine.TIMER_TYPE_WEAPON_DURATION)

        self.addTerritory()

        self.onWeaponFly()

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

    def isInTerritory(self,entityCall):
        """
        范围触发是正方形，需要过滤为具体的长方形
        """
        if entityCall.isDestroyed or entityCall.getScriptName() != "Avatar" or entityCall.isDead() or entityCall.id == self.ownerID:
            return False

        DEBUG_MSG('%s::isInTerritory: id = %i ,position:(%f,%f,%f),entityCall(%f,%f,%f)'% (self.getScriptName(),self.id,
            self.position.x,self.position.y,self.position.z,entityCall.position.x,entityCall.position.y,entityCall.position.z ))

        range_x = self.getDatas()["range_x"]
        range_z = self.getDatas()["range_z"]

        distance_x = math.fabs(math.fabs(self.position.x) - math.fabs(entityCall.position.x))
        distance_z = math.fabs(math.fabs(self.position.z) - math.fabs(entityCall.position.z))

        DEBUG_MSG("%s::isInTerritory: range_x:%f,range_z:%f,distance_x:%f,distance_z:%f" %(self.getScriptName(),range_x,range_z,distance_x,distance_z))

        if distance_x  <= range_x and distance_z  <= range_z:
            return True
        else:
            return False


    def onEnterTrap(self, entityEntering, range_xz, range_y, controllerID, userarg):
        """
        KBEngine method.
        有entity进入trap
        """

        if entityEntering.isDestroyed or entityEntering.getScriptName() != "Avatar" or entityEntering.isDead() or entityEntering.id == self.ownerID:
            return

        if not self.isInTerritory(entityEntering):
            return

        damage = self.getDatas()["hpDamage"]
        entityEntering.recvDamage(self.ownerID,self.id,self.utype,damage)

        DEBUG_MSG("%s::onEnterTrap: %i entityEntering=(%s)%i, range_xz=%s, range_y=%s, controllerID=%i, userarg=%i" % \
                        (self.getScriptName(), self.id, entityEntering.getScriptName(), entityEntering.id, \
                        range_xz, range_y, controllerID, userarg))

        self.startDestroyTimer()

    def onLeaveTrap(self, entityLeaving, range_xz, range_y, controllerID, userarg):
        """
        KBEngine method.
        有entity离开trap
        """

        if entityLeaving.isDestroyed or entityLeaving.getScriptName() != "Avatar" or entityLeaving.isDead()or entityLeaving.id == self.ownerID:
            return

        INFO_MSG("%s::onLeaveTrap: %i entityLeaving=(%s)%i." % (self.getScriptName(), self.id, \
                entityLeaving.getScriptName(), entityLeaving.id))