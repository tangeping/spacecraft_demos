# -*- coding: utf-8 -*-
import KBEngine
import GameConfigs
import d_entities
import d_avatar_inittab
from KBEDebug import *
from interfaces.CombatPropertys import CombatPropertys

class Combat(CombatPropertys):
    """
    关于战斗的一些功能
    """
    def __init__(self):
        CombatPropertys.__init__(self)

    def canUpgrade(self):
        """
        virtual method.
        """
        return True

    def upgrade(self):
        """
        for real
        """
        if self.canUpgrade():
            self.addLevel(1)

    def addLevel(self, lv):
        """
        for real
        """
        self.level += lv
        self.onLevelChanged(lv)

    def isDead(self):
        """
        """
        return self.state == GameConfigs.ENTITY_STATE_DEAD

    def die(self, killerID):
        """
        """
        if self.isDestroyed or self.isDead():
            return

        if killerID == self.id:
            killerID = 0

        INFO_MSG("%s::die: %i i die. killerID:%i." % (self.getScriptName(), self.id, killerID))
        killer = KBEngine.entities.get(killerID)
        if killer:
            killer.onKiller(self.id)

        self.onBeforeDie(killerID)
        self.onDie(killerID)
        self.changeState(GameConfigs.ENTITY_STATE_DEAD)
        self.onAfterDie(killerID)

    def canDie(self, attackerID, skillID, damageType, damage):
        """
        virtual method.
        是否可死亡
        """
        return True

    def addEffect(self, propid, proptype):

        if self.isDestroyed or self.isDead():
            return

        attri = d_entities.datas[propid]
        if attri is None:
            return

        keys = {'hpDamage','mpDamage','hpAdd','mpAdd','expAdd'}
        effects = {k:v for k,v in attri.items() if k in keys}

        for key,value in effects.items():

            if key == "hpDamage":
                self.onHpChanged(-value)
            elif key == "mpDamage":
                self.onMpChanged(-value)
            elif key == "hpAdd":
                self.onHpChanged(value)
            elif key == "mpAdd":
                self.onMpChanged(value)
            elif key == "expAdd":
                self.onExpChanged(value)


    def recvPropEffect(self, propID, propType):
        """
        defined.
        """
        if self.isDestroyed or self.isDead():
            return

        self.allClients.recvPropEffect(propID, propType)

        DEBUG_MSG("%s::recvPropEffect: %i propID=%i, propType=%i" % (self.getScriptName(), self.id, propID, propType))

        self.addEffect(propID, propType)


    def recvDamage(self, attackerID, skillID, damageType, damage):
        """
        defined.
        """
        if self.isDestroyed or self.isDead():
            return

        self.addEnemy(attackerID, damage)

        self.allClients.recvDamage(attackerID, skillID, damageType, damage)

        DEBUG_MSG("%s::recvDamage: %i attackerID=%i, skillID=%i, damageType=%i, damage=%i" % \
            (self.getScriptName(), self.id, attackerID, skillID, damageType, damage))

        if self.HP <= damage:
            if self.canDie(attackerID, skillID, damageType, damage):
                self.die(attackerID)
        else:
            self.setHP(self.HP - damage)



    def addEnemy(self, entityID, enmity):
        """
        defined.
        添加敌人
        """
        if entityID in self.enemyLog:
            return

        DEBUG_MSG("%s::addEnemy: %i entity=%i, enmity=%i" % \
                        (self.getScriptName(), self.id, entityID, enmity))

        self.enemyLog.append(entityID)
        self.onAddEnemy(entityID)

    def removeEnemy(self, entityID):
        """
        defined.
        删除敌人
        """
        DEBUG_MSG("%s::removeEnemy: %i entity=%i" % \
                        (self.getScriptName(), self.id, entityID))

        self.enemyLog.remove(entityID)
        self.onRemoveEnemy(entityID)

        if len(self.enemyLog) == 0:
            self.onEnemyEmpty()

    def checkInTerritory(self):
        """
        virtual method.
        检查自己是否在可活动领地中
        """
        return True

    def checkEnemyDist(self, entity):
        """
        virtual method.
        检查敌人距离
        """
        dist = entity.position.distTo(self.position)
        if dist > 30.0:
            INFO_MSG("%s::checkEnemyDist: %i id=%i, dist=%f." % (self.getScriptName(), self.id, entity.id, dist))
            return False

        return True

    def checkEnemys(self):
        """
        检查敌人列表
        """
        for idx in range(len(self.enemyLog) - 1, -1, -1):
            entity = KBEngine.entities.get(self.enemyLog[idx])
            if entity is None or entity.isDestroyed or entity.isDead() or \
                not self.checkInTerritory() or not self.checkEnemyDist(entity):
                self.removeEnemy(self.enemyLog[idx])

    #--------------------------------------------------------------------------------------------
    #                              Callbacks
    #--------------------------------------------------------------------------------------------
    def onHpChanged(self,addhp):
        """
        virtual method.
        """
        hp = self.HP + addhp
        if hp <= 0:
            self.die(self.id)
        else:
            self.addHP(addhp)

    def onMpChanged(self,addmp):
        """
        virtual method.
        """
        mp = self.MP + addmp
        if mp <= 0:
            self.setMP(0)
        else:
            self.addMP(addmp)

    def onExpChanged(self,addexp):
        """
        virtual method.
        """
        exp = self.EXP + addexp
        if exp <= 0:
            self.setEXP(0)
        else:
            self.addEXP(addexp)


    def onLevelChanged(self, addlv):
        """
        virtual method.
        """
        level = self.level + addlv
        props = self.getLvPops()

        if props is None:
            return

        self.EXP_Max = props.get("exp",0)
        pass

    def onDie(self, killerID):
        """
        virtual method.玩家已经挂了,销毁cell部分，
        """
        self.setHP(0)
        self.setMP(0)


    def onBeforeDie(self, killerID):
        """
        virtual method.
        """
        self.addRound(1)
        self.addDieCount(1)

        pass

    def onAfterDie(self, killerID):
        """
        virtual method.
        """
        self.fullPower()
        self.destroy()
        pass

    def onKiller(self, entityID):
        """
        defined.
        我击杀了entity
        """
        self.addKillCount(1)
        self.addScore(self.level * 10)
        pass

    def onDestroy(self):
        """
        entity销毁
        """
        pass

    def onAddEnemy(self, entityID):
        """
        virtual method.
        有敌人进入列表
        """
        pass

    def onRemoveEnemy(self, entityID):
        """
        virtual method.
        删除敌人
        """
        pass

    def onEnemyEmpty(self):
        """
        virtual method.
        敌人列表空了
        """
        pass
