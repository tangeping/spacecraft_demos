# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import GameConfigs

class SettlePropertys:
    """
    所有结算的属性
    完善的话可以根据策划excel表来直接生成这个模块
    """
    def __init__(self):

        self.resetSettle()
        self.Round_Max = GameConfigs.SPACE_ROUND_MAX
        DEBUG_MSG("SettlePropertys::__init__:Round:%i,Round_Max:%i,Rank:%i" % (self.Round,self.Round_Max,self.Rank))


    def setRound(self, val):
        """
        defined.
        """
        v = int(val)
        if v < 0:
            v = 0

        if self.Round == v:
            return

        self.Round = v

    def addRound(self, val):
        """
        defined.
        """
        v = self.Round + int(val)
        if v < 0:
            v = 0

        if self.Round == v:
            return

        self.Round = v

    def setRoundMax(self, val):
        """
        defined.
        """
        v =  int(val)
        if v < 0:
            v = 0

        if self.Round_Max == v:
            return

        self.Round_Max = v

    def setRank(self, val):
        """
        defined
        """
        v = int(val)
        if v < 0:
            v = 0

        if self.Rank == v:
            return

        self.Rank = v

    def setScore(self, val):
        """
        defined
        """
        v = int(val)
        if v < 0:
            v = 0

        if self.Score == v:
            return

        self.Score = v

    def addScore(self, val):
        """
        defined.
        """
        v = self.Score + int(val)
        if v < 0:
            v = 0

        if self.Score == v:
            return

        self.Score = v

    def setDieCount(self, val):
        """
        defined
        """
        v = int(val)
        if v < 0:
            v = 0

        if self.Die_Count == v:
            return

        self.Die_Count = v

    def addDieCount(self, val):
        """
        defined.
        """
        v = self.Die_Count + int(val)
        if v < 0:
            v = 0

        if self.Die_Count == v:
            return

        self.Die_Count = v

    def setKillCount(self, val):
        """
        defined
        """
        v = int(val)
        if v < 0:
            v = 0

        if self.Kill_Count == v:
            return

        self.Kill_Count = v

    def addKillCount(self, val):
        """
        defined.
        """
        v = self.Kill_Count + int(val)
        if v < 0:
            v = 0

        if self.Kill_Count == v:
            return

        self.Kill_Count = v

    def  resetSettle(self):

        self.Round = 0
        self.Round_Max = 0
        self.Rank = 0
        self.Score = 0
        self.Die_Count=0
        self.Kill_Count=0


