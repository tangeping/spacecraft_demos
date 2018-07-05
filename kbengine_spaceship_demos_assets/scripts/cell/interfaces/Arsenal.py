# -*- coding: utf-8 -*-
import KBEngine
import GameConfigs
import SCDefine
import d_entities
import math
import Math
from KBEDebug import *


class Arsenal:
    """docstring for Arsenal"""
    def __init__(self):
        self.coolTimeList={}
#        self.addTimer(1,1,SCDefine.TIMER_TYPE_WEAPON_CD)

    def hasWeapon(self,weapon):
        return entityID in self.weapons


    def reqWeaponList(self,exposed):

        if self.id != exposed:
            return
        DEBUG_MSG("Arsenal::reqWeaponList: weapons = %s" % str(self.weapons))

        self.client.onReqWeaponList(self.weapons)


    def addWeapon(self,entityCall):

        self.weapons[entityCall.id] = entityCall


    def removeWeapon(self,entityID):

        del self.weapons[entityID]


    def canUseWeapon(self,weaponID):

        cd = self.coolTimeList.get(weaponID,0)
        haveWeapon = d_entities.datas[weaponID] is None
        self.client.canUseWeaponResult(cd,haveWeapon)

        if  cd> 0 or  haveWeapon:
            return False

        self.coolTimeList[weaponID] = d_entities.datas[weaponID]["cd"]

        return  True

    def onTimer(self, tid, userArg):
        """
        KBEngine method.
        引擎回调timer触发
        """
        #DEBUG_MSG("%s::onTimer: %i, tid:%i, arg:%i" % (self.getScriptName(), self.id, tid, userArg))
        if SCDefine.TIMER_TYPE_WEAPON_CD == userArg:
            self.onCoolDownTick()

    def onCoolDownTick(self):
        """
        onCoolDownTick
        此处可以轮询所有的CD，将需要执行的CD执行
        """
        DEBUG_MSG("onCoolDownTick")

        for weapon in self.coolTimeList:
            if self.coolTimeList[weapon] > 0 :
                self.coolTimeList[weapon] -= 1


    def reqUseWeapon(self,exposed,position,direction,destForward,weaponID):

        """
        defined.
        对目标发射武器
        """
        if exposed != self.id:
            return

#        if self.canUseWeapon(weaponID):
#            return

        datas = d_entities.datas[weaponID]
        if datas is None:
            ERROR_MSG("SpawnPoint::spawn:%i not found." % weaponID)
            return

        DEBUG_MSG("Arsenal::reqUseWeapon(%i):weaponID=%i,position:(%f,%f,%f)" % (self.id, weaponID,position.x,position.y,position.z))

        delayBornTime = datas["delayBornTime"] * 0.001

        position = position +  Math.Vector3(destForward) * (self.cruiseSpeed*0.1 * delayBornTime)

        params = {
            "ownerID": self.id,
            "uid" : datas["id"],
            "utype" : datas["etype"],
            "modelID" : datas["modelID"],
            "dialogID" : datas["dialogID"],
            "name" : datas["name"],
            "CD":   datas["cd"],
            "destForward": destForward,
            "descr" : datas.get("descr", ''),
        }

        e = KBEngine.createEntity(datas["entityType"], self.spaceID, position, direction, params)

        self.changeState(GameConfigs.ENTITY_STATE_FIGHT)






