using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KBEngine;
using System;
using UnityEngine.SceneManagement;

public class connectstate : MonoBehaviour
{

    public Font customFont;
    private string labelMsg = "";
    private Color labelColor = Color.green;
    private bool startRelogin = false;
    public  int ui_state = 0;

    public virtual void installEvents()
    {
        // common
        KBEngine.Event.registerOut("onDisconnected", this, "onDisconnected");
        KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
        KBEngine.Event.registerIn("onConnectionState", this, "onConnectionState");
        KBEngine.Event.registerOut("onKicked", this, "onKicked");
        
    }

    protected virtual void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
        KBEngine.Event.deregisterIn(this);
    }

    protected virtual void Awake()
    {
        installEvents();
    }

    void OnGUI()
    {
        GUI.contentColor = labelColor;
        GUI.skin.label.font = customFont;
        GUI.Label(new Rect((Screen.width / 2) - 100, 150, 400, 100), labelMsg);
    }
    // Update is called once per frame

    public void err(string s)
    {
        labelColor = Color.red;
        labelMsg = s;
    }

    public void info(string s)
    {
        labelColor = Color.green;
        labelMsg = s;
    }

    protected virtual void SavePalyerData()
    {

    }

    public void onDisconnected()
    {
        err("disconnect! will try to reconnect...(你已掉线，尝试重连中!)");
        Invoke("onReloginBaseappTimer", 1.0f);
    }
    public void onReloginBaseappTimer()
    {
        if (ui_state == 0)
        {
            err("disconnect! (你已掉线!)");
            return;
        }

        KBEngineApp.app.reloginBaseapp();

        if (startRelogin)
            Invoke("onReloginBaseappTimer", 3.0f);
    }

    public void onConnectionState(bool success)
    {
        if (!success)
            err("connect(" + KBEngineApp.app.getInitArgs().ip + ":" + KBEngineApp.app.getInitArgs().port + ") is error! (连接错误)");
        else
            info("connect successfully, please wait...(连接成功，请等候...)");
    }


    public void onKicked(UInt16 failedcode)
    {
        err("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(failedcode));

        StartCoroutine(reLogin());
        
     
    }

    IEnumerator reLogin()
    {
        yield return null;

        SceneManager.LoadScene("loginscene");
    }
}

