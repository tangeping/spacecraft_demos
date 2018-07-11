# -*- coding: utf-8 -*-
import KBEngine

from KBEDebug import *


# 服务端timer定义
TIMER_TYPE_BUFF_TICK								= 1 # buff的tick
TIMER_TYPE_SPACE_SPAWN_TICK							= 2 # space出生怪
TIMER_TYPE_CREATE_SPACES							= 3 # 创建space
TIMER_TYPE_DESTROY									= 4 # 延时销毁entity
TIMER_TYPE_HEARDBEAT								= 5	# 心跳
TIMER_TYPE_FIGTH_WATI_INPUT_TIMEOUT					= 6	# 战斗回合等待用户输入超时
TIMER_TYPE_SPAWN									= 7	# 出生点出生timer
TIMER_TYPE_DESTROY									= 8	# entity销毁
TIMER_TYPE_POISON_BEGINE							= 9	# 毒圈开始缩小
TIMER_TYPE_WEAPON_DURATION                          = 10 #武器飞行的时间
TIMER_TYPE_WEAPON_CD                                = 11 #武器CD冷却的时间
TIMER_TYPE_WEAPON_IN_WORLD                              = 12 #武器在inworld中初始化好了
TIMER_TYPE_PROP_IN_WORLD                              = 13 #武器在inworld中初始化好了
TIMER_TYPE_PROP_BLANCE_MASS                          = 14 #在space 定时创建一些道具


# 产生毒圈的秒数（秒）
POISON_CREATE_TIME = 720 #60 * 12
WEAPON_DESTORY_TIME = 10 # 60 * 0.1
PROP_BLANCE_TIME = 20