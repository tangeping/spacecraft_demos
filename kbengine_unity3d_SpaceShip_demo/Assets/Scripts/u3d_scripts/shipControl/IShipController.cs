using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipController{

    // Use this for initialization
    void TargetAttached(ShipBase target);

    void Start();

    // Update is called once per frame
    void Update();
}
