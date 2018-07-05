using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test4 : MonoBehaviour {

    // Use this for initialization
    public GameObject ship_frame;

    public static Transform FindTransform(Transform trans,string goName)
    {
        Transform child = trans.Find(goName);
        if (child != null)
            return child;

        Transform go = null;
        for(int i = 0;i<trans.childCount;i++)
        {
            child = trans.GetChild(i);
            go = FindTransform(child, goName);
            if (go != null)
                return go;
        }

        return null;
    }

    public static GameObject FindObj(GameObject obj,string name)
    {
        Transform tranObj = FindTransform(obj.transform, name);

        return tranObj == null ? null : tranObj.gameObject;
    }

    void Start () {

        ship_frame = FindObj(gameObject,"spacecraft02_zhuti");
        if (ship_frame == null)
        {
            Debug.LogError("not found ship_frame!!!");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
