using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using KBEngine;
using System;

public class ShipBase : MonoBehaviour
{

    public ShipAttri _attri = new ShipAttri();
    // Use this for initialization

    public  float MoveSpeed = 10.00f; //每秒的加速度
    public  float AccelerateSpeed = 10.5f;
    public  float RollSpeed = 50.5f;

    //因为设置的摩擦力系数是 1.5 最大速度不超过 7.0f
    public  float SpeedMax = 100.0f;
    public  float RollSpeedMax = 500.0f;

    protected Rigidbody _rb;

    //---------------摄像机开始位置-----------//
    public Vector3 camerOffset = Vector3.zero;

    //---------------飞行姿态----------------//
    protected Animator _animator;
    protected FlySate _state = FlySate.FLY_IDLE;


    //----------------喷火特效设置------------//

    protected float penghuo_lengthscale = 0.2f;
    protected float penghuo_acc_lengthscale = 1.0f;

    protected ParticleSystemRenderer penghuo_left_1, penghuo_left_2, penghuo_left_3;
    protected ParticleSystemRenderer penghuo_middle_1, penghuo_middle_2, penghuo_middle_3;
    protected ParticleSystemRenderer penghuo_right_1, penghuo_right_2, penghuo_right_3;

    //----------------生命值状态：正常，冒烟，爆炸------------
    public Material ship_po = null, ship_normal = null;
    public GameObject ship_Body = null, left_engine = null, right_engine = null;
    public GameObject ship_smoke = null, sub_smoke = null, ship_frame = null;
    public GameObject ship_explosion = null, sub_explosion = null;

    //----------------武器列表---------------------------
    public List<UInt32> weapon_list = new List<UInt32>();
    public enum FlySate
    {
        FLY_IDLE,      //空闲状态
        FLY_TURN_LEFT, //向左转
        FLY_LEFT_RECOVER,//左转恢复平衡位置
        FLY_TURN_RIGHT, //向右转
        FLY_RIGHT_RECOVER //右转恢复平衡
    }

    void InitRigidbody()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
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

    public virtual void InitVariable()
    {
        _attri.AttachShip(this);
    }

    public virtual void ChangeState(FlySate state)
    {

    }

    public virtual void SetPenghuoValue(bool accelerate)
    {

    }
    public virtual void SetPenghuoDir(bool dir)
    {

    }

    public virtual void ChangeShipEffect(ShipAttri.ShipState state)
    {

    }
    public virtual void PlaySparksEffect()
    {

    }
    public void SetShipSpeed(Vector3 speed)
    {
        if (speed == Vector3.zero || _rb == null)
        {
            return;
        }

        //       Debug.Log("sqrMagnitude:" + _rb.velocity.sqrMagnitude);
        if (_rb.velocity.sqrMagnitude >= SpeedMax * SpeedMax)
        {
            return;
        }

        _rb.velocity += speed * Time.deltaTime;
    }

    private void UpdateShipSpeed()
    {
        KBEngine.Event.fireIn("updateCruiseSpeed", (UINT16)(_rb.velocity.magnitude * 10));
    }

    public void SetShipRotation(Vector3 direction)
    {
        if (direction == Vector3.zero || _rb == null)
        {
            return;
        }

        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(direction * Time.deltaTime));
    }

    public void OnLanuchWeapon(UInt32 nID)
    {
        FireWeapon FireScript = GetComponent<FireWeapon>();
        if (FireScript != null && FireScript.isActiveAndEnabled)
        {
            FireScript.CastSpell(nID);
            //           Debug.Log(FireScript.gameObject);
        }
    }

    private void SavePalyerData()
    {

    }

    public bool isMyShip()
    {
        return KBEngineApp.app.entity_id == _attri.Id;
    }

    public virtual void ShipDestroy()
    {
        Debug.LogError("ship_id:" + _attri.Id);

        if (isMyShip())
        {
            PlayerData.Instance.ReliveCount += 1;

            //       if (PlayerData.singleton.ReliveCount >= 2)
            {
                SavePalyerData();
                GameObject world = GameObject.Find("World");
                if (world != null) // 把主摄像机移动回来，避免画面报错
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

    public virtual void SetHpProgressBar()
    {
        GameObject hp_bar = isMyShip() ? Trans.FindObj(Camera.main.gameObject, "hp_cd") : Trans.FindObj(gameObject, "enemy_hp_bar");

        if (hp_bar == null)
        {
            Debug.LogError("hp_bar not found!!");
            return;
        }

        if (_attri.HP_MAX >= _attri.Hp && _attri.HP_MAX > 0)
        {
            //           Debug.Log(name + "::Hp :" + Hp + ",HP_MAX:" + HP_MAX);

            float hp_percent = _attri.Hp / (_attri.HP_MAX * 1.0f);
            HpProgressBar hp_script = hp_bar.GetComponent<HpProgressBar>();
            hp_script.SetProgressBar(hp_percent);
        }
    }

    public virtual void SetExpProgressBar()
    {
        if (!isMyShip())
        {
            return;
        }

        GameObject exp_bar = Trans.FindObj(Camera.main.gameObject, "expIcon");
        if (exp_bar == null)
        {
            Debug.LogError("exp_bar not found!!");
            return;
        }

        if (_attri.EXP_Max >= _attri.Exp && _attri.EXP_Max > 0)
        {
            //           Debug.Log(name + "::Hp :" + Hp + ",HP_MAX:" + HP_MAX);

            float exp_percent = _attri.Exp / (_attri.EXP_Max * 1.0f) * ExpBar.expbar_count;

            ExpBar exp_script = exp_bar.GetComponent<ExpBar>();

            exp_script.SetExpProcessBar((int)exp_percent);
        }
    }

    public virtual void InitPengHuo()
    {

    }

    public virtual IEnumerator LoadMaerialPo()
    {
        yield return null;
    }

    protected virtual void Awake()
    {
        InitVariable();

        InitRigidbody();
    }

    protected virtual void Start()
    {
        InitPengHuo();

        InitFlyController();

        StartCoroutine(LoadMaerialPo());
    }

    protected virtual void FixedUpdate()
    {
        if (_rb.velocity.sqrMagnitude > 0)
        {
            UpdateShipSpeed();
        }
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (gameObject != null && _attri.ShipController != null)
        {
            _attri.ShipController.Update();
        }
    }

}
