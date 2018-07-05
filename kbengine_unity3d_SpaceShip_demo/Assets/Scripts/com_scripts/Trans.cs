using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine
{
    public class Trans
    {
        public static Vector3 ToRotation(Vector3 Angles)
        {
            double x = ((double)Angles.x / 360 * (System.Math.PI * 2));
            double y = ((double)Angles.y / 360 * (System.Math.PI * 2));
            double z = ((double)Angles.z / 360 * (System.Math.PI * 2));
				
				// 根据弧度转角度公式会出现负数
				// unity会自动转化到0~360度之间，这里需要做一个还原
				if(x - System.Math.PI > 0.0)
					x -= System.Math.PI* 2;

				if(y - System.Math.PI > 0.0)
					y -= System.Math.PI* 2;
				
				if(z - System.Math.PI > 0.0)
					z -= System.Math.PI* 2;
            return new Vector3((float)x, (float)y, (float)z);
        }

        public static Vector3 ToAngles(Vector3 Angles)
        {
            double x = Angles.x * 360 / ((float)System.Math.PI * 2);
            double y = Angles.y * 360 / ((float)System.Math.PI * 2);
            double z = Angles.z * 360 / ((float)System.Math.PI * 2);

            return new Vector3((float)x, (float)y, (float)z);
        }

        public static Transform FindTransform(Transform trans, string goName)
        {
            Transform child = trans.Find(goName);
            if (child != null)
                return child;

            Transform go = null;
            for (int i = 0; i < trans.childCount; i++)
            {
                child = trans.GetChild(i);
                go = FindTransform(child, goName);
                if (go != null)
                    return go;
            }

            return null;
        }

        public static GameObject FindObj(GameObject obj, string name)
        {
            Transform tranObj = FindTransform(obj.transform, name);

            return tranObj == null ? null : tranObj.gameObject;
        }

    }

}

