using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KBEngine;
using System;

public class loginUI : connectstate {

    // Use this for initialization
    private InputField accoutName;
    private InputField password;

    public loginUI() : base()
    {
    }

    public override void installEvents()
    {
        base.installEvents();
        // login
        KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onVersionNotMatch", this, "onVersionNotMatch");
        KBEngine.Event.registerOut("onScriptVersionNotMatch", this, "onScriptVersionNotMatch");
        KBEngine.Event.registerOut("onLoginBaseappFailed", this, "onLoginBaseappFailed");
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
        KBEngine.Event.registerOut("onReloginBaseappFailed", this, "onReloginBaseappFailed");
        KBEngine.Event.registerOut("onReloginBaseappSuccessfully", this, "onReloginBaseappSuccessfully");
        KBEngine.Event.registerOut("onLoginBaseapp", this, "onLoginBaseapp");
        KBEngine.Event.registerOut("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
        KBEngine.Event.registerOut("Baseapp_importClientMessages", this, "Baseapp_importClientMessages");
        KBEngine.Event.registerOut("Baseapp_importClientEntityDef", this, "Baseapp_importClientEntityDef");
    }

    protected override void Awake()
    {
        clientapp.GetInstance();
        base.Awake();
    }

    protected override void OnDestroy()
    {
        Debug.Log(gameObject.name + " OnDestroy!!");
        base.OnDestroy();
        KBEngine.Event.deregisterOut(this);
    }

    void Start() {

        accoutName = Trans.FindObj( gameObject,"AccountInput").GetComponent<InputField>();
        password = Trans.FindObj( gameObject,"PasswdInput").GetComponent<InputField>();

        if (accoutName == null || password == null)
        {
            Debug.Log("can not found accoutName or password obj!!");
            return;
        }

        Button btn = Trans.FindObj( gameObject,"StartButton").GetComponent<Button>();
        if (btn == null)
        {
            Debug.Log("can not found StartButton!");
        }
        btn.onClick.AddListener(OnClick);

    }
    void OnClick()
    {
        //       SceneManager.LoadScene("demo1");
        if (accoutName.text != "" && password.text != "")
        {
            login();
        }
        Debug.Log("accoutName:" + accoutName.text + ",password:" + password.text);
    }

    // Update is called once per frame
    void Update () {
		
	}

    protected override void SavePalyerData()
    {
        PlayerData.Instance.Account = accoutName.text;
        PlayerData.Instance.Password = password.text;
    }

    public void login()
    {
 //       info("connect to server...(连接到服务端...)");
        KBEngine.Event.fireIn("login", accoutName.text, password.text, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
    }
    public void onCreateAccountResult(UInt16 retcode, byte[] datas)
    {
        if (retcode != 0)
        {
            err("createAccount is error(注册账号错误)! err=" + KBEngineApp.app.serverErr(retcode));
            return;
        }
    }

    public void onLoginFailed(UInt16 failedcode)
    {
        if (failedcode == 20)
        {
            err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
        }
        else
        {
            err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode));
        }
    }

    public void onVersionNotMatch(string verInfo, string serVerInfo)
    {
        err("");
    }

    public void onScriptVersionNotMatch(string verInfo, string serVerInfo)
    {
        err("");
    }

    public void onLoginBaseappFailed(UInt16 failedcode)
    {
        err("loginBaseapp is failed(登陆网关失败), err=" + KBEngineApp.app.serverErr(failedcode));
    }

    public void onLoginBaseapp()
    {
        info("connect to loginBaseapp, please wait...(连接到网关， 请稍后...)");
    }

    public void onReloginBaseappFailed(UInt16 failedcode)
    {
        err("relogin is failed(重连网关失败), err=" + KBEngineApp.app.serverErr(failedcode));

    }

    public void onReloginBaseappSuccessfully()
    {
        info("relogin is successfully!(重连成功!)");

    }

    public void onLoginSuccessfully(UInt64 rndUUID, Int32 eid, Account accountEntity)
    {
        info("login is successfully!(登陆成功!)");

        SavePalyerData();

        SceneManager.LoadScene("pickscene");
    }



    public void Loginapp_importClientMessages()
    {
        info("Loginapp_importClientMessages ...");
    }

    public void Baseapp_importClientMessages()
    {
        info("Baseapp_importClientMessages ...");
    }

    public void Baseapp_importClientEntityDef()
    {
        info("importClientEntityDef ...");
    }


}
