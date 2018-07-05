using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship02 : ShipBase
{
    GameObject ship_explosion_around = null;

    GameObject weapon_born = null ,ship_cannon =null;
    public override void InitVariable()
    {
        base.InitVariable();

        MoveSpeed = 8.00f; //每秒的加速度
        AccelerateSpeed = 8.5f;
        RollSpeed = 20.5f;

        //因为设置的摩擦力系数是 1.5 最大速度不超过 7.0f
        SpeedMax = 100.0f;
        RollSpeedMax = 500.0f;

        camerOffset = new Vector3(0.0f, 30.1f, -49.9f);

//        weapon_list.Add(10001);
        weapon_list.Add(30001);
    }

    public override void InitPengHuo()
    {
        penghuo_left_1 = Trans.FindObj(gameObject, "penhuo_left_1").GetComponent<ParticleSystemRenderer>();
        penghuo_left_2 = Trans.FindObj(gameObject, "penhuo_left_2").GetComponent<ParticleSystemRenderer>();
        penghuo_left_3 = Trans.FindObj(gameObject, "penhuo_left_3").GetComponent<ParticleSystemRenderer>();

        penghuo_right_1 = Trans.FindObj(gameObject, "penhuo_right_1").GetComponent<ParticleSystemRenderer>();
        penghuo_right_2 = Trans.FindObj(gameObject, "penhuo_right_2").GetComponent<ParticleSystemRenderer>();
        penghuo_right_3 = Trans.FindObj(gameObject, "penhuo_right_3").GetComponent<ParticleSystemRenderer>();

        if (penghuo_left_1 == null || penghuo_left_2 == null || penghuo_left_2 == null ||
            penghuo_right_1 == null || penghuo_right_2 == null || penghuo_right_3 == null)
        {
            Debug.LogError("ship02::InitPengHuo null");
        }
    }

    public override void SetPenghuoValue(bool accelerate)
    {
        if (penghuo_left_1 == null || penghuo_right_1 == null)
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

        penghuo_right_1.lengthScale = value;
        penghuo_right_2.lengthScale = value;
        penghuo_right_3.lengthScale = value;
    }


    public override IEnumerator LoadMaerialPo()
    {
        yield return new WaitForFixedUpdate();
        //------------------------材质球--------------------
        ship_normal = (Material)Resources.Load("character/spacecraft04/material/spacecraft04");
        if (ship_normal == null)
        {
            Debug.LogError("not found ship_normal!!!");
        }

        ship_po = (Material)Resources.Load("character/spacecraft04/material/spacecraft04_po");
        if (ship_po == null)
        {
            Debug.LogError("not found ship_po!!!");
        }
        //-----------------------预制体-----------------------        
        ship_smoke = (GameObject)Resources.Load("character/spacecraft04/effect/prefab/spacecraft04_maoyan");
        if (ship_smoke == null)
        {
            Debug.LogError("not found ship_smoke!!!");
        }

        ship_explosion = (GameObject)Resources.Load("character/spacecraft04/effect/prefab/spacecraft04_baozha");
        if (ship_explosion == null)
        {
            Debug.LogError("not found ship_explosion!!!");
        }

        ship_cannon = (GameObject)Resources.Load("character/spacecraft04/effect/prefab/spacecraft04_zhupaoxuli");
        if (ship_cannon == null)
        {
            Debug.LogError("not found ship_cannon!!!");
        }

        //------------------挂载节点---------------------------
        ship_Body = Trans.FindObj(gameObject, "SF_DestroyerFBX_body"); //破损材质球节点
        if (ship_Body == null)
        {
            Debug.LogError("not found ship_Body!!!");
        }

        ship_frame = Trans.FindObj(gameObject, "body"); //爆炸特效挂在节点 
        if (ship_frame == null)
        {
            Debug.LogError("not found ship_frame!!!");
        }
        
        ship_explosion_around = Trans.FindObj(gameObject, "spacecraft04_baozhazoushen"); //周身爆炸特效节点
        if (ship_explosion_around == null)
        {
            Debug.LogError("not found ship_explosion_around!!!");
        }

        weapon_born = Trans.FindObj(gameObject, "weapon_born_30001"); //周身爆炸特效节点
        if (weapon_born == null)
        {
            Debug.LogError("not found weapon_born_30001!!!");
        }
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
                    if(ship_explosion_around != null)
                    {
                        ship_explosion_around.SetActive(false);
                    }
                    //还原正常的材质球
                    if (ship_Body.GetComponent<Renderer>().material != ship_normal)
                    {
                        ship_Body.GetComponent<Renderer>().material = ship_normal;
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

                    if (ship_explosion_around != null)
                    {
                        ship_explosion_around.SetActive(true);
                    }

                    //替换破损材质球
                    if (ship_Body.GetComponent<Renderer>().material != ship_po)
                    {
                        ship_Body.GetComponent<Renderer>().material = ship_po;
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

    public override void PlaySparksEffect()
    {
        GameObject explode_clone = GameObject.Instantiate(ship_cannon, weapon_born.transform.position, 
            weapon_born.transform.rotation, weapon_born.transform);

        if(explode_clone != null)
        {
            explode sript_explode = explode_clone.AddComponent<explode>();

            sript_explode.Duration = 0.8f;
        }
    }

    // Use this for initialization
    protected override void Start () {

        base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
    }
}
