using CONF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test8 : MonoBehaviour {

	// Use this for initialization
    public Dictionary<UInt32, object> table;

    public conf_bullet bullet_conf;
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
        if(!bullet_conf.init)
        {
            bullet_conf = (conf_bullet)CSVHelper.Instance.GetItem("conf_bullet", 10001);
        }

        if(table == null)
        {
            table = CSVHelper.Instance.GetTable("conf_bullet");
        }
	}
}
