using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipAttri
{
    ShipBase myship;

    public enum ShipState
    {
        AS_IDLE,
        AS_SMOKE, //冒烟
        AS_DEAD //爆炸
    }

    public int Id=0;

    private int hp_max = -1;
    private int mp_max = -1;
    private int exp_max = 0;


    private int _hp = 0;
    private int _mp = 0;
    private int _level = 0;
    private int _exp = 0;


    public int TeamID;
    public int ShipID;
 //   public int reliveCount = 0;


    private ShipState _shipstate;

    private NavMeshAgent _agent; //导航

    private List<IShipController> _controllers = new List<IShipController>();

    public IShipController ShipController //控制器，为了方便兼容多种控制器
    {
        get
        {
            return _controllers.Count > 0 ? _controllers[0] : null;
        }
    }

    public bool TakeController(IShipController controller)
    {
        _controllers.Insert(0, controller);
        return true;
    }

    public void ReleaseController(IShipController controller)
    {
        _controllers.Remove(controller);
    }


    public int Hp
    {
        get
        {
            return _hp;
        }

        set
        {
            _hp = value;

            myship.SetHpProgressBar();

            if(_hp <=0)
            {
                _hp = 0;
                myship.ChangeShipEffect(ShipState.AS_DEAD);
            }
            else if (_hp <= HP_MAX * 0.3)
            {
                myship.ChangeShipEffect(ShipState.AS_SMOKE);
            }
            else
            {
                myship.ChangeShipEffect(ShipState.AS_IDLE);
            }

            

            Debug.Log("name:" + myship.name + "::Set_Hp--->, hp:" + _hp + ",HP_MAX:"+HP_MAX);
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }

        set
        {
            if(value > _level)
            {
                _level = value;

            }

            Debug.Log("name:" + myship.name +  ",_level:" + _level);
        }
    }

    public ShipState Shipstate
    {
        get
        {
            return _shipstate == 0 ? ShipState.AS_IDLE : _shipstate;
        }

        set
        {
            _shipstate = value;
        }
    }

    public int Exp
    {
        get
        {
            return _exp;
        }

        set
        {
            _exp = Mathf.Max(0,value);

            myship.SetExpProgressBar();

            Debug.Log("name:" + myship.name + ",Set_Exp::Exp:" + Exp + ",EXP_Max:" + EXP_Max);
        }
    }

    public int Mp
    {
        get
        {
            return _mp;
        }

        set
        {
            _mp = value;
        }
    }

    public int HP_MAX
    {
        get
        {
            return hp_max;
        }

        set
        {
            hp_max = Mathf.Max(0,value);

            myship.SetHpProgressBar();

            Debug.Log("name:" + myship.name + "::Set_HP_MAX--->HP:" + Hp + ",hP_MAX:" + hp_max);
        }
    }

    public int EXP_Max
    {
        get
        {
            return exp_max;
        }

        set
        {
            exp_max = value;

            myship.SetExpProgressBar();

            Debug.Log("name:" + myship.name + "::Set_EXP_Max--->Exp:" + Exp + ",EXP_Max:" + EXP_Max);
        }
    }


    public int MP_MAX
    {
        get
        {
            return mp_max;
        }

        set
        {
            mp_max = value;
        }
    }

    public void AttachShip(ShipBase ship)
    {
        myship = ship;
    }



}
