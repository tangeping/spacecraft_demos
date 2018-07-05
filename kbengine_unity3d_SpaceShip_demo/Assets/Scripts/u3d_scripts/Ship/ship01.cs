using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship01 : ShipBase {

    // Use this for initialization

    //----------------枪口火花设置------------//
    GameObject sparks_left_1, sparks_left_2, sparks_right_1, sparks_right_2;

    
    public override void InitVariable()
    {
        base.InitVariable();

        MoveSpeed = 10.00f; //每秒的加速度
        AccelerateSpeed = 10.5f;
        RollSpeed = 50.5f;

         //因为设置的摩擦力系数是 1.5 最大速度不超过 7.0f
        SpeedMax = 100.0f;
        RollSpeedMax = 500.0f;

        camerOffset = new Vector3(0.0f, 2.3f, -5.0f);

        weapon_list.Add(10001);
        weapon_list.Add(20001);
    }

    public override void InitPengHuo()
    {
        penghuo_left_1 = Trans.FindObj(gameObject, "penhuo_left_1").GetComponent<ParticleSystemRenderer>();
        penghuo_left_2 = Trans.FindObj(gameObject, "penhuo_left_2").GetComponent<ParticleSystemRenderer>();
        penghuo_left_3 = Trans.FindObj(gameObject, "penhuo_left_3").GetComponent<ParticleSystemRenderer>();

        penghuo_middle_1 = Trans.FindObj(gameObject, "penhuo_middle_1").GetComponent<ParticleSystemRenderer>();
        penghuo_middle_2 = Trans.FindObj(gameObject, "penhuo_middle_2").GetComponent<ParticleSystemRenderer>();
        penghuo_middle_3 = Trans.FindObj(gameObject, "penhuo_middle_3").GetComponent<ParticleSystemRenderer>();

        penghuo_right_1 = Trans.FindObj(gameObject, "penhuo_right_1").GetComponent<ParticleSystemRenderer>();
        penghuo_right_2 = Trans.FindObj(gameObject, "penhuo_right_2").GetComponent<ParticleSystemRenderer>();
        penghuo_right_3 = Trans.FindObj(gameObject, "penhuo_right_3").GetComponent<ParticleSystemRenderer>();

        if (penghuo_left_1 == null || penghuo_left_2 == null || penghuo_left_2 == null ||
            penghuo_middle_1 == null || penghuo_middle_2 == null || penghuo_middle_3 == null ||
            penghuo_right_1 == null || penghuo_right_2 == null || penghuo_right_3 == null)
        {
            Debug.LogError("InitPengHuo null");
        }
    }

    void InitSparks()
    {
        sparks_left_1 = Trans.FindObj(gameObject, "locator1").transform.Find("spacecraft02_pao").gameObject;
        if (sparks_left_1 == null)
        {
            Debug.LogError("sparks_left_1 not found!!");
        }

        sparks_left_2 = Trans.FindObj(gameObject, "locator2").transform.Find("spacecraft02_pao").gameObject; ;
        if (sparks_left_2 == null)
        {
            Debug.LogError("sparks_left_2 not found!!");
        }

        sparks_right_1 = Trans.FindObj(gameObject, "locator3").transform.Find("spacecraft02_pao").gameObject; ;
        if (sparks_right_1 == null)
        {
            Debug.LogError("sparks_right_1 not found!!");
        }

        sparks_right_2 = Trans.FindObj(gameObject, "locator4").transform.Find("spacecraft02_pao").gameObject; ;
        if (sparks_right_2 == null)
        {
            Debug.LogError("sparks_right_2 not found!!");
        }

    }

    void SparkPlay()
    {
        for(int i = 0; i< sparks_left_1.transform.childCount;i++)
        {
            ParticleSystem effect = sparks_left_1.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }

        for (int i = 0; i < sparks_left_2.transform.childCount; i++)
        {
            ParticleSystem effect = sparks_left_2.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }

        for (int i = 0; i < sparks_right_1.transform.childCount; i++)
        {
            ParticleSystem effect = sparks_right_1.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }

        for (int i = 0; i < sparks_right_2.transform.childCount; i++)
        {
            ParticleSystem effect = sparks_right_2.transform.GetChild(i).GetComponent<ParticleSystem>();
            effect.Stop();
            effect.Play();
        }
    }



    public override void PlaySparksEffect()
    {
        SparkPlay();
    }


    public override IEnumerator LoadMaerialPo()
    {
        yield return new WaitForFixedUpdate();
        //------------------------材质球--------------------
        ship_normal = (Material)Resources.Load("character/spacecraft02/material/Copperhead_Desert");
        if (ship_normal == null)
        {
            Debug.LogError("not found ship_normal!!!");
        }

        ship_po = (Material)Resources.Load("character/spacecraft02/material/Copperhead_Desert-po");
        if (ship_po == null)
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
        ship_Body = Trans.FindObj(gameObject, "pasted__Body");
        if (ship_Body == null)
        {
            Debug.LogError("not found ship_Body!!!");
        }

        left_engine = Trans.FindObj(gameObject, "pasted__EngineLeft");
        if (left_engine == null)
        {
            Debug.LogError("not found left_engine!!!");
        }

        right_engine = Trans.FindObj(gameObject, "pasted__EngineRight");
        if (right_engine == null)
        {
            Debug.LogError("not found right_engine!!!");
        }

        ship_frame = Trans.FindObj(gameObject, "spacecraft02_zhuti");
        if (ship_frame == null)
        {
            Debug.LogError("not found ship_frame!!!");
        }

    }

    public override void ChangeState(FlySate state)
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

    public override void SetPenghuoValue(bool accelerate)
    {
        if (penghuo_left_1 == null || penghuo_middle_1 == null || penghuo_right_1 == null)
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

    public override void SetPenghuoDir(bool dir)
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
    public override void ChangeShipEffect(ShipAttri.ShipState state)
    {
        if (state == _attri.Shipstate)
        {
            return;
        }

        if (ship_frame == null || ship_smoke == null || ship_explosion == null
            || ship_po == null || ship_normal == null)
        {
            return;
        }
        switch (state)
        {
            case ShipAttri.ShipState.AS_IDLE:
                {
                    if (sub_smoke != null)
                    {
                        Destroy(sub_smoke);
                    }
                    if (sub_explosion != null)
                    {
                        Destroy(sub_explosion);
                    }
                    //还原正常的材质球
                    if (ship_Body.GetComponent<Renderer>().material != ship_normal)
                    {
                        ship_Body.GetComponent<Renderer>().material = ship_normal;
                    }

                    if (left_engine.GetComponent<Renderer>().material != ship_normal)
                    {
                        left_engine.GetComponent<Renderer>().material = ship_normal;
                    }

                    if (right_engine.GetComponent<Renderer>().material != ship_normal)
                    {
                        right_engine.GetComponent<Renderer>().material = ship_normal;
                    }
                    _attri.Shipstate = ShipAttri.ShipState.AS_DEAD;
                }
                break;

            case ShipAttri.ShipState.AS_SMOKE:
                {
                    if (sub_smoke != null)
                    {
                        return;
                    }
                    sub_smoke = GameObject.Instantiate(ship_smoke);
                    sub_smoke.transform.position = ship_frame.transform.position;
                    sub_smoke.transform.parent = ship_frame.transform;

                    //替换破损材质球
                    if (ship_Body.GetComponent<Renderer>().material != ship_po)
                    {
                        ship_Body.GetComponent<Renderer>().material = ship_po;
                    }

                    if (left_engine.GetComponent<Renderer>().material != ship_po)
                    {
                        left_engine.GetComponent<Renderer>().material = ship_po;
                    }

                    if (right_engine.GetComponent<Renderer>().material != ship_po)
                    {
                        right_engine.GetComponent<Renderer>().material = ship_po;
                    }
                    _attri.Shipstate = ShipAttri.ShipState.AS_SMOKE;
                }
                break;

            case ShipAttri.ShipState.AS_DEAD:
                {
                    if (sub_explosion != null)
                    {
                        return;
                    }
                    sub_explosion = GameObject.Instantiate(ship_explosion);
                    sub_explosion.transform.position = ship_frame.transform.position;
                    sub_explosion.transform.parent = ship_frame.transform;
                    _attri.Shipstate = ShipAttri.ShipState.AS_IDLE;

                    ShipDestroy();
                }
                break;
        }
    }


    protected override void Start () {

        base.Start();
        InitSparks();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}
}
