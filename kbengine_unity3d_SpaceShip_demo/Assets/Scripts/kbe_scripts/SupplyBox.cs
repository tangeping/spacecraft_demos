namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SupplyBox : SupplyBoxBase
    {
        public SupplyBox() : base()
        {

        }

        public override void onModelIDChanged(UInt32 oldValue)
        {
            //             Debug.Log(className + "::modelID: " + oldValue + " => " + name);
            //             Event.fireOut("set_modelID", new object[] { this, modelID });
        }

        public override void onModelScaleChanged(Byte oldValue)
        {
            //             Debug.Log(className + "::set_modelScale: " + oldValue + " => " + modelScale);
            //             Event.fireOut("set_modelScale", new object[] { this, modelScale });
        }

        public override void onNameChanged(string oldValue)
        {
            Debug.Log(className + "::set_name: " + oldValue + " => " + name);
            Event.fireOut("set_name", new object[] { this, name });
        }

        public override void onSpaceUTypeChanged(UInt32 oldValue)
        {
            //           Debug.Log(className + "::set_spaceutype: " + oldValue + " => " + spaceUType);
            //            Event.fireOut("set_spaceutype", new object[] { this, spaceUType });
        }

        public override void onSupplyBoxDestroy(Int32 CollisionID, UInt16 explodetime)
        {
            Debug.LogError(className + "::explodetime: " + explodetime + ",CollisionID:"+ CollisionID);

            Event.fireOut("onSupplyBoxDestroy", new object[] { this, CollisionID,explodetime });
        }

        public override void onUidChanged(UInt32 oldValue)
        {
            Debug.Log(className + "::set_uid: " + oldValue + " => " + uid);
            Event.fireOut("set_uid", new object[] { this, uid });
        }

        public override void onUtypeChanged(UInt32 oldValue)
        {
            //             Debug.Log(className + "::set_utype: " + oldValue + " => " + utype);
            //             Event.fireOut("set_utype", new object[] { this, utype });
        }
    }
}
