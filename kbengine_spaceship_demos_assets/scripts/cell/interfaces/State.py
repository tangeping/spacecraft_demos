# -*- coding: utf-8 -*-
#
"""
处理entity的一些状态
"""
import GameConfigs
from KBEDebug import *

class State:
	"""
	"""
	def __init__(self):
		pass


	def initEntity(self):
		"""
		virtual method.
		"""
		pass

	def isState(self, state):
		DEBUG_MSG("State::isState.%i self.state=%i,state=%i " %(self.id,self.state,state))
		return self.state == state


	def getState(self):
		DEBUG_MSG("State::getState.%i self.state=%i" %(self.id,self.state))
		return self.state

	def changeState(self, state):
		"""
		defined
		改变当前主状态
		GlobalDefine.ENTITY_STATE_**
		"""
		DEBUG_MSG("State::changeState.%i self.state=%i,state=%i " %(self.id,self.state,state))
		if self.state != state:
			oldstate = self.state
			self.state = state



