using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class test9 : MonoBehaviour {

    // Use this for initialization
    GameObject missile;
    public Vector3 position = Vector3.zero;
    public Vector3 EulerAngles = Vector3.zero;
    public Vector3 LocalPosition = Vector3.zero;
    public Vector3 LocalEulerAngles = Vector3.zero;
	void Start () {
        missile = Trans.FindObj(gameObject, "spacecraft04_paodan");

        position = missile.transform.position;
        EulerAngles = missile.transform.eulerAngles;

        LocalPosition = missile.transform.localPosition;
        LocalEulerAngles = missile.transform.localEulerAngles;


        Debug.Log("missile."+"position:" +missile.transform.position
            + "eulerAngles:" + missile.transform.eulerAngles 
//            + ",rotation.eulerAngles:" + missile.transform.rotation.eulerAngles
            + ",rotation:" + missile.transform.rotation
            + "localPosition:" + missile.transform.localPosition
            + "localEulerAngles:" + missile.transform.localEulerAngles
            + "localRotation:" + missile.transform.localRotation
            );

         // 获取原生值
         System.Type transformType = missile.transform.GetType();
         PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
         object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(missile.transform, null);
         MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
         object value = m_methodInfo_GetLocalEulerAngles.Invoke(missile.transform, new object[] { m_OldRotationOrder });
        
 
         Debug.LogError("反射调用GetLocalEulerAngles方法获得的值：" + value.ToString());
         Debug.LogError("transform.localEulerAngles获取的值：" + missile.transform.localEulerAngles.ToString());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
