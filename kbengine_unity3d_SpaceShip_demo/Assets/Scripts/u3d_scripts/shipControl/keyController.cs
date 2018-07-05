using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyController : IShipController {

    // Use this for initialization
    private KeyMap _key = new KeyMap();

    private ShipBase _ship = null;

    void KeyMapBind()
    {
        _key.MoveForward = KeyCode.W;
        _key.MoveBack    = KeyCode.S;
        _key.RollLeft    = KeyCode.A;
        _key.RollRight   = KeyCode.D;
        _key.MoveJump    = KeyCode.Space;
        _key.Accelerate  = KeyCode.Mouse1;
        _key.Shotting    = KeyCode.Mouse0;
    }

    public void TargetAttached(ShipBase Target)
    {
        _ship = Target;
    }

    public void Start ()
    {
        KeyMapBind();
    }

    // Update is called once per frame
    public void Update ()
    {

        if (_ship == null)
        {
            return;
        }

        if (Input.GetKey(_key.Shotting))
        {
            //           Debug.Log("您按下了鼠标左键");

            for(int i=0;i<_ship.weapon_list.Count;i++)
            {
                _ship.OnLanuchWeapon(_ship.weapon_list[i]); 
            }
        }

        _ship.SetPenghuoValue(Input.GetKey(_key.Accelerate));
        

        Vector3 vposition = Vector3.zero;
        Vector3 vDirection = Vector3.zero;

        if (Input.GetKey(_key.Accelerate))
        {
            vposition += _ship.transform.forward *_ship.AccelerateSpeed;
            _ship.SetPenghuoDir(true);
        }
        else if (Input.GetKey(_key.MoveForward))
        {
            vposition += _ship.transform.forward * _ship.MoveSpeed;
            _ship.SetPenghuoDir(true);
        }

        if (Input.GetKey(_key.MoveBack))
        {
            vposition -= _ship.transform.forward * _ship.MoveSpeed;
            _ship.SetPenghuoDir(false);
        }


        if (Input.GetKey(_key.RollLeft))
        {
            vDirection.y += _ship.RollSpeed;
            _ship.ChangeState(ShipBase.FlySate.FLY_TURN_LEFT);
        }
        else if (Input.GetKey(_key.RollRight))
        {
            vDirection.y -= _ship.RollSpeed;
            _ship.ChangeState(ShipBase.FlySate.FLY_TURN_RIGHT);
        }
        else
        {
            _ship.ChangeState(ShipBase.FlySate.FLY_IDLE);
        }

        _ship.SetShipSpeed(vposition);
        _ship.SetShipRotation(vDirection);
    }
}
