using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class select_avatar : connectstate
{

    // Use this for initialization
    private const float reinforce = 5.0f;

    public float beignAngle;
    public float endAngle;
    
    private const float minTurnAngle = 60.0f;
    private const float nextTurnAngle = 120.0f;
    private const int spaceshipCount = 3;

    private Dictionary<UInt64, AVATAR_INFOS> ui_avatarList = null;

    private int selectIndex = 0;
    
    private UInt64 selAvatarDBID = 0;

    public select_avatar() : base()
    {
    }
    public override void installEvents()
    {
        base.installEvents();
        KBEngine.Event.registerOut("onReqAvatarList", this, "onReqAvatarList");
        KBEngine.Event.registerOut("onCreateAvatarResult", this, "onCreateAvatarResult");
        KBEngine.Event.registerOut("onRemoveAvatar", this, "onRemoveAvatar");
    }

    protected override void Awake()
    {
        beignAngle = transform.eulerAngles.y;
        endAngle = beignAngle;
        selectIndex = 0;

        installEvents();
    }

    protected override void OnDestroy()
    {
        Debug.Log(gameObject.name + " OnDestroy!!");
        base.OnDestroy();
        KBEngine.Event.deregisterOut(this);
    }


    void Start()
    {
        Button btn = Trans.FindObj( gameObject,"start_button").GetComponent<Button>();
        if (btn == null)
        {
            Debug.Log("can not found login_button!");
        }
        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        onSelAvatarDBID();
    }

    void onSelAvatarDBID()
    {
        if (ui_avatarList != null && ui_avatarList.Count > 0)
        {
 //           Debug.Log("ui_avatarList.count:" + ui_avatarList.Count);
            foreach (var item in ui_avatarList)
            {
                AVATAR_INFOS info = item.Value;
                Debug.Log("ui_avatarList.key:" + item.Key + ",bid:" + info.dbid + ",selectIndex:"+ selectIndex);
                if (info.roleType == selectIndex + 1)
                {
                    selAvatarDBID = info.dbid;
                    break;
                }
            }

            Debug.Log("selAvatarDBID:" + selAvatarDBID);

            if (selAvatarDBID > 0)
            {
                info("Please wait...(Loading...)");
                //            KBEngine.Event.fireIn("selectAvatarGame", selAvatarDBID);
                SavePalyerData();
                SceneManager.LoadScene("worldscene");
            }
        }
    }

    void OnGetAvatarList()
    {
        if(gameObject == null || KBEngineApp.app.entity_type != "Account")
        {
            return;
        }
        if(ui_avatarList != null)
        {
            return;
        }

        KBEngine.Account account = (KBEngine.Account)KBEngineApp.app.player();
        if (account != null)
        {
            ui_avatarList = new Dictionary<UInt64, AVATAR_INFOS>(account.avatars);
            reqAvatarList();
        }
    }



    // Update is called once per frame
    void Update()
    {
        float deltaAngle = Mathf.Abs(endAngle - beignAngle);
        if (Input.GetMouseButton(0) && deltaAngle < nextTurnAngle/* || (360 - deltaAngle < nextTurnAngle)*/)
        {
            float slipDegree = -(Input.GetAxis("Mouse X") * reinforce) % 360;

            endAngle += slipDegree;
            transform.eulerAngles +=  new Vector3(0, slipDegree, 0);
//            Debug.Log("slipDegree:" + slipDegree+ ",eulerAngles.y:" + transform.eulerAngles.y);

        }

        if (Input.GetMouseButtonUp(0))
        {
            float slipDegree =(endAngle - beignAngle) ;

            if (Mathf.Abs(slipDegree) > minTurnAngle)
            {
                endAngle = (beignAngle + (slipDegree / Mathf.Abs(slipDegree)) * nextTurnAngle)%360;
 //               Debug.Log("endAngle:" + endAngle + ",beignAngle:" + beignAngle + ",slipDegree:" + slipDegree);

                transform.eulerAngles = new Vector3(0, endAngle, 0);
                beignAngle = endAngle;

                selectIndex = (slipDegree> 0) ?(selectIndex + 1)% spaceshipCount : (selectIndex+ spaceshipCount-1)% spaceshipCount;
 //               Debug.Log("select_avatar::selectIndex:" + selectIndex);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, beignAngle, 0);
            }
        }

        OnGetAvatarList();
    }

    protected override void SavePalyerData()
    {
        PlayerData.Instance.SelAvatarDBID = selAvatarDBID;
        PlayerData.Instance.SelectIndex = selectIndex;
        PlayerData.Instance.AvatarList = ui_avatarList;
    }

    public void reqAvatarList()
    {
        KBEngine.Event.fireIn("reqAvatarList", new object[] {});
    }

    public void onReqAvatarList(Dictionary<UInt64, AVATAR_INFOS> avatarList)
    {
        ui_avatarList = avatarList;

        foreach(var item in ui_avatarList)
        {
            AVATAR_INFOS info = item.Value;
            Debug.Log("select_avatar::onReqAvatarList,"+ "key:"+ item.Key+" : name" + info.name + ",dbid=" + info.dbid + ",value:"+info.data);
        }

       
    }

    public void onCreateAvatarResult(UInt64 retcode, AVATAR_INFOS info, Dictionary<UInt64, AVATAR_INFOS> avatarList)
    {
        if (retcode != 0)
        {
            err("Error creating avatar, errcode=" + retcode);
            return;
        }

 //       onReqAvatarList(avatarList);
    }

    public void onRemoveAvatar(UInt64 dbid, Dictionary<UInt64, AVATAR_INFOS> avatarList)
    {
        if (dbid == 0)
        {
            err("Delete the avatar error!(删除角色错误!)");
            return;
        }

        onReqAvatarList(avatarList);
    }
}
