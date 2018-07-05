# -*- coding: utf-8 -*-
import KBEngine
import GameConfigs
from KBEDebug import *

class CombatPropertys:
    """
    所有关于战斗的属性
    完善的话可以根据策划excel表来直接生成这个模块
    """
    def __init__(self):

        DEBUG_MSG("CombatPropertys::__init__:state:%i,HP:%i,MP:%i,HP_Max:%i,MP_Max:%i" % (self.getState(),self.HP,self.MP,self.HP_Max,self.MP_Max))
        # 死亡状态才需要补满
#       if  self.isState(GameConfigs.ENTITY_STATE_DEAD) :
#           self.fullPower()
#        self.HP_Max = self.getDatas()["hp_max"]
#        self.MP_Max = self.getDatas()["mp_max"]

    def fullPower(self):
        """
        """
        self.setHP(self.HP_Max)
        self.setMP(self.MP_Max)

    def addHP(self, val):
        """
        defined.
        """
        v = self.HP + int(val)
        if v < 0:
            v = 0

        if v >= self.HP_Max:
            v = self.HP_Max

        if self.HP == v:
            return

        self.HP = v

    def addMP(self, val):
        """
        defined.
        """
        v = self.MP + int(val)
        if v < 0:
            v = 0

        if v >= self.MP_Max:
            v = self.MP_Max

        if self.MP == v:
            return

        self.MP = v

    def addEXP(self, val):
        """
        defined.
        """
        v = self.EXP + int(val)
        if v < 0:
            v = 0

        upper_exp = self.getUpperExp()
        if upper_exp is None:
            return

        list_exp = list(upper_exp.values())
        lv_max  = list(upper_exp.keys())[-1]

        lv = self.level
        DEBUG_MSG("CombatPropertys::addEXP: lv:%i,lv_max:%i,list_exp:%s" % (lv,lv_max,str(list_exp)))

        for exp in list_exp:
            if v - int(exp) >= 0:
                if lv >= lv_max:
                    lv = lv_max
                    v  = int(exp)
                else:
                    lv += 1
                    v  -= int(exp)

        if lv > self.level:
            self.addLevel(lv - self.level)

        self.EXP = v

        DEBUG_MSG("CombatPropertys::addEXP: EXP:%i,level:%i" % (self.EXP,self.level))

    def setHP(self, hp):
        """
        defined
        """
        hp = int(hp)
        if hp < 0:
            hp = 0

        if self.HP == hp:
            return

        self.HP = hp

    def setMP(self, mp):
        """
        defined
        """
        hp = int(mp)
        if mp < 0:
            mp = 0

        if self.MP == mp:
            return

        self.MP = mp

    def setEXP(self, exp):
        """
        defined
        """
        exp = int(exp)
        if exp < 0:
            exp = 0

        if self.EXP == exp:
            return

        self.EXP = exp

    def setHPMax(self, hpmax):
        """
        defined
        """
        hpmax = int(hpmax)
        self.HP_Max = hpmax

    def setMPMax(self, mpmax):
        """
        defined
        """
        mpmax = int(mpmax)
        self.MP_Max = mpmax


