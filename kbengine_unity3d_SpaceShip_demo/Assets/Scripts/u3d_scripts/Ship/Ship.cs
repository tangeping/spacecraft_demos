using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using KBEngine;

public class Ship : ShipAttri
{
    //--------------- 移动------------------
    //     public const float MoveSpeed = 1.00f; //每秒的加速度
    //     public const float AccelerateSpeed = 0.5f;
    //     public const float RollSpeed = 0.5f;
    //     public const float velocityMax = 20.0f;

    public const float MoveSpeed = 10.00f; //每秒的加速度
    public const float AccelerateSpeed = 10.5f;
    public const float RollSpeed = 50.5f;

    //因为设置的摩擦力系数是 1.5 最大速度不超过 7.0f
    public const float SpeedMax = 100.0f;
    public const float RollSpeedMax = 500.0f;



    //     public const float MoveSpeed = 0.02f;
    //     public const float AccelerateSpeed = 0.02f;
    //     public const float RollSpeed = 0.04
    private Rigidbody _rb;


    //   [HideInInspector] Follow cameraFllow = null;

    //   [HideInInspector] public GameObject mainCamera = null;

    //----------------喷火特效设置------------//

    [HideInInspector] float penghuo_lengthscale = 0.2f;
    [HideInInspector] float penghuo_acc_lengthscale = 1.0f;

    [HideInInspector] ParticleSystemRenderer penghuo_left_1, penghuo_left_2, penghuo_left_3;
    [HideInInspector] ParticleSystemRenderer penghuo_middle_1, penghuo_middle_2, penghuo_middle_3;
    [HideInInspector] ParticleSystemRenderer penghuo_right_1, penghuo_right_2, penghuo_right_3;

    //---------------武器-------------------//
 //   public delegate void LanuchWeaponHander(int nID);
 //   public static event LanuchWeaponHander WeaponEvent;


    //---------------飞行姿态----------------//
    [HideInInspector] Animator _animator;
    [HideInInspector] FlySate _state = FlySate.FLY_IDLE;


    public enum FlySate
    {
        FLY_IDLE,      //空闲状态
        FLY_TURN_LEFT, //向左转
        FLY_LEFT_RECOVER,//左转恢复平衡位置
        FLY_TURN_RIGHT, //向右转
        FLY_RIGHT_RECOVER //右转恢复平衡
    }

    void InitPengHuo()
    {
        penghuo_left_1 = Trans.FindObj( gameObject,"penhuo_left_1").GetComponent<ParticleSystemRenderer>();
        penghuo_left_2 = Trans.FindObj( gameObject,"penhuo_left_2").GetComponent<ParticleSystemRenderer>();
        penghuo_left_3 = Trans.FindObj( gameObject,"penhuo_left_3").GetComponent<ParticleSystemRenderer>();

        penghuo_middle_1 = Trans.FindObj( gameObject,"penhuo_middle_1").GetComponent<ParticleSystemRenderer>();
        penghuo_middle_2 = Trans.FindObj( gameObject,"penhuo_middle_2").GetComponent<ParticleSystemRenderer>();
        penghuo_middle_3 = Trans.FindObj( gameObject,"penhuo_middle_3").GetComponent<ParticleSystemRenderer>();

        penghuo_right_1 = Trans.FindObj( gameObject,"penhuo_right_1").GetComponent<ParticleSystemRenderer>();
        penghuo_right_2 = Trans.FindObj( gameObject,"penhuo_right_2").GetComponent<ParticleSystemRenderer>();
        penghuo_right_3 = Trans.FindObj( gameObject,"penhuo_right_3").GetComponent<ParticleSystemRenderer>();

        if (penghuo_left_1 == null || penghuo_left_2 == null || penghuo_left_2 == null ||
            penghuo_middle_1 == null || penghuo_middle_2 == null || penghuo_middle_3 == null ||
            penghuo_right_1 == null || penghuo_right_2 == null || penghuo_right_3 == null)
        {
            Debug.LogError("InitPengHuo null");
        }
    }

    void InitRigidbody()
    {
        _rb = GetComponent<Rigidbody>();
        if(_rb == null)
        {
            Debug.LogError("Rigidbody have not found.");
        }
    }

