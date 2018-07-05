# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import GameConfigs
import random
import GameUtils
import SCDefine
import copy
import d_space_spawns
import d_entities
import randomPoints

TIMER_TYPE_DESTROY = 1
TIMER_TYPE_BALANCE_MASS = 2

class Space(KBEngine.Entity):
    """docstring for Space"""


    def __init__(self):
        KBEngine.Entity.__init__(self)

        # 把自己移动到一个不可能触碰陷阱的地方
        self.position = (999999.0, 0.0, 0.0)

        # 这个房间中所有的玩家
        self.avatars = {}

        # 这个房间中产生的所有地雷
        self.mines = []

        # 这个房间中产生的所有补给箱
        self.surpplyboxs = []

        # 房间内所有物品的位置
        self.spwanPoslist = []

        # 这个地图上创建的entity总数
        self.tmpCreateEntityDatas = copy.deepcopy(d_space_spawns.datas)

        DEBUG_MSG('created Space cell entityID = %i.' % ( self.id))

        KBEngine.globalData['Space_%i' % self.spaceID] = self.base

        # 设置房间必要的数据，客户端可以获取之后做一些显示和限制
        KBEngine.setSpaceData(self.spaceID, "GAME_MAP_SIZE",  str(GameConfigs.GAME_MAP_SIZE))
        KBEngine.setSpaceData(self.spaceID, "ROOM_MAX_PLAYER",  str(GameConfigs.SPACE_MAX_PLAYER))
        KBEngine.setSpaceData(self.spaceID, "GAME_ROUND_TIME",  str(GameConfigs.GAME_ROUND_TIME))

        # 开始记录一局游戏时间， 时间结束后将产生毒圈，逐渐缩小毒圈，在毒圈范围外的持续掉血，直到最后一个玩家存活，结束游戏
#        self._destroyTimer = self.addTimer(SCDefine.POISON_CREATE_TIME, 0, SCDefine.TIMER_TYPE_POISON_BEGINE)

        # 开启一个timer加载道具
        self.addTimer(0.1, SCDefine.PROP_BLANCE_TIME, SCDefine.TIMER_TYPE_SPACE_SPAWN_TICK)

    def spawnCreatEntity(self,spwanID,position,direction):
        """

        """
        entities_datas = d_entities.datas.get(spwanID,None)

        if entities_datas is None:
            ERROR_MSG('entities_datas is None spwanID = %i.' % (spwanID))
            return

        params = {
            "spawnID"       : spwanID,
            "spawnPos"      : position,
            "uid"           : entities_datas["id"],
            "utype"         : entities_datas["etype"],
            "modelID"       : entities_datas["modelID"],
            "dialogID"      : entities_datas["dialogID"],
            "name"          : entities_datas["name"],
            "descr"         : entities_datas.get("descr", ''),
        }
        e = KBEngine.createEntity(entities_datas["entityType"], self.spaceID,  position,direction, params)

        if entities_datas["entityType"] == "Mine":
            self.mines.append(e.id)
        elif entities_datas["entityType"] == "SupplyBox":
            self.surpplyboxs.append(e.id)
        else:
            ERROR_MSG('entities_datas is not props spwanID = %i.' % (spwanID))
            return

        DEBUG_MSG("entityType:%s,spaceID:%i,positon:%s,direction:%s"%(entities_datas["entityType"], self.spaceID,position,direction))


    def spawnPoints(self):

        radius = 1.0 + random.random()

        for i in range(0,len(randomPoints.ponits_10dis)):

            position = GameUtils.randomPoint3D_10Dis(radius)

            if position  in self.spwanPoslist:
                continue
            else:
                self.spwanPoslist.append(position)
                break

        position.y = -1.0
        direction = (0.0,0.0,0.0)

        return position,direction

    def spawnOnTimer(self, tid):
        """
        出生道具
        """
        diffMinesCount = GameConfigs.MINES_MAX - len(self.mines)

        for i in range(diffMinesCount):

            position,direction = self.spawnPoints()
            spwanID = d_space_spawns.props["Mine"]

            self.spawnCreatEntity(spwanID,position,direction)

        diffSupplyBox = GameConfigs.SUPPLY_BOXS_MAX - len(self.surpplyboxs)

        for i in range(diffSupplyBox):
            position,direction = self.spawnPoints()
            spwanID = d_space_spawns.props["SupplyBox"]

            self.spawnCreatEntity(spwanID,position,direction)


    def spawnRemovePoint(self,entityID):

        entity = KBEngine.entities.get(entityID, None)

        if entity is None:
            return

        if entity.position in self.spwanPoslist:

            self.spwanPoslist.remove(position)


    def onDestroyTimer(self):
        DEBUG_MSG("Room::onDestroyTimer: %i" % (self.id))
        # 请求销毁引擎中创建的真实空间，在空间销毁后，所有该空间上的实体都被销毁
        self.destroySpace()
    #--------------------------------------------------------------------------------------------
    #                              Callbacks
    #--------------------------------------------------------------------------------------------
    def onTimer(self,id,userArg):
        if SCDefine.TIMER_TYPE_POISON_BEGINE == userArg:
            self.onDestroyTimer()
        elif SCDefine.TIMER_TYPE_SPACE_SPAWN_TICK == userArg:
            self.spawnOnTimer(id)


    def onDestroy(self):
        """
        KBEngine method.
        """
        del KBEngine.globalData['Space_%i' % self.spaceID]
        self.destroySpace()

    def onEnter(self, entityCall):
        """
        defined method.
        进入场景
        """
        self.avatars[entityCall.id] = entityCall
        DEBUG_MSG('Space::onEnter Space entityID = %i.' %  entityCall.id)

    def onLeave(self, entityID):
        """
        defined method.
        离开场景
        """
        DEBUG_MSG('Space::onLeave Space entityID = %i.' % ( entityID))

        if entityID in self.avatars:
            del self.avatars[entityID]

    def onMineDestroy(self,mineID):
        if mineID not in self.mines:
            ERROR_MSG("Space::onMineDestroy: mineID not found %i" % (mineID))
            return

        self.mines.remove(mineID)

        self.spawnRemovePoint(mineID)

    def onSupplyBoxDestroy(self,supplyboxID):
        if supplyboxID not in self.surpplyboxs:
            ERROR_MSG("Space::onSupplyBox: supplyboxID not found %i" % (supplyboxID))
            return

        self.surpplyboxs.remove(supplyboxID)
        self.spawnRemovePoint(supplyboxID)

