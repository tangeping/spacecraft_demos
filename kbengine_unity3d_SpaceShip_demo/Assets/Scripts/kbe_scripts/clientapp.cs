using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using UnityEngine.SceneManagement;
using System;

public class clientapp : KBEMain
{
    public static clientapp ins = null;

    public static clientapp GetInstance()
    {
        if (ins == null)
        {
            GameObject kbe_clientapp = new GameObject("kbe_clientapp");
            ins = kbe_clientapp.AddComponent<clientapp>();
        }
        return ins;
    }

    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CSVHelper.Instance.ReadExcelAllFile());
    }

}
