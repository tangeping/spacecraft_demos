namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Weapon : WeaponBase
    {
        public Weapon() : base()
        {

        }

        public override void __init__()
        {

        }

        public override void onAttackerIDChanged(UInt16 oldValue)
        {
 //           Debug.Log(className + "::onAttackerIDChanged: " + id + ",oldValue:"+ oldValue+ " = > attackerID:" + attackerID);
        }
   
        public override void onModelIDChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onModelIDChanged: " + id + ",oldValue:" + oldValue + " = > modelID:" + modelID);
        }

        public override void onModelScaleChanged(Byte oldValue)
        {
 //           Debug.Log(className + "::onModelScaleChanged: " + id + ",oldValue:" + oldValue + " = > modelScale:" + modelScale);
        }

        public override void onMoveSpeedChanged(UInt16 oldValue)
        {
 //           Debug.Log(className + "::onMoveSpeedChanged: " + id + ",oldValue:" + oldValue + " = > moveSpeed:" + moveSpeed);
            Event.fireOut("set_moveSpeed", new object[] { this, moveSpeed });
        }

        public override void onCruiseSpeedChanged(UInt16 oldValue)
        {
 //           Debug.Log(className + "::set_cruiseSpeed: " + oldValue + " => " + cruiseSpeed);
            Event.fireOut("set_cruiseSpeed", new object[] { this, cruiseSpeed });
        }

        public override void onNameChanged(string oldValue)
        {
 //           Debug.Log(className + "::onNameChanged: " + id + ",oldValue:" + oldValue + " = > name:" + name);
        }

        public override void onOwnerIDChanged(UInt16 oldValue)
        {
 //           Debug.Log(className + "::onOwnerIDChanged: " + id + ",oldValue:" + oldValue + " = > ownerID:" + ownerID);
        }

        public override void onSpaceUTypeChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onSpaceUTypeChanged: " + id + ",oldValue:" + oldValue + " = > spaceUType:" + spaceUType);
        }

        public override void onUidChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onUidChanged: " + id + ",oldValue:" + oldValue + " = > uid:" + uid);
        }
    
        public override void onUtypeChanged(UInt32 oldValue)
        {
 //           Debug.Log(className + "::onUtypeChanged: " + id + ",oldValue:" + oldValue + " = > utype:" + utype);
        }

        public override void onCDChanged(UInt16 oldValue)
        {
 //           Debug.Log(className + "::onCDChanged: " + id + ",oldValue:" + oldValue + " = > CD:" + CD);
        }

        public override void onStateChanged(SByte oldValue)
        {
 //           Debug.Log(className + "::onStateChanged: " + id + ",oldValue:" + oldValue + " = > state:" + state);
        }

        public override void onWeaponDestroy(UInt16 explodetime)
        {
            Debug.Log(className + "::onWeaponDestroy: " + id + ",explodetime:"+ explodetime);
            Event.fireOut("onWeaponDestroy", new object[] { this, explodetime });
        }
    }
}