    void InitFlyController()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("not found fly_animator!!!");
        }
    }

    IEnumerator LoadMaerialPo()
    {
        yield return new WaitForFixedUpdate();
//------------------------材质球--------------------
        ship_normal = (Material)Resources.Load("character/spacecraft02/material/Copperhead_Desert");
        if (ship_normal == null)
        {
            Debug.LogError("not found ship_normal!!!");
        }

        ship_po = (Material)Resources.Load("character/spacecraft02/material/Copperhead_Desert-po");
        if(ship_po == null)
        {
            Debug.LogError("not found ship_po!!!");
        }
//-----------------------预制体-----------------------        
        ship_smoke = (GameObject)Resources.Load("character/spacecraft02/effect/prefab/spacecraft02_maoyan");
        if (ship_smoke == null)
        {
            Debug.LogError("not found ship_smoke!!!");
        }

        ship_explosion = (GameObject)Resources.Load("character/spacecraft02/effect/prefab/spacecraft02_baozha");
        if (ship_explosion == null)
        {
            Debug.LogError("not found ship_explosion!!!");
        }
//------------------挂载节点---------------------------
        ship_Body = Trans.FindObj( gameObject,"pasted__Body");
        if (ship_Body == null)
        {
            Debug.LogError("not found ship_Body!!!");
        }

        left_engine = Trans.FindObj( gameObject,"pasted__EngineLeft");
        if (left_engine == null)
        {
            Debug.LogError("not found left_engine!!!");
        }

        right_engine = Trans.FindObj( gameObject,"pasted__EngineRight");
        if (right_engine == null)
        {
            Debug.LogError("not found right_engine!!!");
        }

        ship_frame = Trans.FindObj( gameObject,"spacecraft02_zhuti");
        if (ship_frame == null)
        {
            Debug.LogError("not found ship_frame!!!");
        }

    }

    public void ChangeState(FlySate state)
    {
        if (state != _state)
        {
            _state = state;
            switch (_state)
            {
                case FlySate.FLY_IDLE:
                    _animator.SetBool("isIdle", true);
                    _animator.SetBool("isTurnLeft", false);
                    _animator.SetBool("isTurnRight", false);
                    break;
                case FlySate.FLY_TURN_LEFT:
                    _animator.SetBool("isIdle", false);
                    _animator.SetBool("isTurnLeft", true);
                    _animator.SetBool("isTurnRight", false);

                    break;
                case FlySate.FLY_TURN_RIGHT:
                    _animator.SetBool("isIdle", false);
                    _animator.SetBool("isTurnLeft", false);
                    _animator.SetBool("isTurnRight", true);

                    break;
                default:
                    break;
            }
        }
    }

    public void SetPenghuoValue(bool accelerate)
    {
        if(penghuo_left_1 == null || penghuo_middle_1 == null || penghuo_right_1==null)
        {
            return;
        }
        
        float symbol = penghuo_left_1.lengthScale > 0.0f ? 1.0f : -1.0f;
        float value = symbol * Mathf.Abs(accelerate ? penghuo_acc_lengthscale : penghuo_lengthscale);
        if (Mathf.Abs(Mathf.Abs(value) - Mathf.Abs(penghuo_left_1.lengthScale)) < 0.01)
        {
            return;
        }

        penghuo_left_1.lengthScale = value;
        penghuo_left_2.lengthScale = value;
        penghuo_left_3.lengthScale = value;

        penghuo_middle_1.lengthScale = value;
        penghuo_middle_2.lengthScale = value;
        penghuo_middle_3.lengthScale = value;

        penghuo_right_1.lengthScale = value;
        penghuo_right_2.lengthScale = value;
        penghuo_right_3.lengthScale = value;
    }
    public void SetPenghuoDir(bool dir)
    {
        if (gameObject == null)
        {
            return;
        }

        float symbol = dir ? -1.0f : 1.0f;
        if (penghuo_left_1.lengthScale * symbol > 0)
        {
            return;
        }

        penghuo_left_1.lengthScale = symbol * Mathf.Abs(penghuo_left_1.lengthScale);
        penghuo_left_2.lengthScale = symbol * Mathf.Abs(penghuo_left_2.lengthScale);
        penghuo_left_3.lengthScale = symbol * Mathf.Abs(penghuo_left_3.lengthScale);

        penghuo_middle_1.lengthScale = symbol * Mathf.Abs(penghuo_middle_1.lengthScale);
        penghuo_middle_2.lengthScale = symbol * Mathf.Abs(penghuo_middle_2.lengthScale);
        penghuo_middle_3.lengthScale = symbol * Mathf.Abs(penghuo_middle_3.lengthScale);

        penghuo_right_1.lengthScale = symbol * Mathf.Abs(penghuo_right_1.lengthScale);
        penghuo_right_2.lengthScale = symbol * Mathf.Abs(penghuo_right_2.lengthScale);
        penghuo_right_3.lengthScale = symbol * Mathf.Abs(penghuo_right_3.lengthScale);
    }

    public void SetShipSpeed(Vector3 speed)
    {
        if(speed == Vector3.zero || _rb == null)
        {
            return;
        }

 //       Debug.Log("sqrMagnitude:" + _rb.velocity.sqrMagnitude);
        if (_rb.velocity.sqrMagnitude >= SpeedMax * SpeedMax)
        {
            return;
        }
        
        _rb.velocity += speed*Time.deltaTime;     
    }
    
    private void UpdateShipSpeed()
    {
        KBEngine.Event.fireIn("updateCruiseSpeed", (UINT16)(_rb.velocity.magnitude*10));
    }

    public void SetShipRotation(Vector3 direction)
    {
        if(direction == Vector3.zero || _rb == null)
        {
            return;
        }
        
        _rb.MoveRotation(_rb.rotation *Quaternion.Euler(direction*Time.deltaTime));
    }

    public void OnLanuchWeapon(int nID)
    {
        FireWeapon FireScript = GetComponent<FireWeapon>();
        if (FireScript != null && FireScript.isActiveAndEnabled)
        {
            FireScript.CastSpell(nID);
 //           Debug.Log(FireScript.gameObject);
        }       
    }

    private  void SavePalyerData()
    {

    }

    public bool isMyShip()
    {
        return KBEngineApp.app.entity_id == Id;
    }

    public override void ShipDestroy()
    {
        Debug.LogError("ship_id:" + Id);

        if (isMyShip())
        {
            PlayerData.singleton.ReliveCount += 1;

            //       if (PlayerData.singleton.ReliveCount >= 2)
            {
                SavePalyerData();
                GameObject world = GameObject.Find("World");
                if(world != null) // 把主摄像机移动回来，避免画面报错
                {
                    Camera.main.transform.parent = world.transform;
                }
                SceneManager.LoadScene("failscene");
            }           
        }
        else
        {
 //           Destroy(gameObject);
        }
        
    }

    protected override void SetHpProgressBar()
    {
        GameObject hp_bar = isMyShip() ? Trans.FindObj(Camera.main.gameObject, "hp_cd"): Trans.FindObj(gameObject, "enemy_hp_bar");

        if (hp_bar == null)
        {
            Debug.LogError("hp_bar not found!!");
            return;
        }

        if (HP_MAX >= Hp && HP_MAX > 0)
        {
 //           Debug.Log(name + "::Hp :" + Hp + ",HP_MAX:" + HP_MAX);

            float hp_percent = Hp / (HP_MAX * 1.0f);
            HpProgressBar hp_script = hp_bar.GetComponent<HpProgressBar>();
            hp_script.SetProgressBar(hp_percent);
        }
    }

    protected override void SetExpProgressBar()
    {
        if(!isMyShip())
        {
            return;
        }

        GameObject exp_bar = Trans.FindObj(Camera.main.gameObject, "expIcon");
        if (exp_bar == null)
        {
            Debug.LogError("exp_bar not found!!");
            return;
        }

        if (EXP_Max >= Exp && EXP_Max > 0)
        {
            //           Debug.Log(name + "::Hp :" + Hp + ",HP_MAX:" + HP_MAX);

            float exp_percent = Exp / (EXP_Max * 1.0f) * ExpBar.expbar_count;

            ExpBar exp_script = exp_bar.GetComponent<ExpBar>();

            exp_script.SetExpProcessBar((int)exp_percent);
        }
    }

    private void Awake()
    {
        InitRigidbody();

        InitPengHuo();

        InitFlyController();

        LoadMaerialPo();
    }

    // Use this for initialization
    void Start()
    {

        //         TakeController(new keyController());
        //         ShipController.TargetAttached(this);
        //         ShipController.Start();
        StartCoroutine(LoadMaerialPo());
    }
    private void FixedUpdate()
    {
        if(_rb.velocity.sqrMagnitude > 0)
        {
            UpdateShipSpeed();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject != null&&ShipController != null)
        {
            ShipController.Update();
        }

 //       Debug.Log("velocity:" + _rb.velocity);
    }
}
