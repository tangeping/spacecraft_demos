namespace KBEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;


    public class Avatar : AvatarBase
    {
        public Dictionary<UInt64, AVATAR_INFOS> avatars = new Dictionary<UInt64, AVATAR_INFOS>();
        // Use this for initialization

        public Avatar() : base()
        {

        }

        public override void __init__()
        {
            // 由于任何玩家被同步到该客户端都会使用这个模块创建，因此客户端可能存在很多这样的实体
            // 但只有一个是自己的玩家实体，所以需要判断一下
            if (isPlayer())
            {
                Event.registerIn("relive", this, "relive");
                Event.registerIn("jump", this, "jump");
                Event.registerIn("updatePlayer", this, "updatePlayer");
                Event.registerIn("useWeapon", this, "useWeapon");
                Event.registerIn("transAvatar", this, "transAvatar");
                Event.registerIn("updateCruiseSpeed", this, "updateCruiseSpeed");

                Debug.Log("Avatar::__init__,position:" + position + ",direction:" + direction);
            }
        }

        public override void onDestroy()
        {
            if (isPlayer())
            {
                KBEngine.Event.deregisterIn(this);
            }
            Debug.Log(name + "::onDestroy()");
        }

        public void transAvatar()
        {
            Debug.Log("Avatar::transAvatar");
            baseEntityCall.transAvatar();
        }

        public void relive(Byte type)
        {

            cellEntityCall.relive(type);
        }

        public void jump()
        {
            cellEntityCall.jump();
        }

        public virtual void updateCruiseSpeed(UINT16 speed)
        {
            cellEntityCall.setCruiseSpeed(speed);
//            Debug.Log(className + "::updateCruiseSpeed: " + id + ",speed:" + speed);
        }

        public void useWeapon(Vector3 position, Vector3 direction,Vector3 forward,UInt32 weaponID)
        {
            Debug.Log(className + ".id:" + id + ",position:" + position + ",direction:" + direction +
                ",forward:" + forward + ",weaponId:" + weaponID + "ToRotation:"+ Trans.ToRotation(direction));

            cellEntityCall.reqUseWeapon(position, Trans.ToRotation(direction), forward, weaponID);
        }

        public override void onLevelChanged(UInt16 oldValue)
        {
            Event.fireOut("set_level", new object[] { this, level });
        }

        public override void onEXPChanged(Int32 oldValue)
        {
            Event.fireOut("set_EXP", new object[] { this, EXP });
        }
        public override void onEXP_MaxChanged(Int32 oldValue)
        {
            Event.fireOut("set_EXP_Max", new object[] { this, EXP_Max });
        }
        public override void onModelIDChanged(UInt32 oldValue)
        {
            // Debug.Log(className + "::set_modelID: " + old + " => " + v); 
            Event.fireOut("set_modelID", new object[] { this, modelID });
        }

        public override void onModelScaleChanged(Byte oldValue)
        {
            // Debug.Log(className + "::set_modelScale: " + old + " => " + v); 
            Event.fireOut("set_modelScale", new object[] { this, modelScale });
        }

        public override void onNameChanged(string oldValue)
        {
            // Debug.Log(className + "::set_name: " + old + " => " + v); 
            Event.fireOut("set_name", new object[] { this, name });
        }


        public override void onHPChanged(Int32 oldValue)
        {
            Event.fireOut("set_HP", new object[] { this, HP, HP_Max });
        }

        public override void onHP_MaxChanged(Int32 oldValue)
        {
            Event.fireOut("set_HP_Max", new object[] { this, HP_Max ,HP});
        }

        public override void onMPChanged(Int32 oldValue)
        {
            Event.fireOut("set_MP", new object[] { this, MP,MP_Max });
        }

        public override void onMP_MaxChanged(Int32 oldValue)
        {
            Event.fireOut("set_MP_Max", new object[] { this, MP_Max ,MP});
        }

        public override void onCruiseSpeedChanged(UInt16 oldValue)
        {
 //           Debug.Log(className + "::set_cruiseSpeed: " + oldValue + " => " + cruiseSpeed);
            Event.fireOut("set_cruiseSpeed", new object[] { this,cruiseSpeed});
        }

        public override void onMoveSpeedChanged(UInt16 oldValue)
        {
 //           Debug.Log(className + "::set_moveSpeed: " + oldValue + " => " + moveSpeed);
            Event.fireOut("set_moveSpeed", new object[] { this, moveSpeed });
        }

        public override void onSpaceUTypeChanged(UInt32 oldValue)
        {
            Event.fireOut("set_spaceUType", new object[] { this, spaceUType });
        }

        public override void onUidChanged(UInt32 oldValue)
        {
            Event.fireOut("set_uid", new object[] { this, uid });
        }

        public override void onUtypeChanged(UInt32 oldValue)
        {
            Event.fireOut("set_utype", new object[] { this, utype });
        }
        public override void onStateChanged(SByte oldValue)
        {
            Event.fireOut("set_state", new object[] { this, state });
        }
        public override void onJump()
        {
//            Debug.Log(className + "::onJump: " + id);
            Event.fireOut("otherAvatarOnJump", new object[] { this });
        }

        public override void onKill_CountChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onKill_CountChanged: " + oldValue + " => " + Kill_Count);
        }


        public override void onRankChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onRankChanged: " + oldValue + " => " + Rank);
        }

        public override void onRoundChanged(Int32 oldValue)
        {
 //           Debug.Log(className + "::onRoundChanged: " + oldValue + " => " + Round);
        }

        public override void onRound_MaxChanged(Int32 oldValue)
        {
 //           Debug.Log(className + "::onRound_MaxChanged: " + oldValue + " => " + Round_Max);
        }
  
        public override void onScoreChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onScoreChanged: " + oldValue + " => " + Score);
        }

        public override void onDie_CountChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onDie_CountChanged: " + oldValue + " => " + Die_Count);
        }

        public override void recvDamage(Int32 attackerID, Int32 weaponID, Int32 damage_type, Int32 damage)
        {
 //           Debug.Log(className + "::recvDamage: " + id + ",attackerID:" + attackerID +
 //               ",weaponID:"+ weaponID+ ",damage_type:" + damage_type + ",damage:" + damage);
            Event.fireOut("recvDamage", new object[] { this, attackerID, weaponID, damage_type, damage });
        }

        public override void recvPropEffect(UInt32 propID, Int32 propType)
        {
 //           Debug.Log(className + "::recvPropEffect: " + id + ",propID:" + propID + ",propType:" + propType);
            Event.fireOut("recvPropEffect", new object[] { this, propID, propType });
        }

        public override void canUseWeaponResult(UInt16 cooltime, Byte haveWeapon)
        {
//            Debug.Log(className + "::recvDamage: " + id);
            Event.fireOut("canUseWeaponResult", new object[] { this,cooltime,haveWeapon });
        }

        public virtual void updatePlayer(UInt32 currSpaceID, float x, float y, float z, float yaw)
        {
            // 更加安全的更新位置，避免将上一个场景的坐标更新到当前场景中的玩家
            if (currSpaceID > 0 && currSpaceID != KBEngineApp.app.spaceID)
            {
                return;
            }

            position.x = x;
            position.y = y;
            position.z = z;


            direction.y = yaw;
//            Debug.Log(className + "::updatePlayer: " + id + ",position:" + position);
        }

    }
}
