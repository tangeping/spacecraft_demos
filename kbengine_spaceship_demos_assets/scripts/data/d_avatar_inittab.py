# -*- coding: utf-8 -*-

datas ={
1: {'dexterity': 10, 'mp_max': 97, 'money': 0, 'exp': 10, 'sex': 1, 'spaceUType': 1, 'defense': 19, 'anger': 0, 'speed': 11, 'intellect': 5,'modelID': 90000001, 'strength': 15, 'constitution': 13, 'magic_damage': 15, 'spawnPos': (0.0, 0.0, 0.0), 'stamina': 12, 'potential': 0, 'role': 1, 'mp': 97, 'dodge': 20, 'modelScale': 1, 'hp': 158, 'anger_max': 150, 'moveSpeed': 60, 'spawnYaw': 0, 'damage': 48, 'hp_max': 158, 'level': 0, 'energy_max': 50, 'hitval': 55, 'race': 1, 'magic_defense': 15, 'exp': 0},
2: {'dexterity': 10, 'mp_max': 97, 'money': 0, 'exp': 10, 'sex': 1, 'spaceUType': 1, 'defense': 19, 'anger': 0, 'speed': 11, 'intellect': 5,'modelID': 90000002, 'strength': 15, 'constitution': 13, 'magic_damage': 15, 'spawnPos': (0.0, 0.0, 0.0), 'stamina': 12, 'potential': 0, 'role': 1, 'mp': 97, 'dodge': 20, 'modelScale': 1, 'hp': 258, 'anger_max': 150, 'moveSpeed': 60, 'spawnYaw': 0, 'damage': 48, 'hp_max': 258, 'level': 0, 'energy_max': 50, 'hitval': 55, 'race': 1, 'magic_defense': 15, 'exp': 0},
3: {'dexterity': 10, 'mp_max': 97, 'money': 0, 'exp': 10, 'sex': 1, 'spaceUType': 1, 'defense': 19, 'anger': 0, 'speed': 11, 'intellect': 5,'modelID': 90000003, 'strength': 15, 'constitution': 13, 'magic_damage': 15, 'spawnPos': (0.0, 0.0, 0.0), 'stamina': 12, 'potential': 0, 'role': 1, 'mp': 97, 'dodge': 20, 'modelScale': 1, 'hp': 358, 'anger_max': 150, 'moveSpeed': 60, 'spawnYaw': 0, 'damage': 48, 'hp_max': 358, 'level': 0, 'energy_max': 50, 'hitval': 55, 'race': 1, 'magic_defense': 15, 'exp': 0},
4: {'dexterity': 10, 'mp_max': 97, 'money': 0, 'exp': 10, 'sex': 1, 'spaceUType': 1, 'defense': 19, 'anger': 0, 'speed': 11, 'intellect': 5,'modelID': 90000004, 'strength': 15, 'constitution': 13, 'magic_damage': 15, 'spawnPos': (771.5861, 211.0021, 776.5501), 'stamina': 12, 'potential': 0, 'role': 1, 'mp': 97, 'dodge': 20, 'modelScale': 1, 'hp': 458, 'anger_max': 150, 'moveSpeed': 60, 'spawnYaw': 0, 'damage': 48, 'hp_max': 458, 'level': 0, 'energy_max': 50, 'hitval': 55, 'race': 1, 'magic_defense': 15, 'exp': 0},
5: {'dexterity': 10, 'mp_max': 97, 'money': 0, 'exp': 10, 'sex': 1, 'spaceUType': 1, 'defense': 19, 'anger': 0, 'speed': 11, 'intellect': 5,'modelID': 90000005, 'strength': 15, 'constitution': 13, 'magic_damage': 15, 'spawnPos': (771.5861, 211.0021, 776.5501), 'stamina': 12, 'potential': 0, 'role': 1, 'mp': 97, 'dodge': 20, 'modelScale': 1, 'hp': 558, 'anger_max': 150, 'moveSpeed': 60, 'spawnYaw': 0, 'damage': 48, 'hp_max': 558, 'level': 0, 'energy_max': 50, 'hitval': 55, 'race': 1, 'magic_defense': 15, 'exp': 0}
}

default_avatars ={
    1: { 'roleType':1,'name':'primary','roleTypeName':'ship','level': 1},
    2: { 'roleType':2,'name':'middle','roleTypeName':'ship' ,'level': 2},
    3: { 'roleType':3,'name':'advanced','roleTypeName':'ship','level': 3}
}

# 升级需要的物品表
upgrade_props = {
    1   :{ 'exp': 200 , 'red_stone': 2,'green_stone':2,'bule_stone':1},
    2   :{ 'exp': 400 , 'red_stone': 2,'green_stone':2,'bule_stone':1},
    3   :{ 'exp': 600 , 'red_stone': 2,'green_stone':2,'bule_stone':1},
    4   :{ 'exp': 800 , 'red_stone': 5,'green_stone':5,'bule_stone':1},
    5   :{ 'exp': 800 , 'red_stone': 5,'green_stone':5,'bule_stone':1},
    6   :{ 'exp': 800 , 'red_stone': 5,'green_stone':5,'bule_stone':1},
    7   :{ 'exp': 800 , 'red_stone': 10,'green_stone':10,'bule_stone':1},
    8   :{ 'exp': 800 , 'red_stone': 10,'green_stone':10,'bule_stone':1},
    9   :{ 'exp': 800 , 'red_stone': 20,'green_stone':10,'bule_stone':1},
    10  :{ 'exp': 800 , 'red_stone':20,'green_stone':20,'bule_stone':1}
}

allDatas ={
	'人物初始化': datas,
    '人物升级 ' : upgrade_props,
}