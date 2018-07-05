using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : connectstate {

    //   private UnityEngine.GameObject terrain = null;
    public UnityEngine.GameObject player = null;
    // Use this for initialization
 //   [HideInInspector] List<PropAttri> props = new List<PropAttri>();

    private GameObject ship1Perfab, ship2Perfab, ship3Perfab; // 飞船

 //   private GameObject bulletPerfab, missilePerfab, weapon_parent; //武器
                                                    //   public GameObject surportBoxPerfab, minePerfab;
    private GameObject minePerfab, supplyBoxPerfab; //道具


    public override void installEvents()
    {
        base.installEvents();

        // in world
        KBEngine.Event.registerOut("addSpaceGeometryMapping", this, "addSpaceGeometryMapping");
        KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
        KBEngine.Event.registerOut("set_position", this, "set_position");
        KBEngine.Event.registerOut("set_direction", this, "set_direction");
        KBEngine.Event.registerOut("updatePosition", this, "updatePosition");
        KBEngine.Event.registerOut("onControlled", this, "onControlled");
        KBEngine.Event.registerOut("onSetSpaceData", this, "onSetSpaceData");//设置空间的参数
        KBEngine.Event.registerOut("onEnterSpace", this, "onEnterSpace");

        // in world(register by scripts)
        KBEngine.Event.registerOut("onAvatarEnterWorld", this, "onAvatarEnterWorld");
        KBEngine.Event.registerOut("set_level", this, "set_level");
        KBEngine.Event.registerOut("set_name", this, "set_entityName");
        KBEngine.Event.registerOut("set_modelScale", this, "set_modelScale");
        KBEngine.Event.registerOut("set_modelID", this, "set_modelID");
        KBEngine.Event.registerOut("otherAvatarOnJump", this, "otherAvatarOnJump"); 
        KBEngine.Event.registerOut("set_moveSpeed", this, "set_moveSpeed");
        KBEngine.Event.registerOut("set_cruiseSpeed", this, "set_cruiseSpeed");
        KBEngine.Event.registerOut("set_HP", this, "set_HP");
        KBEngine.Event.registerOut("set_HP_Max", this, "set_HP_Max");
        KBEngine.Event.registerOut("set_MP", this, "set_MP");
        KBEngine.Event.registerOut("set_EXP", this, "set_EXP");
        KBEngine.Event.registerOut("set_EXP_Max", this, "set_EXP_Max");
        KBEngine.Event.registerOut("set_MP_Max", this, "set_MP_Max");
        KBEngine.Event.registerOut("set_spaceUType", this, "set_spaceUType");
        KBEngine.Event.registerOut("set_uid", this, "set_uid");
        KBEngine.Event.registerOut("set_utype", this, "set_utype");
        KBEngine.Event.registerOut("onReqWeaponDestroyTime", this, "onReqWeaponDestroyTime"); 
        KBEngine.Event.registerOut("recvDamage", this, "recvDamage");
        KBEngine.Event.registerOut("recvPropEffect", this, "recvPropEffect");
        KBEngine.Event.registerOut("set_state", this, "set_state");

        //in weapon
        KBEngine.Event.registerOut("onWeaponDestroy", this, "onWeaponDestroy");
        KBEngine.Event.registerOut("canUseWeaponResult", this, "canUseWeaponResult");

        //in props
        KBEngine.Event.registerOut("onMineDestroy", this, "onMineDestroy");
        KBEngine.Event.registerOut("onSupplyBoxDestroy", this, "onSupplyBoxDestroy");
    }

    protected override void OnDestroy()
    {
        Debug.Log(gameObject.name + " OnDestroy!!");
        base.OnDestroy();
        KBEngine.Event.deregisterOut(this);
    }


    private void Start()
    {  
        Debug.Log("World::SelAvatarDBID:" + PlayerData.Instance.SelAvatarDBID);
        KBEngine.Event.fireIn("selectAvatarGame", PlayerData.Instance.SelAvatarDBID);

        StartCoroutine(LoadPerfab());
    }

    IEnumerator LoadPerfab() //预制体有点大,直接挂在很慢
    {
        yield return new WaitForFixedUpdate();

        ship1Perfab = (GameObject)Resources.Load("character/spacecraft02/prefab/spacecraft02_ef");
        if (ship1Perfab == null)
        {
            Debug.LogError("not found ship1Perfab!!!");
        }

        ship2Perfab = (GameObject)Resources.Load("character/spacecraft04/prefab/battleshipG4_ef");
        if (ship2Perfab == null)
        {
            Debug.LogError("not found ship2Perfab!!!");
        }

        ship3Perfab = (GameObject)Resources.Load("character/spacecraft01/prefab/spacecraft01_ef");
        if (ship3Perfab == null) 
        {
            Debug.LogError("not found ship3Perfab!!!");
        }

        minePerfab = (GameObject)Resources.Load("character/daoju/prefab/spacecraft_zhadan02"); 
        if (minePerfab == null)
        {
            Debug.LogError("not found minePerfab!!!");
        }

        supplyBoxPerfab = (GameObject)Resources.Load("character/daoju/prefab/spacecraft_daoju01");
        if (supplyBoxPerfab == null)
        {
            Debug.LogError("not found supplyBoxPerfab!!!");
        }
    }

    private void Update()
    {
        
    }

    private void OnGUI()
    {

        if (gameObject==null ||KBEngineApp.app.entity_type != "Avatar")
        {
            return;
        }

        KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
        if (avatar != null)
        {
            GUI.color = Color.green;
            GUI.Label(new Rect((Screen.width / 2) - 100, 20, 400, 100), "id=" + avatar.id + ", position=" + avatar.position.ToString());
        }

        
    }

    public void onSetSpaceData(UInt32 spaceID, string key, string value)
    {

    }

    public void onEnterSpace(KBEngine.Entity entity)
    {

    }

    public void addSpaceGeometryMapping(string respath)
    {
        Debug.Log("addSpaceGeometryMapping:" + respath);
        if (player)
            player.GetComponent<GameEntity>().entityEnable();
    }

    public GameObject ChooseShip(KBEngine.Entity entity)
    {
        if(entity == null || entity.className != "Avatar")
        {
            return null;
        }

        KBEngine.Avatar avatar = (KBEngine.Avatar)entity;
        Debug.Log(name+",world::ChooseShip,avatar.level:" + avatar.level);

        if (avatar.level == 1)
        {
            return ship1Perfab;
        }
        else if (avatar.level == 2)
        {
            return ship2Perfab;
        }
        else if (avatar.level == 3)
        {
            return ship3Perfab;
        }

        return null;
    }

    public void onEnterWorld(KBEngine.Entity entity)
    {

        if (gameObject == null || entity.isPlayer())
        {
            createPlayer();          
                return;
        }

        GameObject perfab = null;
        Debug.Log("onEnterWorld." + entity.id +",position:"+entity.position+",direction:"+entity.direction);

        if (entity.className == "Avatar")
        {
            perfab = ChooseShip(entity);
        }
        else if(entity.className == "Weapon")
        {
            KBEngine.Weapon arsenal = (KBEngine.Weapon)entity;
            CONF.conf_bullet bullet_conf = (CONF.conf_bullet)CSVHelper.Instance.GetItem("conf_bullet", arsenal.uid);
            if(bullet_conf == null)
            {
                Debug.LogError("world::bullet_conf not found  arsenal.uid:" + arsenal.uid);
                return;
            }
            perfab = Resources.Load<GameObject>(bullet_conf.Perfab);
        }
        else if(entity.className == "Mine")
        {
            perfab = minePerfab;
        }
        else if(entity.className ==  "SupplyBox")
        {
            perfab = supplyBoxPerfab;
        }
        
        if (perfab == null)
        {
            Debug.LogError("entity." + entity.id + ",className:" + entity.className + ",SelectIndex:"+ PlayerData.Instance.SelectIndex);
            return;
        }
     
        entity.renderObj = Instantiate(perfab, entity.position, Quaternion.Euler(entity.direction), transform) as UnityEngine.GameObject;
        ((GameObject)entity.renderObj).name = entity.className + "_" + entity.id;

        Debug.Log("onEnterWorld." + entity.id  + ",rederObj:" + (UnityEngine.GameObject)entity.renderObj +
            ",position:" + ((UnityEngine.GameObject)entity.renderObj).transform.position + 
            ",eulerAngles:" + ((UnityEngine.GameObject)entity.renderObj).transform.rotation.eulerAngles);

        if (entity.className == "Avatar")
        {
            ((GameObject)entity.renderObj).GetComponent<ShipBase>()._attri.Id = entity.id;
        }
//       ((UnityEngine.GameObject)entity.renderObj).transform.parent = transform;
    }

    public void onLeaveWorld(KBEngine.Entity entity)
    {
        if (gameObject == null || entity.renderObj == null)
            return;
        // 
        //         if(entity.className == "Avatar")
        //         {
        //             Camera.main.transform.parent = transform;
        //         }

        UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
        entity.renderObj = null;
    }
    // Update is called once per frame 
    public void set_moveSpeed(KBEngine.Entity entity, UInt16 v)
    {
        if (gameObject == null || entity.renderObj == null)
        {
            return;
        }

        float fspeed = ((float)(UInt16)v) / 10.0f;
        ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().Speed = fspeed;

    }

    public void set_cruiseSpeed(KBEngine.Entity entity, UInt16 v)
    {
        if (gameObject == null || entity.renderObj == null)
        {
            return;
        }

        float fspeed = ((float)(UInt16)v) / 10.0f;
        ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().CruiseSpeed = fspeed;

    }

    public void set_position(KBEngine.Entity entity)
    {
        if (gameObject == null || entity.renderObj == null)
            return;
        Debug.Log("World::set_position."+ entity.id+",position:" + entity.position);
        GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
        gameEntity.destPosition = entity.position;
        gameEntity.Position = entity.position;
        gameEntity.SpaceID = KBEngineApp.app.spaceID;
    }

    public void set_direction(KBEngine.Entity entity)
    {
        if (gameObject == null || entity.renderObj == null)
            return;
        Debug.Log("World::set_direction." + entity.id + ",direction:" + entity.direction);
        GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
        gameEntity.destDirection = new Vector3(entity.direction.x, entity.direction.y, entity.direction.z);
        gameEntity.SpaceID = KBEngineApp.app.spaceID;
    }

    public void updatePosition(KBEngine.Entity entity)
    {
        if (gameObject == null || entity == null || entity.renderObj == null)
            return;

//        Debug.Log("World::updatePosition." + entity.id + ",destPosition:" + entity.position);
        GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
        gameEntity.destPosition = entity.position;
        gameEntity.destDirection = entity.direction;
        gameEntity.isOnGround = entity.isOnGround;
        gameEntity.SpaceID = KBEngineApp.app.spaceID;
    }
    public void onControlled(KBEngine.Entity entity, bool isControlled)
    {
        if (entity.renderObj == null || gameObject == null)
            return;
        Debug.Log("World::onControlled." + entity.id + ",isControlled:" + isControlled);
        GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
        gameEntity.isControlled = isControlled;
    }

    public void onAvatarEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.Avatar avatar)
    {
        if (!avatar.isPlayer())
        {
            return;
        }
        
        Debug.Log("loading scene...");
    }
    public void otherAvatarOnJump(KBEngine.Entity entity)
    {
        Debug.Log("otherAvatarOnJump: " + entity.id);
        if (gameObject == null || entity.renderObj != null)
        {
            ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().OnJump();
        }
    }

    public void set_level(KBEngine.Entity entity, UInt16 v)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_level: " + entity.id + ",level:"+ v);

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();

            if(shipScript._attri.Level != v)
            {
                if (shipScript._attri.Level > 0 && v > shipScript._attri.Level)
                {
                    onShipUpgrade(entity);
                }

                shipScript._attri.Level = v;
            }          
        }
    }

    public void set_entityName(KBEngine.Entity entity, string v)
    {
        if (gameObject == null || entity.renderObj == null )
            return;

        Debug.Log("World::set_entityName: " + entity.id + ",v:" + v);
        ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().entity_name = v;
        ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().entity_type = entity.className;

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.Id = entity.id;
        }

    }

    public void set_modelScale(KBEngine.Entity entity, Byte v)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_modelScale: " + entity.id + ",v:" + v);
    }

    public void set_modelID(KBEngine.Entity entity, UInt32 v)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_modelID: " + entity.id + ",v:" + v);

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.ShipID = (int)v;
        }

    }

    public void set_HP(KBEngine.Entity entity, Int32 v, Int32 HP_Max)
    {
        if (gameObject == null || entity.renderObj == null)
            return;
        Debug.Log("World::set_HP: " + entity.id + ",v:" + v);

        ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = "" + v + "/" + HP_Max;

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.Hp = v;
        }
    }

    public void set_HP_Max(KBEngine.Entity entity, Int32 v, Int32 HP)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_HP_Max: " + entity.id + ",v:" + v);

        ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = "" + HP + "/" + v;

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.HP_MAX = v;
        }
    }
    

    public void set_EXP(KBEngine.Entity entity, Int32 v)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_EXP: " + entity.id + ",v:" + v);

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.Exp = v;
        }
    }

    public void set_EXP_Max(KBEngine.Entity entity, Int32 v)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_EXP_Max: " + entity.id + ",v:" + v);

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.EXP_Max = v;
        }
    }

    public void set_MP(KBEngine.Entity entity, Int32 v, Int32 MP_Max)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_MP: " + entity.id + ",v:" + v);

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.Mp = v;
        }
    }
    public void set_MP_Max(KBEngine.Entity entity, Int32 v, Int32 MP)
    {
        if (gameObject == null || entity.renderObj == null)
            return;

        Debug.Log("World::set_MP_Max: " + entity.id + ",v:" + v);

        if (entity.className == "Avatar")
        {
            ShipBase shipScript = ((UnityEngine.GameObject)entity.renderObj).GetComponent<ShipBase>();
            shipScript._attri.MP_MAX = v;
        }
    }
    public void set_spaceUType(KBEngine.Entity entity, UInt32 v)
    {
        Debug.Log("World::set_spaceUType: " + entity.id + ",v:" + v);
    }
    public void set_uid(KBEngine.Entity entity, UInt32 v)
    {
        Debug.Log("World::set_uid: " + entity.id + ",v:" + v);
    }
    public void set_utype(KBEngine.Entity entity, UInt32 v)
    {
        Debug.Log("World::set_utype: " + entity.id + ",v:" + v);
    }
    public void set_state(KBEngine.Entity entity, SByte v)
    {
        Debug.Log("World::set_state: " + entity.id + ",v:" + v);
    }

    public void onReqWeaponDestroyTime(KBEngine.Entity entity, UInt32 v)
    {
        Debug.Log("World::onReqWeaponDestroyTime: " + entity.id + ",v:" + v);
    }
    public void recvDamage(KBEngine.Entity entity, Int32 attackerID, Int32 weaponID, Int32 damageType, Int32 damage)
    {
        Debug.Log("World::recvDamage: " + entity.id + ",attackerID:" + attackerID + ",weaponID:" + weaponID
            + ",damageType:" + damageType + ",damage:" + damage);

        if (gameObject == null || entity.renderObj == null || entity.className != "Avatar" || entity.isPlayer())
            return;

        GameObject hp_bar = Trans.FindObj(((GameObject)entity.renderObj).gameObject, "enemy_hp_bar");
        if(hp_bar != null)
        {
            hp_bar.SetActive(true);
        }
    }
    
    public void recvPropEffect(KBEngine.Entity entity, UInt32 propID, Int32 propType)
    {
        Debug.Log("World::recvPropEffect: " + entity.id + ",propID:" + propID + ",propType:" + propType);
    }

    public void addEffect(Vector3 position, Vector3 EulerAngle, string perfabName, UInt16 explodeTime)
    {
        GameObject explode_perfab = Resources.Load<GameObject>(perfabName);

        if (explode_perfab == null)
        {
            Debug.LogError(perfabName + " not found explode");
            return;
        }

        GameObject explode_clone = GameObject.Instantiate(explode_perfab, position,
           Quaternion.Euler(EulerAngle), player.transform);

        explode sript_explode = explode_clone.AddComponent<explode>();

        sript_explode.Duration = explodeTime / 1.0f;
    }

    public void onWeaponDestroy(KBEngine.Entity entity, UInt16 explodeTime)//销毁武器,添加爆炸特效
    {
        if (gameObject == null || entity.renderObj == null || entity.className != "Weapon")
            return;

        UInt32 weaponModeID = ((KBEngine.Weapon)entity).uid;

        if (weaponModeID == 40001)//如果是激光等特效放完
        {
            explode sript_explode = ((UnityEngine.GameObject)entity.renderObj).AddComponent<explode>();
            sript_explode.Duration = explodeTime / 1.0f;
            entity.renderObj = null;
        }
        else
        {
            UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
            entity.renderObj = null;

            CONF.conf_bullet bullet_conf = (CONF.conf_bullet)CSVHelper.Instance.GetItem("conf_bullet", weaponModeID);

            addEffect(entity.position, entity.direction, bullet_conf.Explode, explodeTime);
        }
        Debug.Log("World::onReqWeaponDestroyTime: " + entity.id +",className:"+entity.className);
    }
    


    public void onMineDestroy(KBEngine.Entity entity,Int32 CollisionID, UInt16 explodeTime)
    {
        if (gameObject == null || entity.renderObj == null || entity.className != "Mine")
            return;

        UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
        entity.renderObj = null;

        KBEngine.Avatar collisioner = (KBEngine.Avatar)KBEngineApp.app.getEntity(CollisionID);
        if (collisioner == null)
        {
            Debug.Log("collisioner not in your world!");
            return;
        }
        string perfabName = "";
        
        if(collisioner.level == 1)
        {
            perfabName = "character/daoju/effect/prefab/spacecraft_zhadan02baozha";
        }
        else if(collisioner.level == 2)
        {
            perfabName = "character/daoju/effect/prefab/spacecraft_zhadan02baozha";
        }
        else if(collisioner.level == 3)
        {
            perfabName = "character/daoju/effect/prefab/spacecraft_zhadan02baozha";
        }

        addEffect(collisioner.position, collisioner.direction, perfabName, explodeTime);

        Debug.Log("World::onMineDestroy: " + entity.id + ",className:" + entity.className);
    }

    public void onSupplyBoxDestroy(KBEngine.Entity entity, Int32 CollisionID, UInt16 explodeTime)
    {
        if (gameObject == null || entity.renderObj == null || entity.className != "SupplyBox")
            return;

        UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
        entity.renderObj = null;

        KBEngine.Avatar collisioner = (KBEngine.Avatar)KBEngineApp.app.getEntity(CollisionID);
        if (collisioner == null)
        {
            Debug.Log("collisioner not in your world!");
            return;
        }

        string perfabName="";

        if (collisioner.level == 1)
        {
            perfabName = "character/spacecraft02/effect/prefab/spacecraft02_chidaoju";
        }
        else if (collisioner.level == 2)
        {
            perfabName = "character/spacecraft04/effect/prefab/spacecraft04_chidaoju";
        }
        else if (collisioner.level == 3)
        {          
            perfabName = "character/spacecraft01/effect/prefab/spacecraft01_chidaoju";
        }

        addEffect(collisioner.position, collisioner.direction, perfabName, explodeTime);

        Debug.Log("World::onSupplyBoxDestroy: " + entity.id + ",className:" + entity.className);
    }


    public void canUseWeaponResult(KBEngine.Entity entity, UInt16 cooltime, Byte haveWeapon)
    {
        Debug.Log(entity.className + "::canUseWeaponResult " + entity.id + ",cooltime:"+ cooltime+ ",haveWeapon:"+ haveWeapon);

    }

    public void AddcameraFllow()
    {
        if (gameObject == null || player != null)
        {
            Camera mainCamera = Camera.main;

            Debug.Log("<---------mainCamera------------>:" + mainCamera);

            if (mainCamera == null)
            {
                Debug.LogError("mainCamera not found:" + mainCamera);
            }

            Follow cameraFllow = mainCamera.GetComponent<Follow>();
            if (cameraFllow == null)
            {
                Debug.LogError("cameraFllow not found:" + cameraFllow);
            }

            ShipBase myship = player.GetComponent<ShipBase>();
            if (myship == null)
            {
                Debug.LogError("myship not found:" + myship);
            }

            cameraFllow.AttachTarget(player.transform,myship.camerOffset);
        }
    }

    public void AddKeyControll()
    {
        if (gameObject == null || player != null)
        {
            ShipBase myship = player.GetComponent<ShipBase>();
            if (myship == null)
            {
                Debug.LogError("myship not found:" + myship);
            }

            myship._attri.TakeController(new keyController());
            myship._attri.ShipController.TargetAttached(myship);
            myship._attri.ShipController.Start();
        }
    }

    public void AddWeaponFire()
    {
        if (gameObject == null || player == null)
        {
            return;
        }
        Debug.Log("-------AddWeaponFire --------:");

        player.AddComponent<FireWeapon>();
    }

    public void InitShipAttri(KBEngine.Avatar avatar)
    {
        if (gameObject == null || player == null || avatar == null)
        {
            return;
        }

        ShipBase shipScript = player.GetComponent<ShipBase>();
        if(shipScript == null)
        {
            Debug.LogError("shipScript not found!!");
            return;
        }
        shipScript._attri.Id = avatar.id;

        GameEntity gameScript = player.GetComponent<GameEntity>();
        if(gameScript == null)
        {
            Debug.LogError("gameScript not found!!");
            return;
        }

        gameScript.isPlayer = true;
        gameScript.entityEnable();
        player.name = avatar.className + "_" + avatar.id;

        avatar.renderObj = player;
    }

    public void createPlayer()
    {
        if (gameObject == null || player != null)
        {
            return;
        }

        if (KBEngineApp.app.entity_type != "Avatar")
        {
            return;
        }

        KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
        if (avatar == null /*|| avatar.state != 1*/)
        {
            Debug.Log("wait create(palyer)!");
            return;
        }

        GameObject shipPerfab = ChooseShip(avatar);
        if(shipPerfab == null)
        {
            Debug.LogError(name + "shipPerfab is null,level:" + avatar.level);
        }

        Debug.Log("world::avatar,position:"+ avatar.position + ",direction:"+ avatar.direction);

        player = Instantiate(shipPerfab, avatar.position, Quaternion.Euler(avatar.direction), transform) as UnityEngine.GameObject;

        Debug.Log("world::player,position:" + player.transform.position + ",rotation:" + player.transform.rotation +"player:"+player);

        AddcameraFllow();

        AddKeyControll();

        AddWeaponFire();

        InitShipAttri(avatar);

        set_position(avatar);

        set_direction(avatar);
    }

    public void onShipUpgrade(KBEngine.Entity entity)
    {
        if(entity == null || entity.className != "Avatar")
        {
            return;
        }
        KBEngine.Avatar avatar = (KBEngine.Avatar)entity;

        if(avatar.level < 1 || avatar.level > 3)
        {
            Debug.LogError(avatar.id + "::error level:" + avatar.level);
            return;
        }
        Debug.Log("World::onShipUpgrade:level = "+ avatar.level);

        GameObject OldPerfab = (UnityEngine.GameObject)avatar.renderObj;

        ShipBase OldScript = OldPerfab.GetComponent<ShipBase>();

        GameObject NewPerfab = ChooseShip(avatar);

        if(NewPerfab == null)
        {
            return;
        }

        GameObject NewShip =  Instantiate(NewPerfab, avatar.direction, Quaternion.Euler(avatar.direction),transform.parent);

        ShipBase NewScript = NewShip.GetComponent<ShipBase>();
        NewScript._attri = OldScript._attri;

        if(entity.isPlayer())
        {
            player = NewShip;

            AddcameraFllow();

            AddKeyControll();

            AddWeaponFire();

            InitShipAttri(avatar);
        }
        else
        {
            avatar.name = avatar.className + "_" + avatar.id;          
            avatar.renderObj = NewShip;
        }
        
        Destroy(OldPerfab);

        string effectName;
        if (avatar.level == 2)
        {
            effectName = "character/spacecraft04/effect/prefab/spacecraft04_up";
        }
        else if (avatar.level == 3)
        {
            effectName = "character/spacecraft01/effect/prefab/spacecraft01_up";
        }
        else
        {
            return;
        }
        addEffect(NewShip.transform.position,NewShip.transform.eulerAngles, effectName, 2);
    }
}
